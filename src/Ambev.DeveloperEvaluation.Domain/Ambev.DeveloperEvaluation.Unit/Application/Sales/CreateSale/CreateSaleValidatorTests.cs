using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale
{
    public class CreateSaleValidatorTests
    {
        private readonly CreateSaleValidator _validator;

        public CreateSaleValidatorTests()
        {
            _validator = new CreateSaleValidator();
        }

        [Fact]
        public void Validate_WithEmptyCustomerId_ShouldHaveError()
        {
            var command = new CreateSaleCommand { CustomerId = string.Empty };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CustomerId);
        }

        [Fact]
        public void Validate_WithTooLongCustomerId_ShouldHaveError()
        {
            var command = new CreateSaleCommand { CustomerId = new string('x', 51) };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CustomerId);
        }

        [Fact]
        public void Validate_WithNoItems_ShouldHaveError()
        {
            var command = new CreateSaleCommand();
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Items);
        }

        [Fact]
        public void Validate_WithItemQuantityTooHigh_ShouldHaveError()
        {
            var command = new CreateSaleCommand
            {
                CustomerId = "CUST123",
                CustomerName = "Test Customer",
                BranchId = "BRANCH123",
                BranchName = "Test Branch",
                Items = new()
                {
                    new() { ProductId = "PROD1", ProductName = "Test Product", Quantity = 21, UnitPrice = 100 }
                }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Items[0].Quantity");
        }
    }
}