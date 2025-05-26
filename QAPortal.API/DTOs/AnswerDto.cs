using System;

namespace QAPortal.API.DTOs
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public int VoteCount { get; set; }
        public bool IsAccepted { get; set; }
        public int QuestionId { get; set; }

    }

    public class CreateAnswerDto
    {
        public string Content { get; set; }
        public int QuestionId { get; set; }
    }

    public class UpdateAnswerDto
    {
        public string Content { get; set; }
    }
}