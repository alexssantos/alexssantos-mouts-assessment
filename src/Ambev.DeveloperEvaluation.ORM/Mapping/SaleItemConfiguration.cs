using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.HasKey(si => si.Id);

            builder.Property(si => si.ProductId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(si => si.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(si => si.Quantity)
                .IsRequired();

            builder.Property(si => si.UnitPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(si => si.Discount)
                .IsRequired()
                .HasPrecision(18, 4);

            builder.Property(si => si.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(si => si.IsCancelled)
                .IsRequired();

            builder.Property(si => si.CreatedAt)
                .IsRequired();

            builder.Property(si => si.UpdatedAt);
        }
    }
}