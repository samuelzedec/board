using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Persistence.Mappings;

file sealed class ColumnConfiguration : BaseEntityTypeConfiguration<Column>
{
    protected override string GetTableName()
        => "columns";

    protected override void ConfigureEntity(EntityTypeBuilder<Column> builder)
    {
        builder
            .Property(x => x.ProjectId)
            .HasColumnName("project_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder
            .Property(x => x.Name)
            .HasColumnName("name")
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Order)
            .HasColumnName("sort_order")
            .HasColumnType("integer")
            .IsRequired();

        builder
            .HasMany(x => x.Cards)
            .WithOne(x => x.Column)
            .HasForeignKey(x => x.ColumnId)
            .HasConstraintName("fk_cards_columns")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.ProjectId)
            .HasDatabaseName("ix_columns_project_id");

        builder
            .HasIndex(x => new { x.ProjectId, x.Order })
            .IsUnique()
            .HasDatabaseName("uq_columns_project_id_sort_order");
    }
}