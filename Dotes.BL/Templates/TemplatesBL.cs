using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TemplateEngine.Docx;
using Dotes.DAL.Templates;
using Dotes.DAL.TemplateTypes;
using Dotes.BE.Entities;
using Dotes.BE.Enums;

namespace Dotes.BL.Templates
{
    public class TemplatesBL : ITemplatesBL
    {
        private readonly ITemplateTypeRepository _templateTypeRepository;
        private readonly ITemplateRepository _templateRepository;

        public TemplatesBL(
            ITemplateRepository templateRepository,
            ITemplateTypeRepository templateTypeRepository)
        {
            _templateRepository = templateRepository;
            _templateTypeRepository = templateTypeRepository;
        }

        public List<Template> GetTemplates(int? typeId = null)
        {
            return _templateRepository.GetTemplates(typeId);
        }

        public Template GetTemplateById(long id)
        {
            return _templateRepository.GetTemplateById(id);
        }

        public Template GetTemplateByUid(Guid uid)
        {
            return _templateRepository.GetTemplateByUid(uid);
        }

        public bool CreateTemplate(Template template)
        {
            return _templateRepository.CreateTemplate(template);
        }

        public bool UpdateTemplate(Template template)
        {
            return _templateRepository.UpdateTemplate(template);
        }

        public List<Tag> GetTagsByTemplateUid(Guid templateUid)
        {
            var template = _templateRepository.GetTemplateByUid(templateUid);
            return template.Tags.Where(x => x != null).ToList();
        }

        public bool SetTemplateDeleted(long id)
        {
            return _templateRepository.SetTemplateAsDeleted(id);
        }

        public byte[] GenerateDocument(Guid templateUid, List<TagValue> tagValues = null)
        {
            var template = _templateRepository.GetTemplateByUid(templateUid);
            if (template == null) return null;

            var tags = GetTagsByTemplateUid(templateUid);
            var content = GetTemplateContent(tagValues, tags);
            var file = ApplyTemplateContent(template.File, content);

            return file;
        }        

        public byte[] GetFileByTemplateId(long id)
        {
            return _templateRepository.GetTemplateById(id).File;
        }

        public bool CreateTemplateType(TemplateType type)
        {
            return _templateTypeRepository.CreateTemplateType(type);
        }

        public List<TemplateType> GetTemplateTypes()
        {
            return _templateTypeRepository.GetTemplateTypes();
        }

        public bool UpdateTemplateType(TemplateType type)
        {
            return _templateTypeRepository.UpdateTemplateType(type);
        }

        public bool DeleteTemplateType(long id)
        {
            return _templateTypeRepository.DeleteTemplateType(id);
        }

        private Content GetTemplateContent(List<TagValue> tagValues, List<Tag> tags)
        {
            var content = new Content();
            try
            {
                foreach (var tagValue in tagValues.Where(x => !string.IsNullOrEmpty(x.Name)))
                {
                    if (!tags.Any(x => x.Name.Equals(tagValue.Name) && x.Type == tagValue.Type)) continue;

                    switch (tagValue.Type)
                    {
                        case TagType.Table:
                            AddTableContent(content, tagValue, tags);
                            break;
                        case TagType.Image:
                            AddImageContent(content, tagValue, tags);
                            break;
                        default:
                            AddFieldContent(content, tagValue, tags);
                            break;
                    }
                }
                return content;
            }
            catch
            {
                return new Content();
            }
        }

        private void AddFieldContent(Content content, TagValue tagValue, List<Tag> tags)
        {
            content.Fields.Add(new FieldContent(tagValue.Name, tagValue.Value ?? ""));
        }

        private void AddImageContent(Content content, TagValue tagValue, List<Tag> tags)
        {
            content.Images.Add(new ImageContent(tagValue.Name, Convert.FromBase64String(tagValue.Value)));
        }

        private void AddTableContent(Content content, TagValue tagValue, List<Tag> tags)
        {
            var table = new TableContent(tagValue.Name);
            var tag = tags.FirstOrDefault(t => t.Name == tagValue.Name);
            var tableValues = JsonConvert.DeserializeObject<List<List<string>>>(tagValue.Value);

            var tableRowContentList = tableValues
                .Select(row => row.Select((t, index) => new FieldContent(tag.CellNames[index], t)).ToList())
                .Select(fieldContentList => new TableRowContent(fieldContentList)).ToList();

            table.Rows = tableRowContentList;
            content.Tables.Add(table);
        }

        private byte[] ApplyTemplateContent(byte[] templateData, Content content)
        {
            byte[] result;

            var tempDestinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempTemplateFile");
            File.WriteAllBytes(tempDestinationPath, templateData);

            using (var ms = new MemoryStream())
            {
                using (var doc = new TemplateProcessor(tempDestinationPath).SetRemoveContentControls(true).SetNoticeAboutErrors(false))
                {
                    doc.FillContent(content);
                    doc.SaveChanges();
                    doc.Dispose();
                }
                result = File.ReadAllBytes(tempDestinationPath);
                ms.Flush();
            }

            return result;
        }
    }
}