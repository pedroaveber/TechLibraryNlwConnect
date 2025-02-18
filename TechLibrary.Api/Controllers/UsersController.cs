using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Users.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegistedUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
    public IActionResult Register(RequestUserJson request)
    {
            var useCase = new RegisterUserUseCase();
            var response = useCase.Execute(request);
            return Created(string.Empty, response);
    }
}