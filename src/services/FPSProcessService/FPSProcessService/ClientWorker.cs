
namespace FPSProcessService;

public class ClientWorker : BackgroundService
{
    private readonly ILogger<ClientWorker> _logger;
    public Timer _timer = null;


    #region Override methods from BackgroundService
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartService();
        return Task.CompletedTask;
    }

    private void StartService()
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Stop();
        _logger.LogInformation("Timed Hosted Service is stopped. at {stoped}", DateTime.Now);
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }

    #endregion

    private void DoWork(object? state)
    {
        handleServiceStart();
    }

    private void handleServiceStart()
    {
        throw new NotImplementedException();
    }

    private void Stop()
    {
        throw new NotImplementedException();
    }
}
