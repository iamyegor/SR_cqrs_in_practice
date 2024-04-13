namespace DbSynchronization.Synchronizers.Students.Models;

public class EnrollmentInCommandDb
{
    public string Name { get; set; }
    public int Grade { get; set; }
    public int Credits { get; set; }
    public bool IsDeleted { get; set; }
}
