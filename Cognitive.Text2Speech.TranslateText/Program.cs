using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cognitive.TextTranslation;

namespace Cognitive.Text2Speech.TranslateText
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string inputText = "HI, My Name is Asif Khan";
            // This is our main function.
            // Output languages are defined in the route.
            // For a complete list of options, see API reference.
            // https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate
            string host = "https://api.cognitive.microsofttranslator.com";
            string route = "/translate?api-version=3.0&to=hi&to=ur";
            await TranslateTextHelper.GetInstance.TranslateInputText(host, route, inputText);
        }


    }

}
