using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Persistence.Mappings;

file sealed class CommentConfiguration : BaseEntityTypeConfiguration<Comment>
{
    protected override string GetTableName()
        => "comments";

    protected override void ConfigureEntity(EntityTypeBuilder<Comment> builder)
    {
        builder
            .Property(x => x.CardId)
            .HasColumnName("card_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder
            .Property(x => x.AuthorId)
            .HasColumnName("author_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder
            .Property(x => x.Content)
            .HasColumnName("content")
            .HasColumnType("text")
            .IsRequired();

        builder
            .Property(x => x.EditedAt)
            .HasColumnName("edited_at")
            .HasColumnType("timestamptz");

        builder
            .HasIndex(x => x.CardId)
            .HasDatabaseName("ix_comments_card_id");

        builder
            .HasIndex(x => x.AuthorId)
            .HasDatabaseName("ix_comments_author_id");
    }
}