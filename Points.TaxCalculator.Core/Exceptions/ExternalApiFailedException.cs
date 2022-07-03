namespace Points.TaxCalculator.Core.Exceptions;

public class ExternalApiFailedException : Exception
{
    public ExternalApiFailedException()
    {
    }

    public ExternalApiFailedException(string message) : base(message)
    {
    }
}