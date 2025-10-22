using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class SaleModifiedEventHandler : INotificationHandler<SaleModifiedEvent>
    {
        private readonly ILogger<SaleModifiedEventHandler> _logger;

        public SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sale {SaleNumber} was modified", notification.Sale.SaleNumber);
            // Here you would implement actual event publishing to a message broker
            return Task.CompletedTask;
        }
    }
}