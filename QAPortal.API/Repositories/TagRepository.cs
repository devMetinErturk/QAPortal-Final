using Microsoft.EntityFrameworkCore;
using QAPortal.API.Models;

namespace QAPortal.API.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Tag> CreateAsync(Tag tag)
        {
            await AddAsync(tag);
            await SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> UpdateAsync(Tag tag)
        {
            await base.UpdateAsync(tag);
            await SaveChangesAsync();
            return tag;
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await GetByIdAsync(id);
            if (tag != null)
            {
                await base.DeleteAsync(tag);
                await SaveChangesAsync();
            }
        }
    }
} 