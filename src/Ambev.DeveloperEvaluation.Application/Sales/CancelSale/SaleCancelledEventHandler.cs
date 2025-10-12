using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledEvent>
    {
        private readonly ILogger<SaleCancelledEventHandler> _logger;

        public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sale {SaleNumber} was cancelled", notification.Sale.SaleNumber);
            // Here you would implement actual event publishing to a message broker
            return Task.CompletedTask;
        }
    }
}