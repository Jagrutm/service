namespace EventBus.Messages.Events
{
    public class EventOne : IntegrationBaseEvent
    {
        public string EventName { get; set; }
        public long ReferenceID { get; set; }
        public string Message { get; set; }
    }
}
