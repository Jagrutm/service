using Newtonsoft.Json;

namespace BuildingBlocks.Tests.Extensions
{
    public static class ObjectExtensions
    {
        public static TValue ToClone<TValue>(this TValue value)
        {
            var serializedValue = JsonConvert.SerializeObject(value);

            return JsonConvert.DeserializeObject<TValue>(serializedValue);
        }
    }
}
