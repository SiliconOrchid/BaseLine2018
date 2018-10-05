
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using BaseLine2018.Common.Models.Entity;
using BaseLine2018.Data.Context;

namespace BaseLine2018.Data.Helper
{
    public class DatabaseSeeder
    {

        private readonly ApplicationDbContext _context;
        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SeedSampleEntitiesFromJson(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"Value of {filePath} must be supplied to {nameof(SeedSampleEntitiesFromJson)}");
            }

            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"The file '{filePath}' does not exist");
            }

            var dataSet = File.ReadAllText(filePath);
            var seedData = JsonConvert.DeserializeObject<List<SampleEntity>>(dataSet, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            // When seeding objects with links to a reference table, we need to ensure we can pass in just the id of the referenced table (in this instance: ExternalSystemId)
            // Creating an object of the referenced entity will unsurprisingly create a new record in the database, breaking our intended references.

            _context.SampleEntitys.AddRange(seedData);

            return await _context.SaveChangesAsync();
        }

    }
}
