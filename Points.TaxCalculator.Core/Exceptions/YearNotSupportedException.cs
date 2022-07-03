namespace Points.TaxCalculator.Core.Exceptions
{
    public class YearNotSupportedException : Exception
    {
        public YearNotSupportedException()
        {
        }

        public YearNotSupportedException(string message):base(message)
        {
        }
    }

    
}
