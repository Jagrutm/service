using Moq;

namespace BuildingBlocks.Tests.Extensions
{
    public static class ServiceMockExtensions
    {
        public static TValue IsEquivalentTo<TValue>(TValue expectedValue)
        {
            return It.Is<TValue>(actualValue => IsEquivalentTo(actualValue, expectedValue));
        }

        private static bool IsEquivalentTo<TValue>(TValue expectedValue, TValue actualValue)
        {
            try
            {
                actualValue.ShouldBeEquivalent(expectedValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
