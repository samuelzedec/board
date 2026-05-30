using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Persistence.Mappings;

file sealed class ProjectConfiguration : BaseEntityTypeConfiguration<Project>
{
    protected override string GetTableName()
        => "projects";

    protected override void ConfigureEntity(EntityTypeBuilder<Project> builder)
    {
        builder
            .Property(x => x.OwnerId)
            .HasColumnName("owner_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder
            .Property(x => x.Name)
            .HasColumnName("name")
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("text");

        builder
            .Property(x => x.Color)
            .HasColumnName("color")
            .HasColumnType("varchar")
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder
            .HasMany(x => x.Columns)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId)
            .HasConstraintName("fk_columns_projects")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.OwnerId)
            .HasDatabaseName("ix_projects_owner_id");
    }
}