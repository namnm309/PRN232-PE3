using System.Text.Json.Serialization;

namespace Q2.Models;

// DTO tác giả — thường lấy từ GET api/authors để fill dropdown / hiển thị tên
public class AuthorDto
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
}

// Một dòng sách ở list: dùng cho GET /api/Books hoặc /api/Books/Author/{id}
// JSON trả bookAuthors (camelCase) + mỗi phần tử có object Author lồng bên trong
public class BookListDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PublicationYear { get; set; }

    // API gửi "bookAuthors" chứ không phải PascalCase — map thủ công cho khớp
    [JsonPropertyName("bookAuthors")]
    public List<BookAuthorLinkDto>? BookAuthors { get; set; }
}

// Bản ghi nối sách–tác giả kiểu “có nested Author” — list page hay dùng cái này
public class BookAuthorLinkDto
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public AuthorDto? Author { get; set; } // null nếu API không embed author (hiếm nhưng cứ null-safe)
}

// Chi tiết 1 cuốn: GET /api/Books/{id} — cùng field bookAuthors nhưng shape khác list (flat hơn)
public class BookDetailDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PublicationYear { get; set; }

    [JsonPropertyName("bookAuthors")]
    public List<BookAuthorFlatDto>? BookAuthors { get; set; }
}

// Tác giả “dẹt” trên detail: không lồng object Author nữa, id/name/birthYear nằm chung một lớp
public class BookAuthorFlatDto
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
}
