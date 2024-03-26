using MediatR;
using Microsoft.Extensions.Logging;
using BuildingBlocks.Dapper.Contexts;

namespace BuildingBlocks.Dapper.Behaviors
{
    //public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    //{
    //    private readonly IDapperContext _context;
    //    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    //    public TransactionBehavior(IDapperContext context, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    //    {
    //        _context = context ?? throw new ArgumentNullException(nameof(context));
    //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //    }

    //    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    //    {
    //        try
    //        {
    //            _logger.LogInformation($"Begin transaction {typeof(TRequest).Name}");

    //            await _context.BeginTransactionAsync();

    //            var response = await next();

    //            await _context.CommitTransactionAsync();

    //            _logger.LogInformation($"Committed transaction {typeof(TRequest).Name}");

    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogInformation($"Rollback transaction executed {typeof(TRequest).Name}");

    //            await _context.RollbackTransactionAsync();

    //            _logger.LogError(ex.Message, ex.StackTrace);

    //            throw;
    //        }
    //    }
    //}
}
