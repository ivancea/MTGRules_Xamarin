using Microsoft.Services.Store.Engagement;
using MTGRules.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(MTGRules.UWP.EventLogger))]
namespace MTGRules.UWP
{
    public class EventLogger : IEventLogger
    {
        private static readonly StoreServicesCustomEventLogger CustomEventlogger = StoreServicesCustomEventLogger.GetDefault();

        public void Log(EventType eventType)
        {
            CustomEventlogger.Log(eventType.ToString());
        }
    }
}
