using System;
using System.Collections.Generic;
using Dotes.BE.Entities;

namespace Dotes.BL.Templates
{
    public interface ITemplatesBL
    {
        List<Template> GetTemplates(int? type = null);
        Template GetTemplateById(long id);
        Template GetTemplateByUid(Guid uid);
        byte[] GetFileByTemplateId(long id);
        bool CreateTemplate(Template template);
        bool UpdateTemplate(Template template);
        List<Tag> GetTagsByTemplateUid(Guid templateUid);
        bool SetTemplateDeleted(long id);
        byte[] GenerateDocument(Guid templateUid, List<TagValue> tagValues = null);
        bool CreateTemplateType(TemplateType type);
        List<TemplateType> GetTemplateTypes();
        bool UpdateTemplateType(TemplateType type);
    }
}