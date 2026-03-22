namespace GivenBooksAPI.Models;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public List<BookAuthor> BookAuthors { get; set; } = new();
}
