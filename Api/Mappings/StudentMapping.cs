using Api.DTOs;
using Logic.Application.Commands.Disenroll;
using Logic.Application.Commands.EditPersonalInfo;
using Logic.Application.Commands.Enroll;
using Logic.Application.Commands.Transfer;
using Mapster;

namespace Api.Mappings;

public class StudentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<
                (int studentId, StudentForEditPersonalInfoDto dto),
                EditPersonalInfoCommand
            >()
            .Map(dest => dest.StudentId, src => src.studentId)
            .Map(dest => dest.Name, src => src.dto.Name)
            .Map(dest => dest.Email, src => src.dto.Email);

        config
            .NewConfig<(int studentId, StudentForEnrollmentDto dto), EnrollCommand>()
            .Map(dest => dest.StudentId, src => src.studentId)
            .Map(dest => dest.Course, src => src.dto.Course)
            .Map(dest => dest.Grade, src => src.dto.Grade);

        config
            .NewConfig<
                (int studentId, int enrollmentNumber, StudentForTransferDto dto),
                TransferCommand
            >()
            .Map(dest => dest.StudentId, src => src.studentId)
            .Map(dest => dest.EnrollmentNumber, src => src.enrollmentNumber)
            .Map(dest => dest.Course, src => src.dto.Course)
            .Map(dest => dest.Grade, src => src.dto.Grade);

        config
            .NewConfig<
                (int studentId, int enrollmentNumber, StudentForDisenrollmentDto dto),
                DisenrollCommand
            >()
            .Map(dest => dest.StudentId, src => src.studentId)
            .Map(dest => dest.EnrollmentNumber, src => src.enrollmentNumber)
            .Map(dest => dest.Comment, src => src.dto.Comment);
    }
}
