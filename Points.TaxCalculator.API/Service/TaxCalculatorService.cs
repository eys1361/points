using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Points.TaxCalculator.Core.Contracts;
using Points.TaxCalculator.Core.Exceptions;
using Points.TaxCalculator.Core.Models;

namespace Points.TaxCalculator.API.Service;

public class TaxCalculatorService : ITaxCalculatorService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public TaxCalculatorService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    /// <summary>
    ///     Calculates income tax based on tax bracket and income amount
    /// </summary>
    /// <param name="year"></param>
    /// <param name="income"></param>
    /// <returns>Tax amount</returns>
    public async Task<decimal> CalculateIncomeTaxBasedOnYearAndIncome(int year, decimal income)
    {
        var taxBracket = await GetTaxBracketForSpecificYearAndIncome(year, income);

        var incomeTax = income * taxBracket.Rate;

        return incomeTax;
    }

    /// <summary>
    ///     Gets tax bracket based on year & income
    /// </summary>
    /// <param name="year"></param>
    /// <param name="income"></param>
    /// <returns>Tax bracket</returns>
    private async Task<TaxBracket> GetTaxBracketForSpecificYearAndIncome(int year, decimal income)
    {
        var allBracketsForGivenYear = await GetTaxBracketsForSpecificYear(year);

        var taxBracketForGivenYearAndIncome =
            allBracketsForGivenYear.Brackets.FirstOrDefault(x => x.Min < income && x.Max >= income);

        return taxBracketForGivenYearAndIncome ?? new TaxBracket();
    }

    /// <summary>
    ///     Gets all tax brackets for a given year
    /// </summary>
    /// <param name="year"></param>
    /// <returns>Tax brackets</returns>
    /// <exception cref="YearNotSupportedException"></exception>
    private async Task<TaxBrackets> GetTaxBracketsForSpecificYear(int year)
    {
        var taxCalculationUrl = _configuration["TaxCalculatorUrl"];
        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var apiResponse = await client.GetAsync($"{taxCalculationUrl}/{year}");

        if (!apiResponse.IsSuccessStatusCode)
        {
            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                throw new YearNotSupportedException("Only years 2019, 2020 and 2021 are supported at the moment.");

            if (apiResponse.StatusCode == HttpStatusCode.InternalServerError)
                throw new ExternalApiFailedException("Something went wrong but don't give up! try again!!");
        }
            

        var taxBracketsJsonResult = await apiResponse.Content.ReadAsStringAsync();

        var taxBrackets = JsonSerializer.Deserialize<TaxBrackets>(taxBracketsJsonResult);

        return taxBrackets ?? new TaxBrackets();
    }
}