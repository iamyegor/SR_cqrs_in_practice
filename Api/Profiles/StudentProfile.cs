using AutoMapper;
using DTOs;
using Logic.Services.Commands.EditPersonalInfo;
using Logic.Services.Commands.Enroll;
using Logic.Services.Commands.Register;
using Logic.Services.Commands.Transfer;
using Logic.Students;

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
