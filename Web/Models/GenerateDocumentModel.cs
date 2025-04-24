using System;
using System.Collections.Generic;
using Domain.Models;

namespace Web.Models
{
    public class GenerateDocumentModel
    {
        public Guid TemplateId { get; set; }
        public List<TagValue> Tags { get; set; }
    }
}