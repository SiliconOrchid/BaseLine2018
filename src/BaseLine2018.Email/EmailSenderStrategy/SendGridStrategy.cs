using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

using SendGrid;
using SendGrid.Helpers.Mail;

using BaseLine2018.Common.Enums;
using BaseLine2018.Common.Extensions;
using BaseLine2018.Common.Logging;
using BaseLine2018.Common.Models.Configuration;
using BaseLine2018.Common.Models.Domain;
using BaseLine2018.Email.Interface;
using BaseLine2018.Email.Model;

namespace BaseLine2018.Email.EmailSenderStrategy
{
    public class SendGridStrategy : IEmailSenderStrategy
    {
        private readonly EmailSettingsConfig _emailSettingsConfig;
        private readonly IHostingEnvironment _hostingEnvironment;


        public SendGridStrategy(IOptions<EmailSettingsConfig> emailSettingsConfig, IHostingEnvironment hostingEnvironment)
        {
            _emailSettingsConfig = emailSettingsConfig.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<ServiceResponse<string>> SendEmail(EmailTemplate emailTemplate)
        {
            if (string.IsNullOrWhiteSpace(_emailSettingsConfig.SendGridApiKey))
                throw new ArgumentException(
                    $"{this.GetCallingClassAndMethod()} SendGrid ApiKey must be specified in app settings");

            if (string.IsNullOrWhiteSpace(_emailSettingsConfig.FromAddress))
                throw new ArgumentException(
                    $"{this.GetCallingClassAndMethod()} FromAddress must be specified in app settings");

            if (string.IsNullOrWhiteSpace(emailTemplate.ListRecipients.ToString()))
                throw new ArgumentException($"{this.GetCallingClassAndMethod()} No recipients identified ");

            if (string.IsNullOrWhiteSpace(emailTemplate.EmailSubject))
                throw new ArgumentException($"{this.GetCallingClassAndMethod()} Email Subject Line must be specified ");

            if (string.IsNullOrWhiteSpace(emailTemplate.EmailHtmlBody))
                throw new ArgumentException($"{this.GetCallingClassAndMethod()} Email Body must be specified ");



            var client = new SendGridClient(_emailSettingsConfig.SendGridApiKey);
            var sendGridMessage = new SendGridMessage();

            SetEmailFields(emailTemplate, sendGridMessage);
            SetEmailFromField(emailTemplate, sendGridMessage);

            Response result;

            try
            {
                result = await client.SendEmailAsync(sendGridMessage);
            }
            catch (Exception ex)
            {
                // something has blown up properly on our server - e.g. bad SendGrid credentials
                string errorMessage = " Problem attempting to send email to SendGrid - this is not an error response from the Sendgrid API ";
                Log.Error($"{this.GetCallingClassAndMethod()} {errorMessage} : {ex.Message}");

                return new ServiceResponse<string>
                {
                    ServiceResponseStatus = ServiceResponseStatusEnum.Fail_Unhandled,
                    Message = "Problem sending email", //TODO:  This message is likely to be fed back to the UI/client ... this would be a good opportunity to replace this hardcoded message with something like a resource-file lookup
                    Result = null
                };
            }

            if (!(result.StatusCode == HttpStatusCode.Accepted || result.StatusCode == HttpStatusCode.OK))
            {
                // still an error state, but this time the error is being reported by the SendGrid server
                Log.Error($"{this.GetCallingClassAndMethod()} Sendgrid server did not response with an OK :  error Code Received was '{result.StatusCode.ToString()}' ");

                return new ServiceResponse<string>
                {
                    ServiceResponseStatus = ServiceResponseStatusEnum.Fail_Handled,
                    Message = "Problem sending email", //TODO:  This message is likely to be fed back to the UI/client ... this would be a good opportunity to replace this hardcoded message with something like a resource-file lookup
                    Result = string.Empty
                };
            }

            // otherwise report that email(s) were sent ok...
            return new ServiceResponse<string>
            {
                ServiceResponseStatus = ServiceResponseStatusEnum.Ok,
                Message = String.Empty,
                Result = "Emails sent OK"
            };

        }

        private void SetEmailFields(EmailTemplate emailTemplate, SendGridMessage sendGridMessage)
        {
            if (emailTemplate.ListRecipients == null)
            {
                Log.Warn($"{this.GetCallingClassAndMethod()} List of email recipients was null - this suggests that something is not working as it should.");
            }
            else
            {
                foreach (var recipient in emailTemplate.ListRecipients)
                {
                    sendGridMessage.AddTo(recipient.Trim());
                }
            }

            if (emailTemplate.ListCopyRecipients != null)
            {
                foreach (var copyRecipient in emailTemplate.ListCopyRecipients)
                {
                    sendGridMessage.AddCc(copyRecipient.Trim());
                }
            }

            if (emailTemplate.ListBlindCopyRecipients != null)
            {
                foreach (var blindCopyRecipient in emailTemplate.ListBlindCopyRecipients)
                {
                    sendGridMessage.AddCc(blindCopyRecipient.Trim());
                }
            }

            sendGridMessage.Subject = emailTemplate.EmailSubject.Trim();
            sendGridMessage.HtmlContent = emailTemplate.EmailHtmlBody;
        }

        private void SetEmailFromField(EmailTemplate emailTemplate, SendGridMessage sendGridMessage)
        {
// ----------- define "from" address, using SendGrid specific Types -----------
            EmailAddress fromAddress;
            if (string.IsNullOrWhiteSpace(emailTemplate.SentFromOveride))
            {
                // use a generic "from" address which is identified in appsettings, rather than a value specifically added to the emailTemplate model
                fromAddress = new EmailAddress(_emailSettingsConfig.FromAddress);
            }
            else
            {
                // use "from" address identified as override-value in model:   this gives us the option of being more specific (rather than a generic "noreply@website.com" etc)
                fromAddress = new EmailAddress(emailTemplate.SentFromOveride);
            }
            sendGridMessage.From = fromAddress;
        }
    }
}
