using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//Supported Languages : https://docs.microsoft.com/en-us/azure/cognitive-services/Translator/language-support
namespace Cognitive.TextTranslation
{
    public class TranslateTextHelper
    {
        private static TranslateTextHelper _instace = null;
        private static readonly object padlock = new object();
        TranslateTextHelper() { }

        public static TranslateTextHelper GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (_instace == null)
                    {
                        _instace = new TranslateTextHelper();
                    }
                    return _instace;
                }
            }
        }
        public async Task TranslateInputText(string host, string route, string _Text)
        {
            //PWC NON PROD--sit-nif-mdm-test-df-37-rg-->facedetectpoc
            string subscriptionKey = "XXXXXXXXXXXXXXXX";
            // Prompts you for text to translate. If you'd prefer, you can
            // provide a string as textToTranslate.
            Console.Write("Processing Input Text");


            object[] body = new object[] { new { Text = _Text } };
            var requestBody = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient())
                try
                {
                    using (var request = new HttpRequestMessage())
                    {
                        // Build the request.
                        // Set the method to Post.
                        request.Method = HttpMethod.Post;
                        // Construct the URI and add headers.
                        request.RequestUri = new Uri(host + route);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                        // Send the request and get response.
                        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                        // Read response as a string.
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            // Deserialize the response using the classes created earlier.
                            TranslationResult[] deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);
                            // Iterate over the deserialized results.
                            foreach (TranslationResult o in deserializedOutput)
                            {
                                // Print the detected input language and confidence score.
                                Console.WriteLine("Detected input language: {0}\nConfidence score: {1}\n", o.DetectedLanguage.Language, o.DetectedLanguage.Score);
                                // Iterate over the results and print each translation.
                                foreach (Translation t in o.Translations)
                                {
                                    Console.WriteLine("Translated to {0}: {1}", t.To, t.Text);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
        }
        public async Task<TranslationResult[]> TranslateInputText4Speech(string host, string route, string _Text)
        {
            //PWC NON PROD--sit-nif-mdm-test-df-37-rg-->facedetectpoc
            string subscriptionKey = "XXXXXXXXXXXXXXXXXXXXXXXX";
            // Prompts you for text to translate. If you'd prefer, you can
            // provide a string as textToTranslate.
            Console.Write("Processing Input Text");
            TranslationResult[] deserializedOutput = null;

            object[] body = new object[] { new { Text = _Text } };
            var requestBody = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient())
                try
                {
                    using (var request = new HttpRequestMessage())
                    {
                        // Build the request.
                        // Set the method to Post.
                        request.Method = HttpMethod.Post;
                        // Construct the URI and add headers.
                        request.RequestUri = new Uri(host + route);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                        // Send the request and get response.
                        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                        // Read response as a string.
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            // Deserialize the response using the classes created earlier.
                            deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);

                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            return deserializedOutput;
        }
    }
}
