using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using BaseLine2018.Common.Models.Entity;

namespace BaseLine2018.Data.Extension
{
    /// <summary>
    /// Extension to change tracker to ensure Created / Modified dates are added accordingly
    /// </summary>
    public static class ChangeTrackerExtention
    {
        public static void ApplyAuditInformation(this ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries())
            {
                if (!(entry.Entity is EntityBase entityBase)) continue;

                var now = DateTime.UtcNow;
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entityBase.Created = now;
                        entityBase.Modified = now;
                        break;

                    case EntityState.Added:
                        entityBase.Created = now;
                        break;
                }
            }
        }
    }
}
