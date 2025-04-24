using Domain.Models;

namespace Application.Interfaces
{
    public interface ITemplateRepository
    {
        Task<List<Template>> GetAllAsync();
        Task<Template?> GetByIdAsync(Guid id);
        Task CreateAsync(Template template);
        Task UpdateAsync(Template template);
        Task DeleteAsync(Guid id);
    }
}
