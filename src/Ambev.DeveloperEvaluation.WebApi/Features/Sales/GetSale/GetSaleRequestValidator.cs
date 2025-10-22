using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Validator for GetSaleRequest
/// </summary>
public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the GetSaleRequestValidator with defined validation rules
    /// </summary>
    public GetSaleRequestValidator()
    {
        RuleFor(x => x.SaleNumber)
            .GreaterThan(0)
            .WithMessage("Sale number must be greater than 0");
    }
}