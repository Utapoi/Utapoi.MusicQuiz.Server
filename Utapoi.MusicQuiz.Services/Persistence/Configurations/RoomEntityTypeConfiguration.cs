using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Infrastructure.Persistence.Configurations;

public sealed class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        // A User can only be in one room at a time.
        // So we should have a one-to-many relationship.
        builder.HasMany(x => x.Users)
            .WithOne(x => x.CurrentRoom)
            .HasForeignKey(x => x.CurrentRoomId);
    }
}