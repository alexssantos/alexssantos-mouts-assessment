using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    public class ItemCancelledEventHandler : INotificationHandler<ItemCancelledEvent>
    {
        private readonly ILogger<ItemCancelledEventHandler> _logger;

        public ItemCancelledEventHandler(ILogger<ItemCancelledEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Item {ProductId} from sale {SaleNumber} was cancelled",
                notification.Item.ProductId,
                notification.Sale.SaleNumber);
            // Here you would implement actual event publishing to a message broker
            return Task.CompletedTask;
        }
    }
}