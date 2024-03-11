using AutoMapper;
using DTOs;
using Logic.Students;
using Logic.Students.Commands.EditPersonalInfo;
using Logic.Students.Commands.Enroll;
using Logic.Students.Commands.Register;
using Logic.Students.Commands.Transfer;

namespace Api.Profiles;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentForEditPersonalInfoDto>().ReverseMap();

        CreateMap<StudentForRegistrationDto, RegisterCommand>();
        
        CreateMap<StudentForEnrollmentDto, EnrollCommand>();
        CreateMap<StudentForTransferDto, TransferCommand>();
        CreateMap<StudentForEditPersonalInfoDto, EditPersonalInfoCommand>();
    }
}
