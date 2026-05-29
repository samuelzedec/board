using Board.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Persistence.Mappings;

internal abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
    where T : Entity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(GetTableName());

        builder.HasKey(x => x.Id)
            .HasName($"pk_{GetTableName()}_id");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamptz")
            .HasColumnName("created_at")
            .IsRequired();

        ConfigureEntity(builder);
    }

    protected abstract string GetTableName();
    protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
}