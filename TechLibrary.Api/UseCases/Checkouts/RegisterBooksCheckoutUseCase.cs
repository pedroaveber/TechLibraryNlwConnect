using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts;

public class RegisterBooksCheckoutUseCase
{
    private const int MaxLoanDays = 7;
    private readonly LoggedUserService _loggedUserService;

    public RegisterBooksCheckoutUseCase(LoggedUserService loggerUserService)
    {
        _loggedUserService = loggerUserService;
    }
    public void Execute(Guid bookId)
    {
        var dbContext = new TechLibraryDbContext();
        Validate(bookId, dbContext);

        var user = _loggedUserService.GetUser(dbContext);

        var checkout = new Checkout
        {
            BookId = bookId,
            UserId = user.Id,
            ReturnedDate = null,
            ExpectedReturnDate = DateTime.UtcNow.AddDays(MaxLoanDays)
        };

        dbContext.Checkouts.Add(checkout);
        dbContext.SaveChanges();
    }

    private void Validate(Guid bookId, TechLibraryDbContext dbContext)
    {
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);

        if (book is null)
            throw new NotFoundException("Livro não localizado.");

        var amountBookNotReturned = dbContext
            .Checkouts.
            Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);

        if (amountBookNotReturned == book.Amount)
            throw new NotFoundException("Livro não localizado.");
    }
}