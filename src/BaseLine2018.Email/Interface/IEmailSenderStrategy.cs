using System.Threading.Tasks;

using BaseLine2018.Common.Models.Domain;
using BaseLine2018.Email.Model;

namespace BaseLine2018.Email.Interface
{
    /// <summary>
    /// An implementation-agnostic interface.   This would be implenented by different strategies (e.g. a specific SendGrid strategy)
    /// </summary>
    public interface IEmailSenderStrategy
    {
        Task<ServiceResponse<string>> SendEmail(EmailTemplate emailTemplate);
    }
}
