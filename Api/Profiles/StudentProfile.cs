using Api.DTOs;
using AutoMapper;
using Logic.Students;
using Logic.Students.Commands.Register;

namespace Api.Profiles;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>()
            .ForMember(
                dest => dest.Course1,
                opt =>
                    opt.MapFrom(src =>
                        src.FirstEnrollment != null ? src.FirstEnrollment.Course.Name : default
                    )
            )
            .ForMember(
                dest => dest.Course1Grade,
                opt =>
                    opt.MapFrom(src =>
                        src.FirstEnrollment != null ? src.FirstEnrollment.Grade.ToString() : default
                    )
            )
            .ForMember(
                dest => dest.Course1Credits,
                opt =>
                    opt.MapFrom(src =>
                        src.FirstEnrollment != null ? src.FirstEnrollment.Course.Credits : default
                    )
            )
            .ForMember(
                dest => dest.Course2,
                opt =>
                    opt.MapFrom(src =>
                        src.SecondEnrollment != null ? src.SecondEnrollment.Course.Name : default
                    )
            )
            .ForMember(
                dest => dest.Course2Grade,
                opt =>
                    opt.MapFrom(src =>
                        src.SecondEnrollment != null
                            ? src.SecondEnrollment.Grade.ToString()
                            : default
                    )
            )
            .ForMember(
                dest => dest.Course2Credits,
                opt =>
                    opt.MapFrom(src =>
                        src.SecondEnrollment != null ? src.SecondEnrollment.Course.Credits : default
                    )
            );

        CreateMap<Student, StudentForEditPersonalInfoDto>().ReverseMap();

        CreateMap<StudentForRegistrationDto, RegisterCommand>();
    }
}
