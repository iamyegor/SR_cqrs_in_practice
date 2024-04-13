using Logic.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.DAL.Cofigurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students").HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(s => s.Email).HasColumnName("email").IsRequired().HasMaxLength(100);

        builder
            .Property(s => s.IsSyncNeeded)
            .HasColumnName("is_sync_needed")
            .HasDefaultValue(true);

        builder
            .HasMany(s => s.Enrollments)
            .WithOne(e => e.Student)
            .HasForeignKey("student_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.Disenrollments)
            .WithOne(d => d.Student)
            .HasForeignKey("student_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
