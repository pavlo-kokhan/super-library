using Library.Domain.AggregationRoots.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.EntityConfigurations;

public class LibraryEntityConfiguration : IEntityTypeConfiguration<LibraryEntity>
{
    public void Configure(EntityTypeBuilder<LibraryEntity> builder)
    {
        builder.ToTable("Libraries");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.IsDeleted);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Address).HasColumnType("jsonb");
        builder.Property(x => x.WeekSchedule).HasColumnType("jsonb");
    }
}