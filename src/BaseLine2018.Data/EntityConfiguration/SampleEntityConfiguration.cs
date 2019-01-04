using Microsoft.EntityFrameworkCore;

using BaseLine2018.Common.Models.Entity;

namespace BaseLine2018.Data.EntityConfiguration
{
    public class SampleEntityConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            // HELP : EF Core reference for fluentAPI:  http://ef.readthedocs.io/en/latest/modeling/keys.html
            // HELP : FluentAPI reference:  http://ef.readthedocs.io/en/latest/modeling/relationships.html

            // TIP : ENSURE THAT this configuration is registered in "Context/IMCIntegrationDBContext/OnModelCreating"
            // TIP : EF Configuration is partly done in the "BaseEntity" (which is purpose of 'BaseEntityConfiguration.SetUp ...' below) - be aware that fields like the primary key and other common fields are configured there and not in this file.

            modelBuilder.Entity<SampleEntity>(b =>
            {
                modelBuilder = BaseEntityConfiguration.SetUp<SampleEntity>(modelBuilder, "Sample", "WebApp");


                #region ---- Entity Main Properties  ----   
                b.Property(c => c.SampleLookupField)
                    .IsRequired();

                b.Property(c => c.SampleTextField)
                    .IsRequired()
                    .HasMaxLength(1024);

                #endregion

                #region ---- Entity Indicies  ----     

                b.HasIndex(c => c.SampleLookupField);

                #endregion
            });

        }
    }
}
