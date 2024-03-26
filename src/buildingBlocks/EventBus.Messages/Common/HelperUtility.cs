using EventBus.Messages.Events;

namespace EventBus.Messages.Common
{
    public static class HelperUtility
    {
        public static MessageEvent GetMessageEventObject(string eventName, object message)
        {
            return new MessageEvent
            {
                EventName = eventName,
                MessageObject = message
            };
        }
    }
}
