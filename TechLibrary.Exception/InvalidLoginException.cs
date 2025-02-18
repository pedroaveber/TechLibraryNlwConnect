using System.Net;

namespace TechLibrary.Exception;

public class InvalidLoginException : TechLibraryException
{
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    public override List<string> GetErrorMessages() => ["Credenciais inválidas"];
}