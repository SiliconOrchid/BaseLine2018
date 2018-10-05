using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BaseLine2018.Common.Enums;
using BaseLine2018.Common.Extensions;
using BaseLine2018.Common.Logging;
using BaseLine2018.Common.Models.Domain;
using BaseLine2018.Service.Interface.Sample1Services;


namespace BaseLine2018.Api.Controllers
{
    /// <summary>
    /// This is a test controller intended to demonstrate how data might be retrieved  (ultimately from a datastore) via a data-strategy.
    /// </summary>
    [Route("api/[controller]")]
    public class TestDataRetrievalController : Controller
    {
        private readonly ISampleService _sampleService;


        public TestDataRetrievalController(ISampleService sampleService)
        {
            _sampleService = sampleService ?? throw new ArgumentNullException($"{this.GetCallingClassAndMethod()} sampleService was null");           
        }

        /// <summary>
        /// Get a single record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("Unexpected id");
            }

            try
            {
                var result = await _sampleService.GetAsync(id);
                return ReturnHttpResponse(result);
            }
            catch (Exception ex)
            {
                Log.Error($"{this.GetCallingClassAndMethod()} : ", ex);
                return StatusCode(500);
            }           
        }

        /// <summary>
        /// Get a collection of records
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _sampleService.GetAllAsync();
                return ReturnHttpResponse(result);
            }
            catch (Exception ex)
            {
                Log.Error($"{this.GetCallingClassAndMethod()} : ", ex);
                return StatusCode(500);
            }
        }






        private IActionResult ReturnHttpResponse<T>(ServiceResponse<T> result)
        {
            if (result.ServiceResponseStatus == ServiceResponseStatusEnum.Ok_NoData)
            {
                return NoContent();
            }

            if (result.ServiceResponseStatus == ServiceResponseStatusEnum.Ok ||
                result.ServiceResponseStatus == ServiceResponseStatusEnum.Fail_Handled)
            {
                return Ok(result);
            }

            // for all other error (or unset) LogicResponseStatusEnum, default to returning 'Internal Server Error'
            return StatusCode(500);
        }
    }

}
