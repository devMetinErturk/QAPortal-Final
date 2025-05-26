using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QAPortal.API.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public ICollection<QuestionTag> QuestionTags { get; set; }
        public int ViewCount { get; set; }
        public int VoteCount { get; set; }
    }
}