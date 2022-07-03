namespace Points.TaxCalculator.Core.Contracts;

public interface ITaxCalculatorService
{
    Task<decimal> CalculateIncomeTaxBasedOnYearAndIncome(int year, decimal income);
}