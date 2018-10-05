using System.Collections.Generic;

namespace BaseLine2018.Email.Model
{
    /// <summary>
    /// Basic Email Model, which is agnostic of any specific email-sending implementation (e.g. not wiring up directly to SendGrid specific models, etc)
    /// </summary>
    public class EmailTemplate
    {
        public List<string> ListRecipients { get; set; }
        public List<string> ListCopyRecipients { get; set; }
        public List<string> ListBlindCopyRecipients { get; set; }

        public string SentFromOveride { get; set; } // populate this field with a value to specifically overide.    Leave blank to use the _default_ "from", which will be deined in the project appsetting configuration

        public string EmailSubject { get; set; }
        public string EmailHtmlBody { get; set; } //  fieldname is identified as "Html" as a cue that this should really be html content, but it is just a string... (can be whatever)


    }
}
