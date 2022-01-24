using Dotes.BE.Enums;
using System.Collections.Generic;

namespace Dotes.BE.Entities
{
    public class Tag
    {
        public TagType Type { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<string> CellNames { get; set; }
    }
}