using System.Net;

namespace TechLibrary.Exception;

public class InvalidLoginException : TechLibraryException
{
    public InvalidLoginException() : base("Invalid username or password.")
    {
        
    }
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    public override List<string> GetErrorMessages() => ["Credenciais inválidas"];
}