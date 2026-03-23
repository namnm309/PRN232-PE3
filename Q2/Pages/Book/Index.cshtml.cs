using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q2.Models;

namespace Q2.Pages.Book;

// Trang list sách — gọi API backend chứ không query DB trực tiếp ở đây (kiểu SPA-lite vậy đó)
public class IndexModel : PageModel
{
    // Inject factory chứ đừng `new HttpClient()` mỗi request — leak socket là chuyện thường ngày ở huyện
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Data bung ra cho Razor — mỗi cái là một list/DTO tương ứng
    public List<BookListDto> Books { get; set; } = new();
    public List<AuthorDto> Authors { get; set; } = new();
    // Author đang được chọn trong dropdown (0 = "tất cả", không filter)
    public int SelectedAuthorId { get; set; }

    public async Task OnGetAsync(int? authorId)
    {
        // Case insensitive vì đôi khi API trả camelCase mà C# model PascalCase — đỡ drama mismatch
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var client = _httpClientFactory.CreateClient();

        // Bước 1: lấy full list tác giả để fill dropdown filter
        var authorsUrl = Utilities.GetAbsoluteUrl("api/authors");
        var authorsResponse = await client.GetAsync(authorsUrl);
        authorsResponse.EnsureSuccessStatusCode();
        var authorsJson = await authorsResponse.Content.ReadAsStringAsync();
        Authors = JsonSerializer.Deserialize<List<AuthorDto>>(authorsJson, jsonOptions) ?? new();

        // Query string ?authorId=... — không có thì coi như 0 (xem hết)
        var filterId = authorId ?? 0;
        SelectedAuthorId = filterId;

        // 0 = show all sách; có id thì chỉ sách của ông đó thôi (API route khác nhau)
        string booksUrl;
        if (filterId == 0)
            booksUrl = Utilities.GetAbsoluteUrl("api/Books");
        else
            booksUrl = Utilities.GetAbsoluteUrl("api/Books/Author/" + filterId);

        var booksResponse = await client.GetAsync(booksUrl);
        booksResponse.EnsureSuccessStatusCode();
        var booksJson = await booksResponse.Content.ReadAsStringAsync();
        Books = JsonSerializer.Deserialize<List<BookListDto>>(booksJson, jsonOptions) ?? new();
    }
}
