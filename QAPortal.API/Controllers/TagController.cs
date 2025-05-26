using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAPortal.API.DTOs;
using QAPortal.API.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TagsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
        {
            var tags = await _context.Tags.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<TagDto>>(tags));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDto>> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
                return NotFound();

            return Ok(_mapper.Map<TagDto>(tag));
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult<TagDto>> CreateTag(CreateTagDto model)
        {
            if (await _context.Tags.AnyAsync(t => t.Name == model.Name))
                return BadRequest("Tag with this name already exists");

            var tag = _mapper.Map<Tag>(model);
            tag.CreatedAt = DateTime.UtcNow;
            
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, _mapper.Map<TagDto>(tag));
        }

        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, UpdateTagDto model)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
                return NotFound();

            if (await _context.Tags.AnyAsync(t => t.Name == model.Name && t.Id != id))
                return BadRequest("Tag with this name already exists");

            _mapper.Map(model, tag);
            tag.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags
                .Include(t => t.QuestionTags)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
                return NotFound();

            if (tag.QuestionTags.Any())
                return BadRequest("Cannot delete tag that is associated with questions");

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 