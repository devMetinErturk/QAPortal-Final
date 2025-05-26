using QAPortal.API.DTOs;
using QAPortal.API.Models;
using QAPortal.API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QAPortal.API.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<TagDto> GetTagByIdAsync(int id);
        Task<TagDto> CreateTagAsync(CreateTagDto createTagDTO);
        Task<TagDto> UpdateTagAsync(int id, UpdateTagDto updateTagDTO);
        Task DeleteTagAsync(int id);
    }

    public class TagService : ITagService
    {
        private readonly TagRepository _tagRepository;

        public TagService(TagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });
        }

        public async Task<TagDto> GetTagByIdAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return null;

            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt
            };
        }

        public async Task<TagDto> CreateTagAsync(CreateTagDto createTagDTO)
        {
            var tag = new Tag
            {
                Name = createTagDTO.Name,
                Description = createTagDTO.Description
            };

            var createdTag = await _tagRepository.CreateAsync(tag);
            return new TagDto
            {
                Id = createdTag.Id,
                Name = createdTag.Name,
                Description = createdTag.Description,
                CreatedAt = createdTag.CreatedAt,
                UpdatedAt = createdTag.UpdatedAt
            };
        }

        public async Task<TagDto> UpdateTagAsync(int id, UpdateTagDto updateTagDTO)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return null;

            tag.Name = updateTagDTO.Name;
            tag.Description = updateTagDTO.Description;
            tag.UpdatedAt = DateTime.UtcNow;

            var updatedTag = await _tagRepository.UpdateAsync(tag);
            return new TagDto
            {
                Id = updatedTag.Id,
                Name = updatedTag.Name,
                Description = updatedTag.Description,
                CreatedAt = updatedTag.CreatedAt,
                UpdatedAt = updatedTag.UpdatedAt
            };
        }

        public async Task DeleteTagAsync(int id)
        {
            await _tagRepository.DeleteAsync(id);
        }
    }
} 