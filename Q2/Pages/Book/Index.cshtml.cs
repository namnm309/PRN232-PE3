using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Q2.Models;

namespace Q2.Pages.Book;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public List<BookListDto> Books { get; set; } = new();
    public List<AuthorDto> Authors { get; set; } = new();
    public int SelectedAuthorId { get; set; }

    public async Task OnGetAsync(int? authorId)
    {
        var client = _httpClientFactory.CreateClient();

        var authorsUrl = Utilities.GetAbsoluteUrl("api/authors");
        var authorsResponse = await client.GetAsync(authorsUrl);
        authorsResponse.EnsureSuccessStatusCode();
        var authorsJson = await authorsResponse.Content.ReadAsStringAsync();
        Authors = JsonSerializer.Deserialize<List<AuthorDto>>(authorsJson, JsonSerializerDefaults.Options) ?? new();

        var filterId = authorId ?? 0;
        SelectedAuthorId = filterId;

        string booksUrl;
        if (filterId == 0)
            booksUrl = Utilities.GetAbsoluteUrl("api/Books");
        else
            booksUrl = Utilities.GetAbsoluteUrl("api/Books/Author/" + filterId);

        var booksResponse = await client.GetAsync(booksUrl);
        booksResponse.EnsureSuccessStatusCode();
        var booksJson = await booksResponse.Content.ReadAsStringAsync();
        Books = JsonSerializer.Deserialize<List<BookListDto>>(booksJson, JsonSerializerDefaults.Options) ?? new();
    }
}
