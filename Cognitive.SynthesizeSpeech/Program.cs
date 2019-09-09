using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Cognitive.TextTranslation;
using Microsoft.CognitiveServices.Speech;
namespace Cognitive.SynthesizeSpeech
{
    class Program
    {
        public static async Task SynthesisToSpeakerAsync()
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription("XXXXXXXXXXX", "westus");
            string textToSpeak = "I am Asif Khan";
            // Creates a speech synthesizer using the default speaker as audio output.
            using (var synthesizer = new SpeechSynthesizer(config))
            {
                string inputText = "My name is Asif Khan. I live in sydney";
                string host = "https://api.cognitive.microsofttranslator.com";
                string route = "/translate?api-version=3.0&to=hi&to=ur";
                TranslationResult[] _result = await TranslateTextHelper.GetInstance.TranslateInputText4Speech(host, route, inputText);
                foreach (TranslationResult o in _result)
                {
                    // Print the detected input language and confidence score.
                    Console.WriteLine("Detected input language: {0}\nConfidence score: {1}\n", o.DetectedLanguage.Language, o.DetectedLanguage.Score);
                    // Iterate over the results and print each translation.
                    foreach (Translation t in o.Translations)
                    {
                        if (t.To.ToUpper().Equals("HI"))
                        {
                            textToSpeak = t.Text;
                        }
                        Console.WriteLine("Translated to {0}: {1}", t.To, t.Text);
                    }
                }

                //    // Receive a text from console input and synthesize it to speaker.
                //Console.WriteLine("Type some text that you want to speak...");
                //Console.Write("> ");
                //string text = Console.ReadLine();

                //textToSpeak = "मेरा नाम आसिफ खान है। मैं सिडनी में रहते हैं";
                if (!string.IsNullOrEmpty(textToSpeak))
                {

                    //string ssmlText = "<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='en-US' >"
                    //    + "<voice lang='hi-IN'  name = 'Microsoft Server Speech Text to Speech Voice (hi-IN, Kalpana)' > "
                    //    + "मेरा नाम आसिफ खान है। मैं सिडनी में रहते हैं"
                    //    + "</ voice >"
                    //    + "</ speak >";                 
                    string ssmlText = GenerateSsml("hi-IN", "Female", "Microsoft Server Speech Text to Speech Voice (hi-IN, Kalpana)", textToSpeak);


                    using (var result = await synthesizer.SpeakSsmlAsync(ssmlText))
                    {
                        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                        {
                            Console.WriteLine($"Speech synthesized to speaker for text [{textToSpeak}]");
                        }
                        else if (result.Reason == ResultReason.Canceled)
                        {
                            var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                            Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                            if (cancellation.Reason == CancellationReason.Error)
                            {
                                Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                                Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                                Console.WriteLine($"CANCELED: Did you update the subscription info?");
                            }
                        }
                    }
                }
            }
        }
        static string GenerateSsml(string lang, string gender, string name, string text)
        {
            var ssmlDoc = new XDocument(
                              new XElement("speak",
                                  new XAttribute("version", "1.0"),
                                  new XAttribute(XNamespace.Xml + "lang", "en-US"),
                                  new XElement("voice",
                                      new XAttribute(XNamespace.Xml + "lang", lang),
                                      new XAttribute(XNamespace.Xml + "gender", gender),
                                      new XAttribute("name", name),
                                      text)));
            return ssmlDoc.ToString();
        }
        static void Main()
        {
            SynthesisToSpeakerAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
