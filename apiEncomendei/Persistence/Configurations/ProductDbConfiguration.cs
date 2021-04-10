using apiEncomendei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.Persistence.Configurations
{
    public class ProductDbConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
               .HasKey(c => c.Id);

            builder
                .Property(c => c.Brand)
                .HasColumnType("VARCHAR(100)")
                .HasDefaultValue("PADRÃO")
                .HasMaxLength(100);

            builder
                .Property(c => c.ProductionDate)
                .HasDefaultValueSql("getdate()");
        }
    }
}
