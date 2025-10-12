using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    public class CancelSaleItemValidator : AbstractValidator<CancelSaleItemCommand>
    {
        public CancelSaleItemValidator()
        {
            RuleFor(x => x.SaleNumber)
                .GreaterThan(0);

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}