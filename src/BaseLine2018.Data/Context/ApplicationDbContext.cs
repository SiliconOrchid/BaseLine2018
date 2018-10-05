using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using BaseLine2018.Common.Models.Entity;
using BaseLine2018.Data.EntityConfiguration;
using BaseLine2018.Data.Extension;
using BaseLine2018.Common.Models.Domain.Identity;




namespace BaseLine2018.Data.Context
{

    //Note this context inherits from "IdentityDbContext", not just "DbContext" !

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<SampleEntity> SampleEntitys { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Setting a longer Timeout better accomodates a heavily loaded db server.   For example, allows more time for pontentially expensive operations to run (such as changing indexes, applying EF migrations)
            Database.SetCommandTimeout(600);



            //Database.EnsureCreated();
        }

        /// <summary>
        /// Used as part of FluentAPI to allow us to annotate POCO models with database specific attributes
        /// Each entity has its own configuration - we register each of those items in this class.
        /// Registrations can be found in the "EntityConfiguration" namespace of this assembly.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SampleEntityConfiguration.Configure(modelBuilder);
        }


        /// <summary>
        /// Allows us to globally run code, in this case updating the "Created" and "Modified" fields, without having to specify in each repo method.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.ApplyAuditInformation();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
