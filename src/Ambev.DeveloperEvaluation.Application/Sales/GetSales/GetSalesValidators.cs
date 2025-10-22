using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSaleByNumberValidator : AbstractValidator<GetSaleByNumberQuery>
    {
        public GetSaleByNumberValidator()
        {
            RuleFor(x => x.SaleNumber)
                .GreaterThan(0);
        }
    }

    public class GetSalesByCustomerValidator : AbstractValidator<GetSalesByCustomerQuery>
    {
        public GetSalesByCustomerValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .MaximumLength(50);
        }
    }

    public class GetSalesByBranchValidator : AbstractValidator<GetSalesByBranchQuery>
    {
        public GetSalesByBranchValidator()
        {
            RuleFor(x => x.BranchId)
                .NotEmpty()
                .MaximumLength(50);
        }
    }

    public class GetSalesByDateRangeValidator : AbstractValidator<GetSalesByDateRangeQuery>
    {
        public GetSalesByDateRangeValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty();

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate);
        }
    }
}