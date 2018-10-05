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


            modelBuilder.Entity<SampleEntity>(b =>
            {
                b.ToTable("Sample", "WebApp");  // define the sql namespace and tablename (which may be different to the poco classname)
              

                #region ---- Entity Relationships  ----               

                b.HasKey(c => c.Id);

                #endregion

                #region ---- Entity Baseclass properties   ----    

                b.Property(c => c.Created)
                    .IsRequired();

                b.Property(c => c.Modified)
                    .IsRequired();

                #endregion

                #region ---- Entity Main Properties  ----   
                b.Property(c => c.Id)
                    .IsRequired();

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
