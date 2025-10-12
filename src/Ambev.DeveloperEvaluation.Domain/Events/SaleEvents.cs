using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent : DomainEvent
    {
        public Sale Sale { get; }

        public SaleCreatedEvent(Sale sale)
        {
            Sale = sale;
        }
    }

    public class SaleModifiedEvent : DomainEvent
    {
        public Sale Sale { get; }

        public SaleModifiedEvent(Sale sale)
        {
            Sale = sale;
        }
    }

    public class SaleCancelledEvent : DomainEvent
    {
        public Sale Sale { get; }

        public SaleCancelledEvent(Sale sale)
        {
            Sale = sale;
        }
    }

    public class ItemCancelledEvent : DomainEvent
    {
        public Sale Sale { get; }
        public SaleItem Item { get; }

        public ItemCancelledEvent(Sale sale, SaleItem item)
        {
            Sale = sale;
            Item = item;
        }
    }
}