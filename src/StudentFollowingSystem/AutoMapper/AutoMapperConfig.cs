using AutoMapper;
using StudentFollowingSystem.Models;
using StudentFollowingSystem.ViewModels;

namespace StudentFollowingSystem.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void Init()
        {
            Mapper.Initialize(c => c.AddProfile<ViewModelProfile>());
        }
    }

    public class ViewModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<StudentModel, Student>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<Student, StudentModel>();

            CreateMap<Class, ClassModel>()
                .ForMember(dest => dest.CounselerList, opt => opt.Ignore());

            CreateMap<ClassModel, Class>()
                .ForMember(dest => dest.Counseler, opt => opt.Ignore());

            CreateMap<CounselerModel, Counseler>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Counseler, CounselerModel>();

            CreateMap<Subject, SubjectModel>();
            CreateMap<SubjectModel, Subject>();

            CreateMap<Counseling, CounselingModel>();
            CreateMap<CounselingModel, Counseling>();
        }
    }
}
