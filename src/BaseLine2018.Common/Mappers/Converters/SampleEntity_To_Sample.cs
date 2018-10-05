using System;

using AutoMapper;

using BaseLine2018.Common.Logging;
using Entity = BaseLine2018.Common.Models.Entity;
using Domain = BaseLine2018.Common.Models.Domain;

namespace BaseLine2018.Common.Mappers.Converters
{
    /// <summary>
    /// Sample Automapper custom mapper.
    /// Please remove this sample when it is no longer of use.
    /// This class demonstrates how you can use automapper to map fields very specifically
    /// </summary>
    public class SampleEntity_To_Sample : ITypeConverter<Entity.SampleEntity, Domain.Sample>
    {

        public Domain.Sample Convert(Entity.SampleEntity source, Domain.Sample destination, ResolutionContext context)
        {

            if (source == null)
            {
                Log.Error($"[SampleEntity_To_Sample.Convert] Value '{nameof(source)}' was null");
                throw new ArgumentNullException(nameof(source), $"converter \"{nameof(source)}\" param was null");
            }

            try
            {
                destination = new Domain.Sample
                {
                    SampleLookupField = source.SampleLookupField,
                    SampleTextField = source.SampleTextField
                };

                return destination;

            }
            catch (Exception ex)
            {
                Log.Error($"[SampleEntity_To_Sample.Convert] Exception {ex}");
                throw;
            }
        }
    }
}
