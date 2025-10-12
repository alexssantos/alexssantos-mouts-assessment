using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CancelSale
{
    public class CancelSaleValidatorTests
    {
        private readonly CancelSaleValidator _validator;

        public CancelSaleValidatorTests()
        {
            _validator = new CancelSaleValidator();
        }

        [Fact]
        public void Validate_WithInvalidSaleNumber_ShouldHaveError()
        {
            var command = new CancelSaleCommand { SaleNumber = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.SaleNumber);
        }

        [Fact]
        public void Validate_WithValidSaleNumber_ShouldNotHaveError()
        {
            var command = new CancelSaleCommand { SaleNumber = 1 };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.SaleNumber);
        }
    }
}