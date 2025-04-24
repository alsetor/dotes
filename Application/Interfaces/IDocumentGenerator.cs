using Domain.Models;

namespace Application.Interfaces
{
    public interface IDocumentGenerator
    {
        byte[] GenerateDocument(byte[] templateFile, List<Tag> tags, List<TagValue> values);
    }
}
