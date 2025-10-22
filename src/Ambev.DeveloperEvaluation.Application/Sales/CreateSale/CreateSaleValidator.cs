using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.CustomerName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.BranchId)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.BranchName)
                .NotEmpty()
                .MaximumLength(200);

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