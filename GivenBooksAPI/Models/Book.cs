namespace GivenBooksAPI.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public List<BookAuthor> BookAuthors { get; set; } = new();
}
