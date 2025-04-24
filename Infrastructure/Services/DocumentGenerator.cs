using Application.Interfaces;
using Domain.Models;
using Newtonsoft.Json;
using TemplateEngine.Docx;

namespace Infrastructure.Services
{
    public class DocumentGenerator : IDocumentGenerator
    {
        public byte[] GenerateDocument(byte[] templateFile, List<Tag> tags, List<TagValue> tagValues)
        {
            var content = GetTemplateContent(tagValues, tags);
            return ApplyTemplateContent(templateFile, content);
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
