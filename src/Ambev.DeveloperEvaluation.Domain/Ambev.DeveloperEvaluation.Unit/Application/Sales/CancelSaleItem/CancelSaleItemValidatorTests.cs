using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CancelSaleItem
{
    public class CancelSaleItemValidatorTests
    {
        private readonly CancelSaleItemValidator _validator;

        public CancelSaleItemValidatorTests()
        {
            _validator = new CancelSaleItemValidator();
        }

        [Fact]
        public void Validate_WithInvalidSaleNumber_ShouldHaveError()
        {
            var command = new CancelSaleItemCommand { SaleNumber = 0, ProductId = "PROD1" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.SaleNumber);
        }

        [Fact]
        public void Validate_WithEmptyProductId_ShouldHaveError()
        {
            var command = new CancelSaleItemCommand { SaleNumber = 1, ProductId = string.Empty };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ProductId);
        }

        [Fact]
        public void Validate_WithTooLongProductId_ShouldHaveError()
        {
            var command = new CancelSaleItemCommand { SaleNumber = 1, ProductId = new string('x', 51) };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ProductId);
        }
    }
}