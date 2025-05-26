using AutoMapper;
using QAPortal.API.Models;
using QAPortal.API.DTOs;
namespace QAPortal.API.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            // Question mappings
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.AnswerCount, opt => opt.MapFrom(src => src.Answers.Count))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag.Name)))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<CreateQuestionDto, Question>();
            CreateMap<UpdateQuestionDto, Question>();

            // Answer mappings
            CreateMap<Answer, AnswerDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName));

            CreateMap<CreateAnswerDto, Answer>();
            CreateMap<UpdateAnswerDto, Answer>();

            // Tag mappings
            CreateMap<Tag, TagDto>();
            CreateMap<CreateTagDto, Tag>();
            CreateMap<UpdateTagDto, Tag>();

            // User mappings
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Token, opt => opt.Ignore());
        }
    }
} 