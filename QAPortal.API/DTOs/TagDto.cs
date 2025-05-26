using System;
using System.ComponentModel.DataAnnotations;

namespace QAPortal.API.DTOs
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateTagDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }

    public class UpdateTagDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
} 