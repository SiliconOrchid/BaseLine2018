using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using BaseLine2018.Common.Logging;
using BaseLine2018.Common.Models.Entity;
using BaseLine2018.Data.Context;


namespace BaseLine2018.Data.Extension
{
    public static class ApplicationContextExtension
    {
        public static async Task<int> EnsureSeedData(this ApplicationDbContext context)
        {
            var sampleEntityCount = default(int);

            var xyzExampleOtherEntityCount =
                default(int); //this variable is a placeholder for your own additonal entities....


            // Because each of the following seed method needs to do a save
            // (the data they're importing is relational), we need to call
            // SaveAsync within each method.
            // So let's keep tabs on the counts as they come back
            await SeedSampleData(context);
            //TODO :  call additional methods to seed data for other entities...

            return sampleEntityCount + xyzExampleOtherEntityCount;
        }

        private static async Task<int> SeedSampleData(ApplicationDbContext context)
        {
            try
            {
                if (!context.SampleEntitys.Any())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        // JM16Jul18 : Note : Wrap these database activities together as a transaction to ENSURE that identity-insert state is restored

                        //TODO:  JM16Jul18 - Looking for a way to pick the sql namespace.tablename from the fluentAPI configuration, as the following manual sql command introduces a fragile naming relationship.
                        context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [WebApp].[Sample] ON");

                        context.SampleEntitys.Add(new SampleEntity
                        {
                            Id = 1,
                            SampleLookupField = 1,
                            SampleTextField = "Sample Entity Text Field Example #1",
                            Created = new DateTime(2018, 7, 1),
                            Modified = new DateTime(2018, 7, 1)
                        });
                        context.SampleEntitys.Add(new SampleEntity
                        {
                            Id = 2,
                            SampleLookupField = 1,
                            SampleTextField = "Sample Entity Text Field Example #2",
                            Created = new DateTime(2018, 7, 1),
                            Modified = new DateTime(2018, 7, 1)
                        });
                        context.SampleEntitys.Add(new SampleEntity
                        {
                            Id = 3,
                            SampleLookupField = 1,
                            SampleTextField = "Sample Entity Text Field Example #3",
                            Created = new DateTime(2018, 7, 1),
                            Modified = new DateTime(2018, 7, 1)
                        });
                        context.SampleEntitys.Add(new SampleEntity
                        {
                            Id = 4,
                            SampleLookupField = 1,
                            SampleTextField = "Sample Entity Text Field Example #4",
                            Created = new DateTime(2018, 7, 1),
                            Modified = new DateTime(2018, 7, 1)
                        });
                        context.SaveChanges();

                        context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [WebApp].[Sample]  OFF");

                        transaction.Commit();
                    }

                    return await context.SampleEntitys.CountAsync();
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error($"[ApplicationContextExtension.SeedSampleData] : Unexpected exception : ", ex);
                throw;
            }
        }


        //TODO:  Implement seeding of Identity Roles 

        //private static async Task<int> SeedIdentityProviderRoles(ApplicationDbContext context)
        //{
        //    try
        //    {

        //        using (var transaction = context.Database.BeginTransaction())
        //        {

        //            //var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        //            //foreach (var role in Roles)
        //            //{
        //            //    if (!await roleManager.RoleExistsAsync(role))
        //            //    {
        //            //        await roleManager.CreateAsync(new ApplicationRole(role));
        //            //    }
        //            //}

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"[ApplicationContextExtension.SeedIdentityProviderRoles] : Unexpected exception : ", ex);
        //        throw;
        //    }
        //}

        //TODO:  Additonal private methods, very similar to "SeedSampleData" that seed other entities we end up creating in a real application ...
    }
}