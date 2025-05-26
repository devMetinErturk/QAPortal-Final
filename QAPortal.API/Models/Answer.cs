using System;
using System.ComponentModel.DataAnnotations;

namespace QAPortal.API.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int VoteCount { get; set; }
        public bool IsAccepted { get; set; }
    }
}