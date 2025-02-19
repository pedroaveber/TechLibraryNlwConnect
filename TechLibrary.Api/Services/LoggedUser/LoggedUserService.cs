using System.IdentityModel.Tokens.Jwt;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Exception;

namespace TechLibrary.Api.Services.LoggedUser;

public class LoggedUserService
{
    private readonly HttpContext _httpContext;
    public LoggedUserService(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public User GetUser(TechLibraryDbContext dbContext)
    {
        var authentication = _httpContext.Request.Headers.Authorization.ToString();
        var token = authentication.Split(" ")[1];

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken
            .Claims
            .First(claim => claim.Type == JwtRegisteredClaimNames.Sub)
            .Value;
        
        var userId = Guid.Parse(identifier);
        
        var user = dbContext.Users.FirstOrDefault(user => user.Id == userId);

        if (user is null)
        {
            throw new NotFoundException("Usuário não localizado.");
        }
        
        return user;
    }
}