using Dotes.BE.Entities;
using System.Collections.Generic;

namespace Dotes.DAL.TemplateTypes
{
    public interface ITemplateTypeRepository
    {
        bool CreateTemplateType(TemplateType type);
        List<TemplateType> GetTemplateTypes();
        bool UpdateTemplateType(TemplateType type);
    }
}