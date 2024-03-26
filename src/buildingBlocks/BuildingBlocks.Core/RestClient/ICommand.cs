namespace BuildingBlocks.Core.RestClient
{
    public interface ICommand
    {
        public string Name { get; }

        Task ExecuteAsync(IServiceProvider serviceProvider);
    }
}
