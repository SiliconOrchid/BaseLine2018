using AutoMapper;
using BaseLine2018.Common.Mappers.Converters;

// tip:  use the following namespace alias to avoid lots of typing in the body of this class
using Entity = BaseLine2018.Common.Models.Entity;
using Domain = BaseLine2018.Common.Models.Domain;

namespace BaseLine2018.Common.Mappers
{
    public class CommonMapperConfiguration : Profile
    {

        public CommonMapperConfiguration()
        {
            /*-------------------------
            // this code is intended as examples of configuraiton for automapper - please feel free to remove this (and this comment) in a production project

            // using a custom-mapper...
            CreateMap<Entity.SampleEntity, Models.Domain.Sample>()
                .MaxDepth(1)
                .ConvertUsing(new SampleEntity_To_Sample());

            // using generic automapper...
            CreateMap<Entity.SampleEntity, Domain.Sample>().MaxDepth(1).ReverseMap();
            -------------------------*/
        }
    }
}
