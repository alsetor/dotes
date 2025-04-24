using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services;
using Domain.Models;
using Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly TemplateService _service;

        private const string ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public TemplateController(TemplateService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Template>>> GetAll()
        {
            var templates = await _service.GetAllTemplatesAsync();
            return Ok(templates);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Template>> GetById(Guid id)
        {
            var template = await _service.GetTemplateByIdAsync(id);
            if (template == null) return NotFound();

            var templateRequest = new TemplateRequest()
            {
                Id = id,
                FileName = template.FileName,
                Name = template.Name,
                FileBase64 = $"data:{ContentType};base64,{Convert.ToBase64String(template.File)}",
                Tags = JsonConvert.DeserializeObject<List<Tag>>(template.Tags),
            };

            return Ok(templateRequest);
        }

        [HttpGet("{id:guid}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            var template = await _service.GetTemplateByIdAsync(id);
            if (template == null || template.File == null)
                return NotFound();

            var fileName = $"{template.FileName}.docx";

            return File(template.File, ContentType, fileName);
        }

        [HttpPost("CreateTemplate")]
        public async Task<ActionResult> CreateTemplate([FromBody] TemplateRequest model)
        {
            var template = new Template()
            {
                File = model.FileBase64.Split(',').Length > 1 ? Convert.FromBase64String(model.FileBase64.Split(',')[1]) : new byte[0],
                FileName = model.FileName,
                Name = model.Name,
                Tags = JsonConvert.SerializeObject(model.Tags)
            };
            await _service.CreateTemplateAsync(template);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpPost("UpdateTemplate")]
        public async Task<ActionResult> UpdateTemplate([FromBody] TemplateRequest model)
        {
            var existing = await _service.GetTemplateByIdAsync(model.Id);
            if (existing == null)
                return NotFound();

            var template = new Template()
            {
                Id = model.Id,
                File = model.FileBase64.Split(',').Length > 1 ? Convert.FromBase64String(model.FileBase64.Split(',')[1]) : new byte[0],
                FileName = model.FileName,
                Name = model.Name,
                Tags = JsonConvert.SerializeObject(model.Tags)
            };

            await _service.UpdateTemplateAsync(template);
            return NoContent();
        }

        [HttpGet("{id:guid}/tags")]
        public async Task<IActionResult> GetTagsFromTemplate(Guid id)
        {
            var tags = await _service.GetTagsFromTemplateAsync(id);
            return Ok(tags);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existing = await _service.GetTemplateByIdAsync(id);
            if (existing == null) return NotFound();

            await _service.DeleteTemplateAsync(id);
            return NoContent();
        }
    }
}
