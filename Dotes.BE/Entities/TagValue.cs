using Dotes.BE.Enums;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Dotes.BE.Entities
{
    public class TagValue
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TagType Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
