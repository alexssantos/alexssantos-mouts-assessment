using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale
{
    public class UpdateSaleValidatorTests
    {
        private readonly UpdateSaleValidator _validator;

        public UpdateSaleValidatorTests()
        {
            _validator = new UpdateSaleValidator();
        }

        [Fact]
        public void Validate_WithInvalidSaleNumber_ShouldHaveError()
        {
            var command = new UpdateSaleCommand { SaleNumber = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.SaleNumber);
        }

        [Fact]
        public void Validate_WithNoItems_ShouldHaveError()
        {
            var command = new UpdateSaleCommand { SaleNumber = 1 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Items);
        }

        [Fact]
        public void Validate_WithItemQuantityTooHigh_ShouldHaveError()
        {
            var command = new UpdateSaleCommand
            {
                SaleNumber = 1,
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