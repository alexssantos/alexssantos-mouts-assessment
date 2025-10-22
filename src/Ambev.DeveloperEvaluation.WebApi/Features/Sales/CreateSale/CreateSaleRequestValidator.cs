using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleRequestValidator with defined validation rules
    /// </summary>
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required")
            .MinimumLength(3).WithMessage("Customer ID must be at least 3 characters long");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required")
            .MinimumLength(3).WithMessage("Branch ID must be at least 3 characters long");

        RuleFor(x => x.SaleDate)
            .NotEmpty().WithMessage("Sale date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required")
            .ForEach(item =>
            {
                item.SetValidator(new CreateSaleItemRequestValidator());
            });
    }
}

/// <summary>
/// Validator for CreateSaleItemRequest
/// </summary>
public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleItemRequestValidator with defined validation rules
    /// </summary>
    public CreateSaleItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0");
    }
}