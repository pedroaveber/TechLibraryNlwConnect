using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Infrastructure.Security.Cryptography;
using TechLibrary.Api.Infrastructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase
{
    public ResponseRegistedUserJson Execute(RequestUserJson request)
    {
        var dbContext = new TechLibraryDBContext();
        
        Validate(request, dbContext);

        var cryptography = new BCryptAlgorithm();

        var entity = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = cryptography.HashPassword(request.Password),
        };
        
        dbContext.Users.Add(entity);
        dbContext.SaveChanges();

        var tokenGenerator = new JwtTokenGenerator();
        
        return new ResponseRegistedUserJson
        {
            Name = entity.Name,
            AccessToken = tokenGenerator.Generate(entity),
        };
    }

    private static void Validate(RequestUserJson request, TechLibraryDBContext dbContext)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        var userWithSameEmail = dbContext.Users.Any(user => user.Email.Equals(request.Email));

        if (userWithSameEmail)
        {
            result.Errors.Add(new ValidationFailure("Email", "E-mail já registrado na plataforma."));
        }

        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ErrorOnValidateException(errorMessages);
    }
}