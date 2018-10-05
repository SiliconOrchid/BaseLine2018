using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using BaseLine2018.Common.Enums;
using BaseLine2018.Common.Extensions;
using BaseLine2018.Common.Logging;
using BaseLine2018.Common.Models.Domain;
using BaseLine2018.Common.Models.Entity;
using BaseLine2018.Data.Repository.Interfaces;
using BaseLine2018.Service.Interface.Sample1Services;


namespace BaseLine2018.Service.Sample1Services
{

    /// <summary>
    /// Example Strategy, within a sample namespace.
    /// This class (and folder) can be safely removed.
    /// </summary>
    public class SampleService : ISampleService
    {
        private readonly IMapper _mapper;
        private readonly ISampleEntityRepository _sampleEntityRepository;


        public SampleService(
            IMapper mapper, 
            ISampleEntityRepository sampleEntityRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException($"{this.GetCallingClassAndMethod()} mapper was null");
            _sampleEntityRepository = sampleEntityRepository ?? throw new ArgumentNullException($"{this.GetCallingClassAndMethod()} sampleEntityRepository was null");
        }


        public async Task<ServiceResponse<Sample>> GetAsync(int id)
        {
            var serviceResponse = new ServiceResponse<Sample>();

            if (id < 1)
            {
                string errorMessage = "Unexpected id";
                Log.Error($"{this.GetCallingClassAndMethod()} {errorMessage}");
                serviceResponse.ServiceResponseStatus = ServiceResponseStatusEnum.Fail_Handled;
                serviceResponse.Message = errorMessage;
                return serviceResponse;
            }


            SampleEntity sampleEntity;

            // ------- retrieve -------
            try
            {
                sampleEntity = await _sampleEntityRepository.GetAsync(id);
            }
            catch (Exception ex)
            {
                string errorMessage = "Problem retrieving data from repository";

                Log.Error($"{this.GetCallingClassAndMethod()}{errorMessage}", ex);

                serviceResponse.ServiceResponseStatus = ServiceResponseStatusEnum.Fail_Handled;
                serviceResponse.Message = errorMessage;

                return serviceResponse; // noting that error is not rethrown
            }

            // ------- map from entity to domain object -------
            Sample sample;

            try
            {
                sample = _mapper.Map<Sample>(sampleEntity);
            }
            catch (Exception ex)
            {
                string errorMessage = "Problem when attempting to map";

                Log.Error($"{this.GetCallingClassAndMethod()} {errorMessage}", ex);

                serviceResponse.ServiceResponseStatus = ServiceResponseStatusEnum.Fail_Handled;
                serviceResponse.Message = errorMessage;

                return serviceResponse; // noting that error is not rethrown
            }

            // ------- annotate and return response object -------
            if (sample is null)
            {
                serviceResponse.ServiceResponseStatus = ServiceResponseStatusEnum.Ok_NoData;
            }
            else
            {
                serviceResponse.ServiceResponseStatus = ServiceResponseStatusEnum.Ok;
            }
            serviceResponse.Result = sample;

            return serviceResponse;
        }
        




        public async Task<ServiceResponse<List<Sample>>> GetAllAsync()
        {
            var serviceResponse = new ServiceResponse<List<Sample>>();

            List<SampleEntity> listSampleEntity;

            // ------- retrieve -------
            try
            {
                IEnumerable<SampleEntity> result = await _sampleEntityRepository.GetAllOrderedAsync();
                listSampleEntity = result.ToList();
            }
            catch (Exception ex)
            {
                string errorMessage = "Problem retrieving data from repository";
                Log.Error($"{this.GetCallingClassAndMethod()} {errorMessage} : ", ex);

                serviceResponse.ServiceResponseStatus = ServiceResponseStatusEnum.Fail_Handled;
                serviceResponse.Message = errorMessage;

                return serviceResponse; // noting that error is not rethrown
            }

            // ------- map from entity to domain object -------
            List<Sample> listSamples;

            try
            {
                listSamples = _mapper.Map<List<Sample>>(listSampleEntity);
            }
            catch (Exception ex)
            {
                string errorMessage = "Problem when attempting to map objects";
                Log.Error($"{this.GetCallingClassAndMethod()} {errorMessage} : ", ex);

                serviceResponse.ServiceResponseStatus = ServiceResponseStatusEnum.Fail_Handled;
                serviceResponse.Message = errorMessage;

                return serviceResponse; // noting that error is not rethrown
            }

            // ------- annotate and return response object -------
            if (listSampleEntity.Any())
            {
                serviceResponse.ServiceResponseStatus = ServiceResponseStatusEnum.Ok;
            }
            else
            {
                serviceResponse.ServiceResponseStatus = ServiceResponseStatusEnum.Ok_NoData;
            }
            serviceResponse.Result = listSamples;

            return serviceResponse;
        }

    }
}
 