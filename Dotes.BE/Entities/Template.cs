using System;
using System.Collections.Generic;

namespace Dotes.BE.Entities
{
    public class Template
    {
        public long Id { get; set; }
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public string FileBase64 { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public TemplateType Type { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
