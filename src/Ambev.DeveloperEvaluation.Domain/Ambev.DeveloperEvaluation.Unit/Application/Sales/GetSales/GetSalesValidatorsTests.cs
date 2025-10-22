using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSales
{
    public class GetSalesValidatorsTests
    {
        [Fact]
        public void GetSaleByNumberValidator_WithInvalidSaleNumber_ShouldHaveError()
        {
            var validator = new GetSaleByNumberValidator();
            var query = new GetSaleByNumberQuery { SaleNumber = 0 };
            var result = validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.SaleNumber);
        }

        [Fact]
        public void GetSalesByCustomerValidator_WithEmptyCustomerId_ShouldHaveError()
        {
            var validator = new GetSalesByCustomerValidator();
            var query = new GetSalesByCustomerQuery { CustomerId = string.Empty };
            var result = validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.CustomerId);
        }

        [Fact]
        public void GetSalesByBranchValidator_WithEmptyBranchId_ShouldHaveError()
        {
            var validator = new GetSalesByBranchValidator();
            var query = new GetSalesByBranchQuery { BranchId = string.Empty };
            var result = validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.BranchId);
        }

        [Fact]
        public void GetSalesByDateRangeValidator_WithEndDateBeforeStartDate_ShouldHaveError()
        {
            var validator = new GetSalesByDateRangeValidator();
            var query = new GetSalesByDateRangeQuery 
            { 
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(-1)
            };
            var result = validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.EndDate);
        }
    }
}