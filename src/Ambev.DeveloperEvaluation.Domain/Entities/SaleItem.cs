using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        private SaleItem() { }

        public Guid SaleId { get; private set; }
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }

        public static SaleItem Create(
            Guid saleId,
            string productId,
            string productName,
            int quantity,
            decimal unitPrice,
            decimal discount)
        {
            var item = new SaleItem
            {
                SaleId = saleId,
                ProductId = productId,
                ProductName = productName,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Discount = discount,
                IsCancelled = false
            };

            item.CalculateTotalAmount();
            return item;
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Item is already cancelled");

            IsCancelled = true;
            TotalAmount = 0;
        }

        private void CalculateTotalAmount()
        {
            var subtotal = Quantity * UnitPrice;
            var discountAmount = subtotal * Discount;
            TotalAmount = subtotal - discountAmount;
        }
    }
}