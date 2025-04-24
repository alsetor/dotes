using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Application.Services;
using Application.Interfaces;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DotesController : ControllerBase
    {
        private readonly TemplateService _service;
        private readonly IDocumentGenerator _documentGenerator;

        public DotesController(TemplateService service, IDocumentGenerator documentGenerator)
        {
            _service = service;
            _documentGenerator = documentGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDocument([FromBody] GenerateDocumentModel model)
        {
            var tags = await _service.GetTagsFromTemplateAsync(model.TemplateId);
            var template = await _service.GetTemplateByIdAsync(model.TemplateId);
            var result = _documentGenerator.GenerateDocument(template.File, tags, model.Tags);
            return Ok(new { documentBody = Convert.ToBase64String(result) });
        }
    }
}
