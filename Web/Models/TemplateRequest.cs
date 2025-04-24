using System.Collections.Generic;
using System;
using Domain.Models;

namespace Web.Models
{
    public class TemplateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public List<Tag> Tags { get; set; }
        public string FileBase64 { get; set; }
    }
}
