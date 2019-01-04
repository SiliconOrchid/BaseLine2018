using Microsoft.EntityFrameworkCore;

using BaseLine2018.Common.Models.Entity;

namespace BaseLine2018.Data.EntityConfiguration
{
    public static class BaseEntityConfiguration
    {
        public static ModelBuilder SetUp<TEntity>(ModelBuilder modelBuilder, string tableName, string sqlNamespace) where TEntity : EntityBase
        {
            modelBuilder.Entity<TEntity>(b =>
            {
                b.ToTable(tableName, sqlNamespace);

                b.HasKey(c => c.Id);

                b.Property(c => c.Id)
                   .IsRequired();

                b.Property(c => c.Created)
               .IsRequired();

                b.Property(c => c.Modified)
                .IsRequired();
            });

            return modelBuilder;
        }
    }
}
