using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAPortal.API.DTOs;
using QAPortal.API.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public QuestionsController(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _context.Questions
                .Include(q => q.User)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .Include(q => q.Answers)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<QuestionDto>>(questions));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(QuestionDto), 201)]
        public async Task<IActionResult> GetQuestion(int id)
        {
            var question = await _context.Questions
                .Include(q => q.User)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .Include(q => q.Answers)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return NotFound();

            question.ViewCount++;
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<QuestionDto>(question));
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateQuestion(CreateQuestionDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            if (model.Tags == null || model.Tags.Count == 0)
                return BadRequest("At least one tag is required");

            if (model.Tags.Count > 5)
                return BadRequest("Maximum 5 tags allowed");

            var question = _mapper.Map<Question>(model);
            question.CreatedAt = DateTime.UtcNow;
            question.UserId = user.Id;
            question.QuestionTags = new List<QuestionTag>();

            foreach (var tagName in model.Tags)
            {
                if (string.IsNullOrWhiteSpace(tagName))
                    continue;

                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());
                if (tag == null)
                {
                    tag = new Tag { Name = tagName.Trim() };
                    _context.Tags.Add(tag);
                }

                question.QuestionTags.Add(new QuestionTag { Question = question, Tag = tag });
            }

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, _mapper.Map<QuestionDto>(question));
        }

        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, UpdateQuestionDto model)
        {
            var question = await _context.Questions
                .Include(q => q.QuestionTags)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || question.UserId != user.Id)
                return Unauthorized();

            if (model.Tags == null || model.Tags.Count == 0)
                return BadRequest("At least one tag is required");

            if (model.Tags.Count > 5)
                return BadRequest("Maximum 5 tags allowed");

            _mapper.Map(model, question);
            question.UpdatedAt = DateTime.UtcNow;

            question.QuestionTags.Clear();
            foreach (var tagName in model.Tags)
            {
                if (string.IsNullOrWhiteSpace(tagName))
                    continue;

                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());
                if (tag == null)
                {
                    tag = new Tag { Name = tagName.Trim() };
                    _context.Tags.Add(tag);
                }

                question.QuestionTags.Add(new QuestionTag { Question = question, Tag = tag });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || question.UserId != user.Id)
                return Unauthorized();

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
