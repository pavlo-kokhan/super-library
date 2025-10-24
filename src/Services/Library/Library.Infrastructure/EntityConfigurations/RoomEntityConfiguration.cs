using Library.Domain.AggregationRoots.Library;
using Library.Domain.AggregationRoots.Room;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.EntityConfigurations;

public class RoomEntityConfiguration : IEntityTypeConfiguration<RoomEntity>
{
    public void Configure(EntityTypeBuilder<RoomEntity> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.IsDeleted);

        builder
            .HasOne<LibraryEntity>()
            .WithMany()
            .HasForeignKey(x => x.LibraryId);
    }
}