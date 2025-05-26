using System;
using System.Collections.Generic;

namespace QAPortal.API.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public int ViewCount { get; set; }
        public int VoteCount { get; set; }
        public int AnswerCount { get; set; }
        public List<string> Tags { get; set; }
        public List<AnswerDto> Answers { get; set; }


    }

    public class CreateQuestionDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }

    public class UpdateQuestionDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }
}