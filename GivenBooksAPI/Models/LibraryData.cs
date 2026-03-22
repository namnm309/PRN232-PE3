namespace GivenBooksAPI.Models;

/// <summary>In-memory dummy library (Figure 1 sample) for Q2 testing.</summary>
public static class LibraryData
{
    public static IReadOnlyList<Author> Authors { get; }
    public static IReadOnlyList<Book> Books { get; }

    static LibraryData()
    {
        var authors = new List<Author>
        {
            new() { AuthorId = 1, Name = "Nguyen Nhat Anh", BirthYear = 1955 },
            new() { AuthorId = 2, Name = "Stephen Hawking", BirthYear = 1942 },
            new() { AuthorId = 3, Name = "Agatha Christie", BirthYear = 1890 },
            new() { AuthorId = 4, Name = "Dale Carnegie", BirthYear = 1888 },
            new() { AuthorId = 5, Name = "Yuval Noah Harari", BirthYear = 1976 },
        };

        var books = new List<Book>
        {
            new() { BookId = 1, Title = "Blue Eyes", PublicationYear = 1990 },
            new() { BookId = 2, Title = "Promise of an Icon", PublicationYear = 1995 },
            new() { BookId = 3, Title = "Advice from a Physicist", PublicationYear = 2018 },
            new() { BookId = 4, Title = "A New Conscience", PublicationYear = 2015 },
            new() { BookId = 5, Title = "The Mysterious Woman", PublicationYear = 1930 },
        };

        void Link(int bookId, int authorId)
        {
            var book = books.First(b => b.BookId == bookId);
            var author = authors.First(a => a.AuthorId == authorId);
            var link = new BookAuthor { Book = book, Author = author, BookId = bookId, AuthorId = authorId };
            book.BookAuthors.Add(link);
            author.BookAuthors.Add(link);
        }

        Link(1, 1);
        Link(1, 4);
        Link(2, 1);
        Link(2, 3);
        Link(3, 2);
        Link(4, 5);
        Link(5, 3);

        Authors = authors;
        Books = books;
    }
}
