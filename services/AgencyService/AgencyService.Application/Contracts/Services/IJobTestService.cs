namespace AgencyService.Application.Contracts.Services
{
    public interface IJobTestService
    {
        Task FireAndForgetJobAsync();

        Task ReccuringJobAsync();

        Task DelayedJobAsync();

        Task ContinuationJobAsync();
    }
}
