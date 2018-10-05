
using System.Collections.Generic;

namespace BaseLine2018.Common.LookupData
{
    public static class Titles
    {
        public static Dictionary<int, string> ListTitles { get; set; }

        static Titles()
        {
            ListTitles = new Dictionary<int, string>
            {
                {1, "Mr"},
                {2, "Ms"},
                {3, "Mrs"},
                {4, "Miss"},
                {5, "Dr"}
            };
        }
    }
}
