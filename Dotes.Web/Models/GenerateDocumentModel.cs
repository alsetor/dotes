using Dotes.BE.Entities;
using System;
using System.Collections.Generic;

namespace Templates.Web.Models
{
    public class GenerateDocumentModel
    {
        public Guid TemplateUid { get; set; }
        public List<TagValue> Tags { get; set; }
    }
}