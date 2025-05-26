using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QAPortal.API.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<QuestionTag> QuestionTags { get; set; }
    }
}