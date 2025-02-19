using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.UseCases.Books.Filter;

public class FilterBooksUseCase
{
    private const int PageSize = 10;
    
    public ResponseBooksJson Execute(RequestFilterBooksJson request)
    {
        var dbContext = new TechLibraryDbContext();

        var query = dbContext.Books.AsQueryable();
        
        if (string.IsNullOrWhiteSpace(request.Title) == false)
        {
            query = query.Where(book => book.Title.Contains(request.Title));
        }

        var books = query
            .OrderBy(book => book.Title)
            .ThenBy(book => book.Author)
            .Skip((request.PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();
        

        var totalCount = string.IsNullOrWhiteSpace(request.Title) ?
            dbContext.Books.Count() :
            dbContext.Books.Count(book => book.Title.Contains(request.Title));
        
        return new ResponseBooksJson
        {
            Pagination = new ResponsePaginationJson
            {
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
            },
            Books = books.Select(book => new ResponseBookJson
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
            }).ToList()
        };
    }
}