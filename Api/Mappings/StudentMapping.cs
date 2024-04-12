using Api.DTOs;
using Logic.Application.Commands.Disenroll;
using Logic.Application.Commands.EditPersonalInfo;
using Logic.Application.Commands.Enroll;
using Logic.Application.Commands.Transfer;
using Logic.Application.Queries.GetStudentsList;
using Logic.Students;
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

        config
            .NewConfig<StudentInDb, StudentDto>()
            .Map(dest => dest.Id, src => src.StudentId)
            .Map(dest => dest.Course1, src => src.FirstCourseName)
            .Map(dest => dest.Course1Grade, src => src.FirstCourseGrade)
            .Map(dest => dest.Course1Credits, src => src.FirstCourseCredits)
            .Map(dest => dest.Course2, src => src.SecondCourseName)
            .Map(dest => dest.Course2Grade, src => src.SecondCourseGrade)
            .Map(dest => dest.Course2Credits, src => src.SecondCourseCredits)
            .AfterMapping(
                (src, dest) =>
                {
                    Enum.TryParse(src.FirstCourseGrade, out Grade course1Grade);
                    dest.Course1Grade = course1Grade.ToString();

                    Enum.TryParse(src.SecondCourseGrade, out Grade course2Grade);
                    dest.Course2Grade = course2Grade.ToString();
                }
            );
    }
}
