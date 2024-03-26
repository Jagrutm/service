using System.Text.Json;

namespace EventBus.Messages.Events
{
    public class MessageEvent
    {
        public MessageEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public DateTime CreationDate { get; private set; }

        public string EventName { get; set; }

        public string Message { get; private set; }

        public object MessageObject
        {
            set
            {
                Message = JsonSerializer.Serialize(value);
            }
        }
    }
}
