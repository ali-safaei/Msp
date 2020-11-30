using Common;
using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasMaxLength(Lenghts.ProductName);
            builder.Property(c => c.ShortDescription).IsRequired().HasMaxLength(Lenghts.ProductShortDescription);
            builder.Property(c => c.Description).IsRequired();
            builder.OwnsOne(c => c.Price);
            builder.OwnsOne(c => c.OldPrice);

        }
    }
}
