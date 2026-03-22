# Hướng dẫn làm bài — Câu 1 (Web API) và Câu 2 (Razor + HttpClient)

Tài liệu này mô tả **trình tự code** thường gặp trong đề PE PRN232 (API + client), để bạn đối chiếu với **wording đề** chứ không phải hướng dẫn cài đặt môi trường hay lệnh `dotnet run`.

---

## Câu 1 — ASP.NET Core Web API + Entity Framework + SQL Server

### 1. Khởi tạo pipeline (`Program.cs`)

1. Đăng ký **DbContext** với connection string từ cấu hình (ví dụ `builder.Configuration.GetConnectionString("MyCnn")`) — **giữ đúng tên key** nếu đề/ghi chú trong solution quy định.
2. `AddControllers()`; nếu đề yêu cầu **XML** thêm `AddXmlSerializerFormatters()`.
3. (Tuỳ đề) `AddEndpointsApiExplorer()` + `AddSwaggerGen()` để test nhanh.
4. Sau `var app = builder.Build()`: bật Swagger nếu dùng; **`app.MapControllers()`** — không quên thì controller không ánh xạ.
5. Không chèn middleware thừa làm thay đổi thứ tự trước `MapControllers` nếu không cần.

### 2. Cấu hình kết nối CSDL (`appsettings.json`)

1. Thêm `ConnectionStrings` với **đúng tên** mà code đọc (ví dụ `MyCnn`).
2. Chuỗi kết nối trỏ tới SQL Server instance và database theo **script/đề** (tên DB, login, `TrustServerCertificate` nếu dev).

### 3. Tầng dữ liệu

1. **Models**: class map bảng (`Book`, `Author`, `BookCopy`, …) — property + khóa, quan hệ `virtual` nếu dùng Include/navigation.
2. **DbContext**: `DbSet<>` cho từng entity; `OnModelCreating` nếu đề có ràng buộc đặc biệt hoặc tên bảng khác.
3. **Migration** (nếu đề cho phép scaffold): `Add-Migration` / `dotnet ef migrations add` rồi `Update-Database` — hoặc chạy script `.sql` có sẵn thay cho migration.

### 4. Controller và route

1. Tạo class kế thừa `ControllerBase`, gắn `[ApiController]`.
2. **`[Route("api")]`** hoặc `[Route("api/[controller]")]` — **bám sát đề**: ví dụ đề ghi `/api/books` thì route template + `[HttpGet("books")]` phải ra đúng đường dẫn (tránh nhầm `api` lặp hai lần).
3. Inject `LibraryContext` (hoặc service) qua constructor.
4. Mỗi action: `[HttpGet("...")]`, `[HttpPost("...")]`, `[FromQuery]`, `[FromBody]` đúng chỗ theo đề.

### 5. Logic nghiệp vụ và DTO

1. Tách **DTO** (request/response) khác entity nếu đề không trả nguyên bảng.
2. Viết query bằng **LINQ** trên `_context` (Where, Select, Include, Count, …) đúng định nghĩa nghiệp vụ (ví dụ “available book”, “đang mượn”, phân trang).
3. Trả về `Ok(data)`, `BadRequest()`, `NotFound()`, `StatusCode(400, …)` theo từng trường hợp đề mô tả.

### 6. Kiểm tra trước khi nộp

1. Đối chiếu **từng URL** trong đề với `[Route]` + `[HttpGet]`/`[HttpPost]`.
2. Kiểm tra kiểu trả về và tên field JSON (PascalCase mặc định hoặc cấu hình camelCase nếu đề yêu cầu).

---

## Câu 2 — Razor Pages (hoặc MVC) + HttpClient gọi API có sẵn

### 1. Cấu hình base URL API (`appsettings.json`)

1. Thêm key **`GivenAPIBaseUrl`** (hoặc đúng key đề giao — **không đổi tên** nếu đề cấm).
2. Giá trị là URL gốc API (ví dụ `http://localhost:5100`), **không** gắn `/api/...` ở đây.

### 2. `Program.cs` (thường có đoạn cố định)

1. Gọi **`Utilities.Initialize(builder.Configuration)`** — giữ nguyên nếu đề/template yêu cầu.
2. `builder.Services.AddRazorPages()` (hoặc MVC tương ứng).
3. `builder.Services.AddHttpClient()` — bắt buộc dùng HttpClient theo đề.
4. `app.MapRazorPages()`; (tuỳ chọn) redirect `/` → `/Book`.

### 3. Ghép URL gọi API (`Utilities` hoặc trực tiếp trong PageModel)

1. Đọc base URL từ config.
2. Nếu đề yêu cầu **nối chuỗi bằng `+`**: dùng `baseUrl + "/api/..." + id` (không dùng kiểu ghép “ẩn” khác nếu đề cấm).
3. Endpoint path **khớp API thật** (ví dụ `api/Books` vs `api/books/author`) — đối chiếu Swagger project **GivenAPIs** khi thi, không đoán theo tóm tắt ngắn.

### 4. DTO khớp JSON API

1. Tạo class property trùng **tên field JSON** (thường camelCase từ ASP.NET Core API).
2. Dùng `JsonSerializerOptions.PropertyNameCaseInsensitive = true` để tránh lệch hoa thường.
3. Nếu API trả **`bookAuthors`** thay vì `authors`, thêm `[JsonPropertyName("bookAuthors")]` hoặc property tên đúng.

### 5. Trang danh sách (`/Book`)

1. File ví dụ `Pages/Book/Index.cshtml` + `Index.cshtml.cs`.
2. `@page "/Book"` — URL **không** được thành `/Book/Index` nếu đề bắt đúng `/Book`.
3. **PageModel**: inject `IHttpClientFactory`; trong `OnGetAsync(int? authorId)`:
   - Gọi GET danh sách tác giả cho dropdown.
   - Gọi GET danh sách sách (tất cả hoặc theo `authorId` từ query) — form filter dùng `method="get"` + `name="authorId"` khớp tham số.
4. Deserialize bằng `System.Text.Json` (không bắt buộc Refit/RestSharp nếu đề cấm thư viện ngoài).

### 6. Trang chi tiết (`/Book/{BookId}`)

1. `@page "/Book/{bookId:int}"` để URL dạng `/Book/1`.
2. `OnGetAsync(int bookId)`: GET `api/.../books/{bookId}` (đúng path API); `NotFound()` nếu 404.
3. View hiển thị thông tin sách + bảng tác giả; DTO chi tiết có thể khác list (ví dụ `bookAuthors` phẳng).

### 7. Quy tắc HTML `id` (thường auto chấm)

1. Đối chiếu **bảng tóm tắt đề** từng thẻ: `td_{columnName}_{bookId}`, `div_{bookId}_{authorId}`, `a_{bookId}`, `sl_authors`, `op_0`, `op_{authorId}`, `bt_filter`, `span_...`, v.v.
2. **Mọi** phần tử input/output có `id` nếu đề nói rõ — kể cả `label`, `form`, `th` nếu yêu cầu.
3. Link trong bảng trỏ `href="/Book/@bookId"` đúng route Razor.

### 8. API mẫu trong solution (`GivenBooksAPI`)

- Project **GivenBooksAPI** chỉ để **bắt chước** shape endpoint/JSON khi ôn tập; khi thi, bám **project GivenAPIs** thật của giảng viên.

---

## Checklist nhanh trước khi nộp

| Hạng mục | Câu 1 | Câu 2 |
|----------|-------|-------|
| Route API đúng đề | ✓ | ✓ (path gọi từ client) |
| DB / HttpClient đúng yêu cầu | ✓ | ✓ |
| DTO / JSON khớp | ✓ | ✓ |
| `id` HTML đủ và đúng pattern | — | ✓ |

---

## Cấu trúc thư mục tham chiếu trong repo này

```
PE_PRN232_GivenSolution/
├── Q1/            Web API + EF (ví dụ BooksController, CopiesController)
├── Q2/            Razor Pages + HttpClient + Utilities
├── GivenBooksAPI/ API mẫu Books/Authors (ôn Q2)
└── givenAPI/      API mẫu Movies (không dùng cho Q2)
```

Khi làm bài thi, luôn **ưu tiên đề in phông** và **Swagger/API gốc** hơn mọi tóm tắt ngắn.
