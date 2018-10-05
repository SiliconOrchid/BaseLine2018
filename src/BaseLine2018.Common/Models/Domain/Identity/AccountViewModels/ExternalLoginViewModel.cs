using System.ComponentModel.DataAnnotations;

namespace BaseLine2018.Common.Models.Domain.Identity.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
