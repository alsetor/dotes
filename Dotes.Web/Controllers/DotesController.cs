using System;
using Dotes.BL.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Templates.Web.Models;

namespace Templates.Web.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DotesController : ControllerBase
    {
        private readonly ITemplatesBL _templatesBL;

        public DotesController(ITemplatesBL templatesBl)
        {
            _templatesBL = templatesBl;
        }

        /// <summary>
        /// Return generated document by template uid
        /// </summary>
        /// <param name="model">Model with filled tags</param>
        [SwaggerOperation(Summary = "Return generated document by template uid")]
        [HttpPost]
        public IActionResult GenerateDocument([FromBody] GenerateDocumentModel model)
        {
            var result = _templatesBL.GenerateDocument(model.TemplateUid, model.Tags);
            return Ok(new { documentBody = Convert.ToBase64String(result) });
        }

        /// <summary>
        /// Return template with tags by uid
        /// </summary>
        [SwaggerOperation(Summary = "Return template with tags by uid")]
        [HttpGet("{uid}")]
        public IActionResult GetTemplateByUid(Guid uid)
        {
            var template = _templatesBL.GetTemplateByUid(uid);
            return Ok(template);
        }

        [SwaggerOperation(Summary = "Return template tags by template uid")]
        [HttpGet("{uid}")]
        public IActionResult GetTagsByTemplateUid(Guid uid)
        {
            var tags = _templatesBL.GetTagsByTemplateUid(uid);
            return Ok(tags);
        }
    }
}
