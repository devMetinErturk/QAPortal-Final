using Microsoft.EntityFrameworkCore;
using QAPortal.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QAPortal.API.Repositories
{
    public class QuestionRepository : Repository<Question>
    {
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Question> CreateAsync(Question question)
        {
            await AddAsync(question);
            await SaveChangesAsync();
            return question;
        }

        public async Task<Question> UpdateAsync(Question question)
        {
            await base.UpdateAsync(question);
            await SaveChangesAsync();
            return question;
        }

        public async Task DeleteAsync(int id)
        {
            var question = await GetByIdAsync(id);
            if (question != null)
            {
                await base.DeleteAsync(question);
                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Question>> GetQuestionsWithDetailsAsync()
        {
            return await _context.Questions
                .Include(q => q.User)
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                .Include(q => q.Answers)
                .ToListAsync();
        }
    }
} 