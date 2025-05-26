using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAPortal.API.DTOs;
using QAPortal.API.Models;
using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QAPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AnswersController(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> GetAnswers(int questionId)
        {
            var answers = await _context.Answers
                .Include(a => a.User)
                .Where(a => a.QuestionId == questionId)
                .OrderByDescending(a => a.IsAccepted)
                .ThenByDescending(a => a.VoteCount)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<AnswerDto>>(answers));
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateAnswer(CreateAnswerDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var question = await _context.Questions.FindAsync(model.QuestionId);
            if (question == null)
                return NotFound("Question not found");

            var answer = _mapper.Map<Answer>(model);
            answer.CreatedAt = DateTime.UtcNow;
            answer.UserId = user.Id;

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnswers), new { questionId = answer.QuestionId }, _mapper.Map<AnswerDto>(answer));
        }

        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(int id, UpdateAnswerDto model)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || answer.UserId != user.Id)
                return Unauthorized();

            _mapper.Map(model, answer);
            answer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || answer.UserId != user.Id)
                return Unauthorized();

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "User")]
        [HttpPost("{id}/accept")]
        public async Task<IActionResult> AcceptAnswer(int id)
        {
            var answer = await _context.Answers
                .Include(a => a.Question)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (answer == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || answer.Question.UserId != user.Id)
                return Unauthorized();

            var question = answer.Question;
            var previouslyAcceptedAnswer = await _context.Answers
                .FirstOrDefaultAsync(a => a.QuestionId == question.Id && a.IsAccepted);

            if (previouslyAcceptedAnswer != null)
                previouslyAcceptedAnswer.IsAccepted = false;

            answer.IsAccepted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "User")]
        [HttpPost("{id}/vote")]
        public async Task<IActionResult> VoteAnswer(int id, [FromQuery] bool isUpvote)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
                return NotFound();

            answer.VoteCount += isUpvote ? 1 : -1;
            await _context.SaveChangesAsync();

            return Ok(new { voteCount = answer.VoteCount });
        }
    }
}