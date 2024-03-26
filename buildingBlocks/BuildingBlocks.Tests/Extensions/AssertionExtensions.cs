using FluentAssertions;

namespace BuildingBlocks.Tests.Extensions
{
    public static partial class AssertionExtensions
    {
        public static bool ShouldBeEquivalent<TValue>(this TValue value, TValue expectedValue)
        {
            value.Should().BeEquivalentTo(expectedValue);
            return true;
        }
    }
}
