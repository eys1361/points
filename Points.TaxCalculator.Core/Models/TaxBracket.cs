using System.Text.Json.Serialization;

namespace Points.TaxCalculator.Core.Models;

public class TaxBrackets
{
    [JsonPropertyName("tax_brackets")]
    public IList<TaxBracket> Brackets { get; set; } = new List<TaxBracket>();
}

public class TaxBracket
{
    [JsonPropertyName("max")]
    public decimal Max { get; set; }

    [JsonPropertyName("min")]
    public decimal Min { get; set; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
}