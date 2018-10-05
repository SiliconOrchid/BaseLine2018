using System.Text;
using System.Threading.Tasks;
using BaseLine2018.Common.Logging;

namespace BaseLine2018.Email.RazorEngine
{
    public abstract class RazorEngineBaseTemplate<TTemplateViewModel>
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        // this will map to @Model (property name)
        public TTemplateViewModel Model;


        public void WriteLiteral(string literal)
        {
            // replace that by a text writer for example
            //Console.Write(literal);
            //Log.Debug(literal);
            stringBuilder.Append(literal);
        }

        public void Write(object obj)
        {
            // replace that by a text writer for example
            //Console.Write(obj);
            //Log.Debug(obj.ToString());
            stringBuilder.Append(obj.ToString());
        }

        public string GetMarkup()
        {
            return stringBuilder.ToString();
        }

        public async virtual Task ExecuteAsync()
        {
            await Task.Yield(); // whatever, we just need something that compiles...
        }
    }
}
