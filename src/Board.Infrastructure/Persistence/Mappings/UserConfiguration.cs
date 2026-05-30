using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Persistence.Mappings;

file sealed class UserConfiguration : BaseEntityTypeConfiguration<User>
{
    protected override string GetTableName()
        => "users";

    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(x => x.Name)
            .HasColumnName("name")
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Email)
            .HasColumnName("email")
            .HasColumnType("varchar")
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .HasColumnType("varchar")
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder
            .Property(x => x.LastAccessAt)
            .HasColumnName("last_access_at")
            .HasColumnType("timestamptz");

        builder
            .HasMany(x => x.Projects)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId)
            .HasConstraintName("fk_projects_users")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.AssignedCards)
            .WithOne(x => x.Assignee)
            .HasForeignKey(x => x.AssigneeId)
            .HasConstraintName("fk_cards_users_assignee")
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasMany(x => x.Comments)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId)
            .HasConstraintName("fk_comments_users")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("uq_users_email");
    }
}