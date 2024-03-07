using Logic.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.DAL.Cofigurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Email).IsRequired().HasMaxLength(100);

        builder.HasMany(s => s.Enrollments).WithOne(e => e.Student).HasForeignKey("StudentId");
        builder.HasMany(s => s.Disenrollments).WithOne(d => d.Student).HasForeignKey("StudentId");

        builder.Ignore(s => s.FirstEnrollment);
        builder.Ignore(s => s.SecondEnrollment);
    }
}
