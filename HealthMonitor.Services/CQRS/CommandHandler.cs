using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMonitor.Services.CQRS
{
    public interface ICommand<T>
    {
        CommandParameters<T> Parameters { get; }
    }

    public abstract class CommandHandler<TCommand, T, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<T>, IRequest<TResponse>, IRemoteableRequest
    {

        private readonly ILogger<CommandHandler<TCommand, T, TResponse>> _logger;

        public Func<TCommand, Task<TResponse>> CommandFunc { get; set; }

        public CommandHandler(ILogger<CommandHandler<TCommand, T, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (CommandFunc == null)
                {
                    return HandleException(new Exception($"CommandFunc wasn't initialized for {typeof(TCommand)}"));
                }

                var response = await CommandFunc(command);
                if (command.Parameters.AuditLog)
                {
                   //log something
                }

                return response;
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private TResponse HandleException(Exception ex)
        {
            _logger.LogError($"Command {typeof(TCommand)} encountered an issue: {ex.Message}", ex);
            return default;
        }
    }
}
