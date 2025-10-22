using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        private readonly List<SaleItem> _items;

        public Sale()
        {
            _items = new List<SaleItem>();
        }

        public int SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public string CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public string BranchId { get; private set; }
        public string BranchName { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        public static Sale Create(
            int saleNumber,
            string customerId,
            string customerName,
            string branchId,
            string branchName)
        {
            var sale = new Sale
            {
                SaleNumber = saleNumber,
                SaleDate = DateTime.UtcNow,
                CustomerId = customerId,
                CustomerName = customerName,
                BranchId = branchId,
                BranchName = branchName,
                IsCancelled = false
            };

            sale.AddDomainEvent(new SaleCreatedEvent(sale));
            return sale;
        }

        public void AddItem(string productId, string productName, int quantity, decimal unitPrice)
        {
            if (quantity > 20)
                throw new DomainException("Cannot sell more than 20 identical items");

            var discount = CalculateDiscount(quantity);
            var item = SaleItem.Create(this.Id, productId, productName, quantity, unitPrice, discount);
            
            _items.Add(item);
            RecalculateTotalAmount();
            
            AddDomainEvent(new SaleModifiedEvent(this));
        }

        public void CancelItem(string productId)
        {
            var item = _items.Find(i => i.ProductId == productId && !i.IsCancelled);
            if (item == null)
                throw new DomainException("Item not found or already cancelled");

            item.Cancel();
            RecalculateTotalAmount();
            
            AddDomainEvent(new ItemCancelledEvent(this, item));
            AddDomainEvent(new SaleModifiedEvent(this));
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Sale is already cancelled");

            IsCancelled = true;
            foreach (var item in _items)
            {
                if (!item.IsCancelled)
                    item.Cancel();
            }

            AddDomainEvent(new SaleCancelledEvent(this));
        }

        private decimal CalculateDiscount(int quantity)
        {
            if (quantity < 4)
                return 0;

            if (quantity >= 10 && quantity <= 20)
                return 0.2m;

            return 0.1m;
        }

        private void RecalculateTotalAmount()
        {
            TotalAmount = 0;
            foreach (var item in _items)
            {
                if (!item.IsCancelled)
                    TotalAmount += item.TotalAmount;
            }
        }
    }
}