using MediatR;
using Microsoft.Extensions.Logging;
using StockNexusAPI.Application.Common.Interfaces;
using System.Diagnostics;


namespace StockNexusAPI.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId;

            _logger.LogInformation("Handling {RequestName} for UserId: {UserId}", requestName, userId);

            var timer = Stopwatch.StartNew();
            TResponse response;

            try
            {
                response = await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StockNexusAPI Request: {RequestName} for UserId: {UserId} threw an exception", requestName, userId);
                throw;
            }
            finally
            {
                timer.Stop();
                var elapsedMilliseconds = timer.ElapsedMilliseconds;
                _logger.LogInformation("StockNexusAPI Request: {RequestName} completed successfully in {ElapsedMilliseconds} ms", requestName, elapsedMilliseconds);

            }

            return response;
        }
    }
}
