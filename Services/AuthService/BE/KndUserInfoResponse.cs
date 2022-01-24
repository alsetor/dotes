using System;
using System.Collections.Generic;
using AuthService.Enums;

namespace AuthService.BE
{
    public class KndUserInfoResponse
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public Guid UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string SubjectCode { get; set; }
        public UnitType UnitType { get; set; }
        public bool? InquiryFunction { get; set; }
        public Rank Rank { get; set; }
        public OfficialPosition OfficialPosition { get; set; }
        public Unit Unit { get; set; }
        public List<KndRoles> Roles { get; set; }
        public Guid Id { get; set; }
    }
}
