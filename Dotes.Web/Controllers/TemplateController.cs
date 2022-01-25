using System;
using System.Linq;
using Dotes.BE.Entities;
using Dotes.BL.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Templates.Web.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private const string ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        private readonly ITemplatesBL _templatesBL;

        public TemplateController(ITemplatesBL templatesBl)
        {
            _templatesBL = templatesBl;
        }

        [HttpGet("GetTemplates")]
        public IActionResult GetTemplates(int? typeId = null)
        {
            return Ok(_templatesBL.GetTemplates(typeId));
        }

        [HttpGet("GetTemplate")]
        public IActionResult GetTemplate(long id)
        {
            var model = _templatesBL.GetTemplateById(id);
            return Ok(model);
        }

        [HttpGet("GetFileByTemplateId")]
        public IActionResult GetFileByTemplateId(long templateId)
        {
            var file = _templatesBL.GetFileByTemplateId(templateId);
            return File(file, ContentType);
        }

        [HttpPost("CreateTemplate")]
        public IActionResult CreateTemplate([FromBody] Template model)
        {
            model.File = model.FileBase64.Split(',').Length > 1 ? Convert.FromBase64String(model.FileBase64.Split(',')[1]) : new byte[0];
            var id = _templatesBL.CreateTemplate(model);
            return Ok(id);
        }

        [HttpPost("UpdateTemplate")]
        public IActionResult UpdateTemplate([FromBody] Template model)
        {
            model.File = model.FileBase64.Split(',').Length > 1 ? Convert.FromBase64String(model.FileBase64.Split(',')[1]) : new byte[0];
            var result = _templatesBL.UpdateTemplate(model);
            return Ok(result);
        }

        [HttpPost("DeleteTemplate")]
        public IActionResult DeleteTemplate([FromBody] Template model)
        {
            var result = _templatesBL.SetTemplateDeleted(model.Id);
            return Ok(result);
        }

        [HttpPost("CreateTemplateType")]
        public IActionResult CreateTemplateType([FromBody] TemplateType type)
        {
            var result = _templatesBL.CreateTemplateType(type);
            return Ok(result);
        }

        [HttpGet("GetTemplateTypes")]
        public IActionResult GetTemplateTypes()
        {
            var result = _templatesBL.GetTemplateTypes();
            return Ok(result.OrderBy(r => r.Name));
        }

        [HttpPut("UpdateTemplateType")]
        public IActionResult UpdateTemplateType([FromBody] TemplateType type)
        {
            var result = _templatesBL.UpdateTemplateType(type);
            return Ok(result);
        }
    }
}
