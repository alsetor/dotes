using Application.Interfaces;
using Domain.Models;
using Newtonsoft.Json;

namespace Application.Services
{
    public  class TemplateService
    {
        private readonly ITemplateRepository _repository;

        public TemplateService(ITemplateRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Template>> GetAllTemplatesAsync()
            => _repository.GetAllAsync();

        public Task<Template?> GetTemplateByIdAsync(Guid id)
            => _repository.GetByIdAsync(id);

        public Task CreateTemplateAsync(Template template)
            => _repository.CreateAsync(template);

        public Task UpdateTemplateAsync(Template template)
            => _repository.UpdateAsync(template);

        public Task DeleteTemplateAsync(Guid id)
            => _repository.DeleteAsync(id);

        public async Task<List<Tag>> GetTagsFromTemplateAsync(Guid id)
        {
            var template = await _repository.GetByIdAsync(id);
            return JsonConvert.DeserializeObject<List<Tag>>(template.Tags);
        }
    }
}
