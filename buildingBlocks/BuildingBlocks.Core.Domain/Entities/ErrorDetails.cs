using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BuildingBlocks.Core.Domain.Entities
{
    public class ErrorDetails
    {
        private IEnumerable<string>? _errors;
        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public string Error { get; private set; } = string.Empty;

        [JsonIgnore]
        public IEnumerable<string> Errors
        {
            get { return _errors ?? new List<string>(); }
            set
            {
                _errors = value;
                Error = JsonSerializer.Serialize(value);
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
