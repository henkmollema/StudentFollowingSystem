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
            CreateMap<StudentModel, Student>();
            CreateMap<Student, StudentModel>();
        }
    }
}
