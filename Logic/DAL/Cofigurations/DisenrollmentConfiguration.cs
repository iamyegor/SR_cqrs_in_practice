using Logic.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.DAL.Cofigurations;

public class DisenrollmentConfiguration : IEntityTypeConfiguration<Disenrollment>
{
    public void Configure(EntityTypeBuilder<Disenrollment> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasOne(d => d.Course).WithMany().HasForeignKey("CourseId");

        builder.Property(d => d.DateTime);
        builder.Property(d => d.Comment);
    }
}
