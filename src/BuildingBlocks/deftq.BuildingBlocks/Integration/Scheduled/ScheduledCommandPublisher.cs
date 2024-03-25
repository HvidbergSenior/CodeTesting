using System.Diagnostics.CodeAnalysis;
using deftq.BuildingBlocks.DataAccess;
using MediatR;
using Microsoft.Extensions.Logging;

namespace deftq.BuildingBlocks.Integration.Scheduled
{
    internal sealed class ScheduledCommandPublisher
    {
        private readonly ISender sender;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandSchedulerRepository repository;
        private readonly ILogger<ScheduledCommandPublisher> logger;

        public ScheduledCommandPublisher(ISender sender, IUnitOfWork unitOfWork, ICommandSchedulerRepository repository, ILogger<ScheduledCommandPublisher> logger)
        {
            this.sender = sender;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.logger = logger;
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "<Pending>")]
        internal async Task<bool> PublishPendingAsync(CancellationToken stoppingToken)
        {
            var serializer = new ScheduledCommandSerializer();
            var messages = await repository.GetDueForProcessing(stoppingToken);
            if (messages is not null)
            {
                foreach (var message in messages)
                {
                    try
                    {
                        var cmd = serializer.Deserialize(message.Payload, message.CommandType);
                        if (cmd is not null)
                        {
                            await sender.Send(cmd, stoppingToken);
                            message.Published();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Processing ScheduledCommand {Id} failed", message.Id);
                        message.PublishFailed();
                    }
                    await repository.Update(message, stoppingToken);
                }
            }
            await unitOfWork.Commit(stoppingToken);
            return false;
        }
    }
}
