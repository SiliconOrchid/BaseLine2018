
using System.Collections.Generic;

namespace BaseLine2018.Api.EmailTemplates
{
    public class SampleEmailTemplateModel
    {
        public string EmailHeader { get; set; }
        public List<SampleEmailTemplateModelCollectionItem> ListCollectionItems { get; set; }


        public SampleEmailTemplateModel()
        {
            ListCollectionItems = new List<SampleEmailTemplateModelCollectionItem>();
        }

    }
    public class SampleEmailTemplateModelCollectionItem
    {
        public string CollectionItemDescription { get; set; }
    }

}
