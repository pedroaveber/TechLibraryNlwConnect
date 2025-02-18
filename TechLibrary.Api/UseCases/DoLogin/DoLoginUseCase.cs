using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Infrastructure.Security.Cryptography;
using TechLibrary.Api.Infrastructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.DoLogin;

public class DoLoginUseCase
{
    public ResponseRegistedUserJson Execute(RequestLoginJson request)
    {
        var dbContext = new TechLibraryDBContext();
        var user = dbContext.Users.FirstOrDefault(user => user.Email == request.Email);

        if (user is null) throw new InvalidLoginException();

        var cryptography = new BCryptAlgorithm();
        var doesPasswordMatch = cryptography.Verify(request.Password, user);
        
        if (doesPasswordMatch == false) throw new InvalidLoginException();

        var tokenGenerator = new JwtTokenGenerator();
        
        return new ResponseRegistedUserJson
        {
            Name = user.Name,
            AccessToken = tokenGenerator.Generate(user),
        };
    }
}