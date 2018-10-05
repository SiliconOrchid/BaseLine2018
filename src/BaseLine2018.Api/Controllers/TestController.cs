using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BaseLine2018.Common.Extensions;
using BaseLine2018.Common.Logging;
using BaseLine2018.Email.RazorEngine;

using BaseLine2018.Api.EmailTemplates;
using BaseLine2018.Common.Enums;
using BaseLine2018.Email.Interface;
using BaseLine2018.Email.Model;



namespace BaseLine2018.Api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IEmailSenderStrategy _emailSenderStrategy;

        public TestController(IEmailSenderStrategy emailSenderStrategy)
        {
            _emailSenderStrategy = emailSenderStrategy;
        }

        /// <summary>
        /// This is a demonstration of how to render an email template using the razorengine.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Email/GenerateFromTemplate")]
        public async Task<IActionResult> GenerateFromTemplate()
        {
            var templateModel = new SampleEmailTemplateModel();
            templateModel.EmailHeader = "Any old text";
            templateModel.ListCollectionItems.Add(new SampleEmailTemplateModelCollectionItem { CollectionItemDescription  = "CollectionItemOne"} );
            templateModel.ListCollectionItems.Add(new SampleEmailTemplateModelCollectionItem { CollectionItemDescription = "CollectionItemTwo" });

            // define some comfiguration items used by the dynamic assembly
            string dynamicAssemblyNamespace = "EmailTemplateDynamicAssembly";
            string dynamicDllName = "EmailTemplateDynamicAssemblyDllName";

            // dynamically compile a template assembly
            Assembly templateAssembly = RazorEngineDynamicCompilerHelper<SampleEmailTemplateModel>.CompileTemplate("SampleEmailTemplate.cshtml", dynamicAssemblyNamespace, dynamicDllName);

            var renderedHtml = RazorEngineTemplateHelper<SampleEmailTemplateModel>.MergeViewModelReturnMarkup(templateModel, templateAssembly, dynamicAssemblyNamespace);

            return Ok(renderedHtml);
        }


        /// <summary>
        /// This is a demonstration of how to send an email using sendgrid
        /// </summary>
        /// <returns></returns>
        [HttpGet("Email/SendEmail")]
        public async Task<IActionResult> SendEmail()
        {
            var templateModel = new EmailTemplate();
            templateModel.ListRecipients = new List<string>{"jim@wecreateclarity.co.uk"};
            templateModel.SentFromOveride = "jim@wecreateclarity.co.uk";
            templateModel.EmailSubject = "This is a test email";
            templateModel.EmailHtmlBody = "<html><body><p>This is a test email.</p></body></html>";

            var serviceResponse = await _emailSenderStrategy.SendEmail(templateModel);

            if (serviceResponse.ServiceResponseStatus == ServiceResponseStatusEnum.Ok)
            {
                return Ok(serviceResponse.Result);
            }
            else
            {
                return StatusCode(500, serviceResponse.Message);
            }
        }


        [HttpGet("Logging/ProvokeLoggedError")]
        public async Task<IActionResult> ProvokeLoggedError()
        {
            string errorMessage = "Manually provoked Error, for testing logging.";
            Log.Error($"{this.GetCallingClassAndMethod()}{errorMessage}");

            return Ok();
        }


    }
}
