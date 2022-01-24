using System;
using System.Collections.Generic;
using System.Linq;
using AuthService.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AuthService.BE
{
    public class KndUserInfo
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string RankName { get; set; }

        public string OfficialPositionName { get; set; }

        public Guid UnitId { get; set; }

        public Guid? SubjectUnitId { get; set; }

        public string UnitCodeNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UnitCode UnitCode { get; set; }

        public string SubjectCode { get; set; }

        public Rank Rank { get; set; }

        public OfficialPosition OfficialPosition { get; set; }

        public UnitType UnitType { get; set; }

        public string UnitName { get; set; }

        public List<KndRoles> Roles { get; set; }

        public Guid Id { get; set; }

        public bool InquiryFunction { get; set; }

        public string FIO => $"{LastName} {FirstName} {Patronymic}";

        public string Login => $"{LastName}{(string.IsNullOrEmpty(FirstName) ? "" : FirstName[0].ToString())}{(string.IsNullOrEmpty(Patronymic) ? "" : Patronymic[0].ToString())}";

        public bool IsSheff => Roles != null &&
            Roles.Any(x => x == KndRoles.ROLE_HEAD);

        public string Post => OfficialPositionName;

        public string SurnameAndInitials => $"{LastName}{(string.IsNullOrEmpty(FirstName) ? "" : $" {FirstName[0]}.")} {(string.IsNullOrEmpty(Patronymic) ? "" : $" {Patronymic[0]}.")}";

        public static KndUserInfo MapToUserInfo(KndUserInfoResponse response)
        {
            return response != null ? new KndUserInfo
            {
                LastName = response.LastName,
                FirstName = response.FirstName,
                Patronymic = response.Patronymic,
                RankName = response.Rank?.Name,
                OfficialPositionName = response.OfficialPosition?.Name,
                UnitId = response.Unit?.Id ?? response.UnitId,
                UnitCodeNumber = response.UnitCode,
                UnitCode = response.Unit?.UnitTypeDict?.Code ?? UnitCode.Undefined,
                SubjectUnitId = response.Unit?.UnitTypeDict?.Code == UnitCode.S3 ? response.Unit.Parent.Id : (Guid?)null,
                SubjectCode = response.SubjectCode,
                Rank = response.Rank,
                OfficialPosition = response.OfficialPosition,
                UnitType = response.UnitType,
                UnitName = response.Unit?.ShortName ?? response.UnitName,
                Roles = response.Roles,
                Id = response.Id,
                InquiryFunction = response.InquiryFunction ?? false,
            } : null;
        }
    }

    public class CaseForms
    {
        public string Genitive { get; set; }
        public string Dative { get; set; }
        public string Ablative { get; set; }
        public string Accusative { get; set; }
        public string Prepositional { get; set; }
    }

    public class Rank
    {
        public string Id { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Position { get; set; }
        public CaseForms CaseForms { get; set; }
    }

    public class OfficialPosition
    {
        public string Id { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Position { get; set; }
        public CaseForms CaseForms { get; set; }
    }

    public class Parent
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }

    public class NameCaseForms
    {
        public object Genitive { get; set; }
        public object Dative { get; set; }
        public object Ablative { get; set; }
        public object Accusative { get; set; }
        public object Prepositional { get; set; }
    }

    public class WorkPhone
    {
        public string Number { get; set; }
    }

    public class UnitType
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public UnitCode Code { get; set; }
        public string ShortName { get; set; }
    }
}
