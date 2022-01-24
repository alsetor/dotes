using System;
using AuthService.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AuthService.BE
{
    public class UnitTypeDict
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UnitCode Code { get; set; }
        public string ShortName { get; set; }
    }    
}
