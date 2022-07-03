using Microsoft.AspNetCore.Mvc;
using Points.TaxCalculator.Core.Contracts;
using Points.TaxCalculator.Core.Exceptions;

namespace Points.TaxCalculator.API.Controllers;

[ApiController]
[Route("tax-calculator")]
public class TaxCalculatorController : ControllerBase
{
    private readonly ILogger<TaxCalculatorController> _logger;
    private readonly ITaxCalculatorService _taxCalculatorService;

    public TaxCalculatorController(ILogger<TaxCalculatorController> logger, ITaxCalculatorService taxCalculatorService)
    {
        _logger = logger;
        _taxCalculatorService = taxCalculatorService;
    }

    /// <summary>
    /// Http method to calculate income tax amount based on year and income
    /// </summary>
    /// <param name="year"></param>
    /// <param name="income"></param>
    /// <returns>Income tax amount</returns>
    [HttpGet]
    public async Task<IActionResult> Get(int year, decimal income)
    {
        try
        {
            var result = await _taxCalculatorService
                .CalculateIncomeTaxBasedOnYearAndIncome(year, income);

            return Ok(new { incomeTax = result });
        }
        catch (YearNotSupportedException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ExternalApiFailedException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"tax-calculator?year={year}&income={income}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}