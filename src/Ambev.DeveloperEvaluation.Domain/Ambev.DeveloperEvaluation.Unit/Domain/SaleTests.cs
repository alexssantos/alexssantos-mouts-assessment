using System;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain
{
    public class SaleTests
    {
        [Fact]
        public void Create_ShouldCreateSaleWithCorrectProperties()
        {
            // Arrange
            var saleNumber = 1;
            var customerId = "CUST123";
            var customerName = "Test Customer";
            var branchId = "BRANCH123";
            var branchName = "Test Branch";

            // Act
            var sale = Sale.Create(saleNumber, customerId, customerName, branchId, branchName);

            // Assert
            Assert.NotNull(sale);
            Assert.Equal(saleNumber, sale.SaleNumber);
            Assert.Equal(customerId, sale.CustomerId);
            Assert.Equal(customerName, sale.CustomerName);
            Assert.Equal(branchId, sale.BranchId);
            Assert.Equal(branchName, sale.BranchName);
            Assert.False(sale.IsCancelled);
            Assert.Empty(sale.Items);
            Assert.Contains(sale.DomainEvents, e => e is SaleCreatedEvent);
        }

        [Fact]
        public void AddItem_WithValidQuantity_ShouldAddItemAndCalculateDiscount()
        {
            // Arrange
            var sale = CreateTestSale();

            // Act
            sale.AddItem("PROD1", "Test Product", 5, 100);

            // Assert
            var item = sale.Items.Single();
            Assert.Equal("PROD1", item.ProductId);
            Assert.Equal("Test Product", item.ProductName);
            Assert.Equal(5, item.Quantity);
            Assert.Equal(100, item.UnitPrice);
            Assert.Equal(0.1m, item.Discount); // 10% discount for quantity >= 4
            Assert.Equal(450m, item.TotalAmount); // (5 * 100) * 0.9
            Assert.Contains(sale.DomainEvents, e => e is SaleModifiedEvent);
        }

        [Fact]
        public void AddItem_WithQuantityOver20_ShouldThrowException()
        {
            // Arrange
            var sale = CreateTestSale();

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
                sale.AddItem("PROD1", "Test Product", 21, 100));
            Assert.Equal("Cannot sell more than 20 identical items", exception.Message);
        }

        [Theory]
        [InlineData(3, 0)] // No discount
        [InlineData(4, 0.1)] // 10% discount
        [InlineData(10, 0.2)] // 20% discount
        [InlineData(15, 0.2)] // 20% discount
        public void AddItem_ShouldApplyCorrectDiscount(int quantity, decimal expectedDiscount)
        {
            // Arrange
            var sale = CreateTestSale();

            // Act
            sale.AddItem("PROD1", "Test Product", quantity, 100);

            // Assert
            var item = sale.Items.Single();
            Assert.Equal(expectedDiscount, item.Discount);
        }

        [Fact]
        public void CancelItem_ShouldCancelItemAndUpdateTotalAmount()
        {
            // Arrange
            var sale = CreateTestSale();
            sale.AddItem("PROD1", "Test Product", 5, 100);
            var initialTotal = sale.TotalAmount;

            // Act
            sale.CancelItem("PROD1");

            // Assert
            var item = sale.Items.Single();
            Assert.True(item.IsCancelled);
            Assert.Equal(0, sale.TotalAmount);
            Assert.Contains(sale.DomainEvents, e => e is ItemCancelledEvent);
            Assert.Contains(sale.DomainEvents, e => e is SaleModifiedEvent);
        }

        [Fact]
        public void Cancel_ShouldCancelSaleAndAllItems()
        {
            // Arrange
            var sale = CreateTestSale();
            sale.AddItem("PROD1", "Test Product 1", 5, 100);
            sale.AddItem("PROD2", "Test Product 2", 3, 50);

            // Act
            sale.Cancel();

            // Assert
            Assert.True(sale.IsCancelled);
            Assert.All(sale.Items, item => Assert.True(item.IsCancelled));
            Assert.Equal(0, sale.TotalAmount);
            Assert.Contains(sale.DomainEvents, e => e is SaleCancelledEvent);
        }

        private static Sale CreateTestSale()
        {
            return Sale.Create(1, "CUST123", "Test Customer", "BRANCH123", "Test Branch");
        }
    }
}