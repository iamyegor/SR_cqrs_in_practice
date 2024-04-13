using DbSynchronization.Synchronizers.Students.Models;
using Mapster;

namespace DbSynchronization.Mappings;

public class StudentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<StudentInCommandDb, StudentInQueryDb>()
            .Map(dest => dest.NumberOfEnrollments, src => src.Enrollments.Count)
            .Map(dest => dest.StudentId, src => src.Id)
            .AfterMapping(
                (src, dest) =>
                {
                    dest.FirstCourseName = null;
                    dest.FirstCourseGrade = null;
                    dest.FirstCourseCredits = null;

                    dest.SecondCourseName = null;
                    dest.SecondCourseGrade = null;
                    dest.SecondCourseCredits = null;

                    if (src.Enrollments.Count > 0)
                    {
                        dest.FirstCourseName = src.Enrollments[0].Name;
                        dest.FirstCourseGrade = src.Enrollments[0].Grade;
                        dest.FirstCourseCredits = src.Enrollments[0].Credits;
                    }

                    if (src.Enrollments.Count > 1)
                    {
                        dest.SecondCourseName = src.Enrollments[1].Name;
                        dest.SecondCourseGrade = src.Enrollments[1].Grade;
                        dest.SecondCourseCredits = src.Enrollments[1].Credits;
                    }

                    if (src.Enrollments.Count > 2)
                    {
                        throw new Exception("Number of enrollments exceeds the intended number");
                    }
                }
            );
    }
}
