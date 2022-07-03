namespace Points.TaxCalculator.API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad request.",
                401 => "Not Authorized",
                404 => "Resource not found",
                500 =>
                    "Something went wrong at our side.",
                _ => null
            };
        }
    }
}
