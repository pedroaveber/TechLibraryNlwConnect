using System.Net;

namespace TechLibrary.Exception;

public class ErrorOnValidateException : TechLibraryException
{
    private readonly List<string> _errors;
    public ErrorOnValidateException(List<string> errorMessages)
    {
        _errors = errorMessages;
    }

    public override List<string> GetErrorMessages() => _errors;
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}