using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Validator for CancelSaleRequest
/// </summary>
public class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the CancelSaleRequestValidator with defined validation rules
    /// </summary>
    public CancelSaleRequestValidator()
    {
        RuleFor(x => x.SaleNumber)
            .GreaterThan(0)
            .WithMessage("Sale number must be greater than 0");

        RuleFor(x => x.CancellationReason)
            .NotEmpty().WithMessage("Cancellation reason is required")
            .MinimumLength(10).WithMessage("Cancellation reason must be at least 10 characters long")
            .MaximumLength(500).WithMessage("Cancellation reason must not exceed 500 characters");
    }
}