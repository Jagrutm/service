using FluentAssertions;

namespace BuildingBlocks.Tests.Extensions
{
    public static class ExceptionExtension
    {
        public static string ThrowExceptionExactly<TException>(
            this Func<Task> value, 
            string? expectedMessage = null) where TException : Exception
        {
            if (!string.IsNullOrEmpty(expectedMessage))
            {
                value.Should()
                     .ThrowExactlyAsync<TException>()
                     .WithMessage(expectedMessage);
            }

            return expectedMessage;
        }
    }
}
