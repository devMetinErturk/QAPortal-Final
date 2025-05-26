using Microsoft.EntityFrameworkCore;
using QAPortal.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QAPortal.API.Repositories
{
    public class AnswerRepository : Repository<Answer>
    {
        public AnswerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Answer> CreateAsync(Answer answer)
        {
            await AddAsync(answer);
            await SaveChangesAsync();
            return answer;
        }

        public async Task<Answer> UpdateAsync(Answer answer)
        {
            await base.UpdateAsync(answer);
            await SaveChangesAsync();
            return answer;
        }

        public async Task DeleteAsync(int id)
        {
            var answer = await GetByIdAsync(id);
            if (answer != null)
            {
                await base.DeleteAsync(answer);
                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Answer>> GetAnswersWithDetailsAsync()
        {
            return await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .ToListAsync();
        }
    }
} 