using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Books.Filter;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("books")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("filter")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseBooksJson), StatusCodes.Status200OK)]
    public IActionResult Filter(int pageNumber, string? title)
    {
        var useCase = new FilterBooksUseCase();
        
        var response = useCase.Execute(new RequestFilterBooksJson
        {
            PageNumber = pageNumber,
            Title = title,
        });

        if (response.Books.Count == 0)
        {
            return NoContent();
        }
        
        return Ok(response);
    }
}