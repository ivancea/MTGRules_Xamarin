using MTGRules.Interfaces;
using System;
using System.Linq;
using Windows.Media.SpeechSynthesis;
using Xamarin.Forms;

[assembly: Dependency(typeof(MTGRules.UWP.TextToSpeech))]
namespace MTGRules.UWP
{
    public class TextToSpeech : ITextToSpeech
    {
        private static Windows.UI.Xaml.Controls.MediaElement mediaElement = new Windows.UI.Xaml.Controls.MediaElement();

        public async void Speak(string text)
        {
            using (var speech = new SpeechSynthesizer())
            {
                speech.Voice = (SpeechSynthesizer.DefaultVoice.Language.StartsWith("en")
                    ? SpeechSynthesizer.DefaultVoice
                    : SpeechSynthesizer.AllVoices.FirstOrDefault(
                        voice => voice.Language.StartsWith("en")
                    ));
                var stream = await speech.SynthesizeTextToStreamAsync(text);

                mediaElement.Stop();
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();
            }
        }
    }
}
