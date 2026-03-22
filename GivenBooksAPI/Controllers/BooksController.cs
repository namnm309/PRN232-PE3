using GivenBooksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GivenBooksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet]
    [Route("/api/[controller]")]
    public ActionResult<IEnumerable<Book>> GetBooks()
    {
        return Ok(LibraryData.Books);
    }

    [Route("/api/[controller]/Author/{authorId}")]
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetBooksByAuthor(int? authorId)
    {
        if (authorId.HasValue && authorId.Value != 0)
        {
            var list = LibraryData.Books
                .Where(b => b.BookAuthors.Any(a => a.AuthorId == authorId.Value))
                .ToList();
            return Ok(list);
        }

        return Ok(LibraryData.Books);
    }

    [HttpGet("{bookId:int}")]
    public ActionResult GetBook(int bookId)
    {
        var book = LibraryData.Books.FirstOrDefault(b => b.BookId == bookId);
        if (book == null)
            return NotFound();

        var result = new
        {
            book.BookId,
            book.Title,
            book.PublicationYear,
            bookAuthors = book.BookAuthors.Select(ba => new
            {
                ba.AuthorId,
                Name = ba.Author.Name,
                BirthYear = ba.Author.BirthYear
            }).ToList()
        };
        return Ok(result);
    }
}
