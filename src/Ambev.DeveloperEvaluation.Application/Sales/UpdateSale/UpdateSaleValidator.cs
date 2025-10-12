using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleValidator()
        {
            RuleFor(x => x.SaleNumber)
                .GreaterThan(0);

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one item is required");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .MaximumLength(50);

                item.RuleFor(x => x.ProductName)
                    .NotEmpty()
                    .MaximumLength(200);

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .LessThanOrEqualTo(20)
                    .WithMessage("Quantity must be between 1 and 20");

                item.RuleFor(x => x.UnitPrice)
                    .GreaterThan(0)
                    .WithMessage("Unit price must be greater than 0");
            });
        }
    }
}