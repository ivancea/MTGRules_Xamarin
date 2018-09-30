using Android.Speech.Tts;
using Java.Util;
using MTGRules.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(MTGRules.Droid.TextToSpeechImpl))]
namespace MTGRules.Droid
{
    public class TextToSpeechImpl : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public void Speak(string text)
        {
            toSpeak = text;

            if (speaker == null)
            {
                speaker = new TextToSpeech(MainActivity.Instance, this);
            }
            else
            {
                SpeakIt();
            }
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.SetLanguage(Locale.English);
                SpeakIt();
            }
        }

        private void SpeakIt()
        {
            speaker.Speak(toSpeak, QueueMode.Flush, null, null);
        }
    }
}