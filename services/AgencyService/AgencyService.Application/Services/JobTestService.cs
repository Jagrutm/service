using AgencyService.Application.Contracts.Services;

namespace AgencyService.Application.Services
{
    public class JobTestService : IJobTestService
    {
        public async Task FireAndForgetJobAsync()
        {
            Console.WriteLine("Hello from a Fire and Forget job!");
            await Task.CompletedTask;
        }

        public async Task ReccuringJobAsync()
        {
            Console.WriteLine("Hello from a Scheduled job!");
            await Task.CompletedTask;
        }

        public async Task DelayedJobAsync()
        {
            Console.WriteLine("Hello from a Delayed job!");
            await Task.CompletedTask;
        }

        public async Task ContinuationJobAsync()
        {
            Console.WriteLine("Hello from a Continuation job!");
            await Task.CompletedTask;
        }
    }
}
