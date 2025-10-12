using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
    {
        private readonly ILogger<SaleCreatedEventHandler> _logger;

        public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sale {SaleNumber} was created", notification.Sale.SaleNumber);
            // Here you would implement actual event publishing to a message broker
            return Task.CompletedTask;
        }
    }
}