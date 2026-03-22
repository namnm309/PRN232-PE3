using System.Text.Json.Serialization;

namespace Q2.Models;

public class AuthorDto
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
}

/// <summary>Book row from GET /api/Books or GET /api/Books/Author/{id} — JSON uses bookAuthors + nested author.</summary>
public class BookListDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PublicationYear { get; set; }

    [JsonPropertyName("bookAuthors")]
    public List<BookAuthorLinkDto>? BookAuthors { get; set; }
}

public class BookAuthorLinkDto
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public AuthorDto? Author { get; set; }
}

/// <summary>Book from GET /api/Books/{id} — bookAuthors items are flat authorId, name, birthYear.</summary>
public class BookDetailDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PublicationYear { get; set; }

    [JsonPropertyName("bookAuthors")]
    public List<BookAuthorFlatDto>? BookAuthors { get; set; }
}

public class BookAuthorFlatDto
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
}
