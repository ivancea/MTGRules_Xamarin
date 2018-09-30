namespace MTGRules.Interfaces
{
    public enum EventType
    {
        CompareRules,
        SearchText,
        RandomRule,
        TextToSpeech
    }

    public interface IEventLogger
    {
        void Log(EventType eventType);
    }
}
