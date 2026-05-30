using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Persistence.Mappings;

file sealed class CardConfiguration : BaseEntityTypeConfiguration<Card>
{
    protected override string GetTableName() 
        => "cards";

    protected override void ConfigureEntity(EntityTypeBuilder<Card> builder)
    {
        builder
            .Property(x => x.ColumnId)
            .HasColumnName("column_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder
            .Property(x => x.AssigneeId)
            .HasColumnName("assignee_id")
            .HasColumnType("uuid");

        builder
            .Property(x => x.Title)
            .HasColumnName("title")
            .HasColumnType("varchar")
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("text");

        builder
            .Property(x => x.Order)
            .HasColumnName("sort_order")
            .HasColumnType("integer")
            .IsRequired();

        builder
            .Property(x => x.DueDate)
            .HasColumnName("due_date")
            .HasColumnType("date");

        builder
            .Property(x => x.Priority)
            .HasColumnName("priority")
            .HasColumnType("text")
            .HasConversion<string>()
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamptz")
            .IsRequired();

        builder
            .HasMany(x => x.Comments)
            .WithOne(x => x.Card)
            .HasForeignKey(x => x.CardId)
            .HasConstraintName("fk_comments_cards")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.ColumnId)
            .HasDatabaseName("ix_cards_column_id");

        builder
            .HasIndex(x => x.AssigneeId)
            .HasDatabaseName("ix_cards_assignee_id");

        builder
            .HasIndex(x => new { x.ColumnId, x.Order })
            .IsUnique()
            .HasDatabaseName("uq_cards_column_id_sort_order");

        builder
            .HasIndex(x => x.DueDate)
            .HasDatabaseName("ix_cards_due_date");
    }
}
