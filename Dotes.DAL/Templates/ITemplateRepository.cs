using Dotes.BE.Entities;
using System;
using System.Collections.Generic;

namespace Dotes.DAL.Templates
{
    public interface ITemplateRepository
    {
        bool CreateTemplate(Template template);
        Template GetTemplateById(long id);
        Template GetTemplateByUid(Guid uid);
        List<Template> GetTemplates(int? typeId = null);
        bool UpdateTemplate(Template template);
        bool SetTemplateAsDeleted(long id);
    }
}