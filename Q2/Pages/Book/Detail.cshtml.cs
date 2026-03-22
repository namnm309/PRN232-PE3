using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q2.Models;

namespace Q2.Pages.Book;

public class DetailModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DetailModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public BookDetailDto Book { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int bookId)
    {
        var client = _httpClientFactory.CreateClient();
        var url = Utilities.GetAbsoluteUrl("api/Books/" + bookId);
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return NotFound();

        var json = await response.Content.ReadAsStringAsync();
        var book = JsonSerializer.Deserialize<BookDetailDto>(json, JsonSerializerDefaults.Options);
        if (book == null)
            return NotFound();

        Book = book;
        return Page();
    }
}
