using System;
using System.Linq;
using Dotes.BE.Entities;
using Dotes.BL.Templates;
using Microsoft.AspNetCore.Mvc;

namespace Templates.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private const string ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        private readonly ITemplatesBL _templatesBL;

        public TemplateController(ITemplatesBL templatesBl)
        {
            _templatesBL = templatesBl;
        }

        [HttpGet]
        public IActionResult GetTemplates(int? typeId = null)
        {
            return Ok(_templatesBL.GetTemplates(typeId));
        }

        [HttpGet("{id}")]
        public IActionResult GetTemplate(long id)
        {
            var model = _templatesBL.GetTemplateById(id);
            return Ok(model);
        }

        [HttpGet("{templateId}")]
        public IActionResult GetFileByTemplateId(long templateId)
        {
            var file = _templatesBL.GetFileByTemplateId(templateId);
            return File(file, ContentType);
        }

        [HttpPost]
        public IActionResult CreateTemplate([FromBody] Template model)
        {
            model.File = model.FileBase64.Split(',').Length > 1 ? Convert.FromBase64String(model.FileBase64.Split(',')[1]) : new byte[0];
            var id = _templatesBL.CreateTemplate(model);
            return Ok(id);
        }

        [HttpPost]
        public IActionResult UpdateTemplate([FromBody] Template model)
        {
            model.File = model.FileBase64.Split(',').Length > 1 ? Convert.FromBase64String(model.FileBase64.Split(',')[1]) : new byte[0];
            var result = _templatesBL.UpdateTemplate(model);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult DeleteTemplate([FromBody] Template model)
        {
            var result = _templatesBL.SetTemplateDeleted(model.Id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateTemplateType([FromBody] TemplateType type)
        {
            var result = _templatesBL.CreateTemplateType(type);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetTemplateTypes()
        {
            var result = _templatesBL.GetTemplateTypes();
            return Ok(result.OrderBy(r => r.Name));
        }

        [HttpPut]
        public IActionResult UpdateTemplateType([FromBody] TemplateType type)
        {
            var result = _templatesBL.UpdateTemplateType(type);
            return Ok(result);
        }
    }
}
