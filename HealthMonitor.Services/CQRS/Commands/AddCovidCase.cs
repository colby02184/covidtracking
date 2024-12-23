using HealthMonitor.Data.Repositories;
using HealthMonitor.Framework;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Services.CQRS.Commands
{
    public class AddCovidCase
    {
        public record Command(CommandParameters<CovidData> Parameters) : ICommand<CovidData>, IRequest<Response<bool>>, IRemoteableRequest;

        public class Handler : CommandHandler<Command, CovidData, Response<bool>>
        {
            private readonly ICovidCaseRepository _repository;

            public Handler(ILogger<Handler> logger, ICovidCaseRepository repository) : base(logger)
            {
                _repository = repository;
                CommandFunc = CommandHandler;
            }

            private async Task<Response<bool>> CommandHandler(Command command)
            {
                try
                {
                    await _repository.AddAsync(command.Parameters.Data);
                    return new Response<bool>(true);
                }
                catch (Exception e)
                {
                    return new Response<bool>(false, false, new List<ResponseError>
                    {
                        new ResponseError
                        {
                            Message = e.Message
                        }
                    });
                }
            }
        }
    }
}
