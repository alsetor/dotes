using System;
using System.Collections.Generic;

namespace AuthService.BE
{
    public class Unit
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public Parent Parent { get; set; }
        public object WorkMail { get; set; }
        public object Website { get; set; }
        public object Okato { get; set; }
        public NameCaseForms NameCaseForms { get; set; }
        public object ShortNameCaseForms { get; set; }
        public object Helpline { get; set; }
        public List<WorkPhone> WorkPhones { get; set; }
        public UnitTypeDict UnitTypeDict { get; set; }
        public string SubjectCode { get; set; }
        public string UnitCode { get; set; }
        public string EpguCode { get; set; }
        public int IncomingFsRequestNumber { get; set; }
        public List<object> UnitAddresses { get; set; }
        public object Children { get; set; }
    }
}
