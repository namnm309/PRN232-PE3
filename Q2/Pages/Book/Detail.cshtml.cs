using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q2.Models;

namespace Q2.Pages.Book;

// Trang chi tiết 1 cuốn — có kèm bảng tác giả (BookAuthors) nếu API trả về
public class DetailModel : PageModel
{
    // Cùng vibe Index: dùng factory gọi API, khỏi tự bơi HttpClient
    private readonly IHttpClientFactory _httpClientFactory;

    public DetailModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // null! vì Razor đọc sau khi OnGet chạy xong — lúc render là đã có data rồi (hoặc đã 404)
    public BookDetailDto Book { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int bookId)
    {
        var client = _httpClientFactory.CreateClient();
        var url = Utilities.GetAbsoluteUrl("api/Books/" + bookId);
        var response = await client.GetAsync(url);

        // 404 / lỗi server — không cố render view với Book null (crash mệt lắm)
        if (!response.IsSuccessStatusCode)
            return NotFound();

        var json = await response.Content.ReadAsStringAsync();
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var book = JsonSerializer.Deserialize<BookDetailDto>(json, jsonOptions);

        // JSON rỗng / parse fail — coi như không có sách đó
        if (book == null)
            return NotFound();

        Book = book;
        return Page();
    }
}
