namespace DeveloperStore.Services.Services;

public class CustomException : Exception
{
    public string ErrorType { get; }
    public string Detail { get; }

    public CustomException(string errorType, string message, string detail)
        : base(message)
    {
        ErrorType = errorType;
        Detail = detail;
    }

    public object ToJson()
    {
        return new
        {
            type = ErrorType,
            error = Message,
            detail = Detail
        };
    }
}