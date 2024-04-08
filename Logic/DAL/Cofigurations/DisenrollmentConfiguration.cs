using Logic.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logic.DAL.Cofigurations;

public class DisenrollmentConfiguration : IEntityTypeConfiguration<Disenrollment>
{
    public void Configure(EntityTypeBuilder<Disenrollment> builder)
    {
        builder.ToTable("disenrollments").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.DateTime).HasColumnName("date_time");
        builder.Property(d => d.Comment).HasColumnName("comment");
        
        builder.HasOne(d => d.Course).WithMany().HasForeignKey("course_id");
    }
}
