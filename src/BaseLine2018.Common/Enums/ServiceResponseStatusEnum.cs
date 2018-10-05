using System.ComponentModel;

namespace BaseLine2018.Common.Enums
{
    public enum ServiceResponseStatusEnum
    {
        [Description("Unset")]
        Unset = 0,

        [Description("Ok")]
        Ok = 1,

        [Description("Ok, No Data")]
        Ok_NoData = 2,

        // typical expected usage: some major or unexpected fault - would return an http500 error
        [Description("Failure, was not Handled")] 
        Fail_Unhandled = 3,

        // typical expected usage: some minor or handled fault - would return an http200 ok, but supply an error message as the body - for example, a data problem where a business-rule could not be met, etc
        [Description("Failure, was Handled")]
        Fail_Handled = 4
    }
}
