using Application.Interfaces;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly AppDbContext _context;

        public TemplateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Template template)
        {
            _context.Templates.Add(template);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template != null)
            {
                _context.Templates.Remove(template);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Template>> GetAllAsync()
        {
            return await _context.Templates.ToListAsync();
        }

        public async Task<Template?> GetByIdAsync(Guid id)
        {
            return await _context.Templates.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(Template template)
        {
            var existingTemplate = await _context.Templates.FindAsync(template.Id);

            existingTemplate.Name = template.Name;
            existingTemplate.FileName = template.FileName;
            existingTemplate.Tags = template.Tags;
            existingTemplate.File = template.File;

            _context.Templates.Update(existingTemplate);
            await _context.SaveChangesAsync();
        }
    }
}
