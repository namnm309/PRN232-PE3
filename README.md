# Hướng dẫn chạy PE — Câu 1 (Q1) và Câu 2 (Q2)

Tài liệu này mô tả **từng bước** để cấu hình, build và chạy hai project trong solution `PE_PRN232_GivenSolution`.

---

## Yêu cầu chung

| Thành phần | Ghi chú |
|------------|---------|
| **.NET SDK** | 8.0 trở lên (`dotnet --version`) |
| **SQL Server** | Chỉ **bắt buộc cho Q1** (Entity Framework Core + SQL Server) |
| **IDE** | Visual Studio 2022 hoặc VS Code / Rider (tuỳ chọn) |

---

## Câu 1 — Q1 (Web API + SQL Server)

Q1 là **ASP.NET Core Web API** dùng **DbContext** (`LibraryContext`) kết nối SQL Server qua connection string `MyCnn` trong `appsettings.json`.

### Bước 1: Chuẩn bị database

1. Cài **SQL Server** (Express / Developer / LocalDB) và bật SQL Server Authentication nếu dùng `Uid`/`Pwd`.
2. Tạo **database** trống (hoặc dùng database đã có schema khớp đề bài / script giảng viên).
3. Cập nhật chuỗi kết nối trong [`Q1/appsettings.json`](Q1/appsettings.json) tại `ConnectionStrings:MyCnn`:
   - Đổi `Server=...` cho đúng instance SQL trên máy bạn (ví dụ `localhost`, `.\SQLEXPRESS`, `NAMPC\NAMPC`, …).
   - Đổi `Database=...` nếu cần.
   - Đổi `Uid` / `Pwd` (hoặc dùng Windows Authentication và chỉnh connection string cho phù hợp).

**Lưu ý:** Comment trong file nhắc **không đổi tên key** `MyCnn` nếu đề / code đang tham chiếu đúng key đó.

### Bước 2: Migration / tạo bảng (nếu đề yêu cầu)

- Nếu project có migration: mở terminal tại thư mục `Q1` và chạy (ví dụ):

  ```bash
  dotnet ef database update
  ```

  (Cần cài tool `dotnet-ef` nếu chưa có: `dotnet tool install --global dotnet-ef`.)

- Nếu database đã được import từ script `.sql`, bỏ qua bước này.

### Bước 3: Build project Q1

1. Đóng mọi instance **Q1** đang chạy (tránh lỗi khóa file `Q1.exe`).
2. Trong Visual Studio: chọn project **Q1** → **Build** → **Build Q1**, hoặc terminal:

   ```bash
   dotnet build Q1/Q1.csproj
   ```

### Bước 4: Chạy và kiểm tra API

1. Đặt **Startup Project** là **Q1** → **F5** hoặc **Ctrl+F5**.
2. Mặc định profile `http` trong [`Q1/Properties/launchSettings.json`](Q1/Properties/launchSettings.json) dùng cổng **`http://localhost:5230`** (kiểm tra lại nếu bạn đổi).
3. `Program.cs` redirect `/` sang **Swagger** — mở trình duyệt tại Swagger UI để gọi thử các controller (ví dụ `Books`, `Copies`).

### Bước 5: Gỡ lỗi thường gặp (Q1)

| Triệu chứng | Cách xử lý |
|-------------|------------|
| Không kết nối được SQL | Kiểm tra SQL Server đang chạy, firewall, đúng instance và tài khoản. |
| Lỗi migration | Đảm bảo connection string đúng và user có quyền tạo/sửa bảng. |
| MSB3021 / file bị khóa khi build | Dừng debug (**Shift+F5**), đóng `dotnet run`, hoặc Task Manager → kết thúc `Q1.exe`. |

---

## Câu 2 — Q2 (Razor Pages + HttpClient) và API mẫu GivenBooksAPI

Q2 là **Razor Pages** gọi REST API qua **HttpClient**. Base URL lấy từ [`Q2/appsettings.json`](Q2/appsettings.json) — key **`GivenAPIBaseUrl`** (không đổi tên key theo yêu cầu đề).

Trong solution có project **`GivenBooksAPI`**: API mẫu in-memory (sách / tác giả) chạy **`http://localhost:5100`**, khớp `GivenAPIBaseUrl` mặc định.

### Bước 1: Giải phóng cổng 5100

- Chỉ một process được nghe **5100**. Nếu đang chạy project **`givenAPI`** (Movies) hoặc API khác trên 5100, **dừng** nó trước khi chạy **GivenBooksAPI**.

### Bước 2: Chạy GivenBooksAPI (bắt buộc trước Q2)

1. Trong Visual Studio: đặt **Startup Project** là **GivenBooksAPI** (hoặc click phải project → **Debug** → **Start New Instance**).
2. Hoặc terminal:

   ```bash
   cd GivenBooksAPI
   dotnet run
   ```

3. Xác nhận log kiểu: `Now listening on: http://localhost:5100`.
4. (Tuỳ chọn) Mở Swagger: **http://localhost:5100/swagger** — thử `GET /api/authors`, `GET /api/Books`, v.v.

### Bước 3: Cấu hình Q2 (nếu đổi cổng API)

- Nếu bạn chạy API ở địa chỉ khác, sửa **giá trị** (không đổi tên key):

  ```json
  "GivenAPIBaseUrl": "http://localhost:5100"
  ```

### Bước 4: Build và chạy Q2

1. **Dừng** mọi instance Q2 đang chạy (tránh lỗi khóa `Q2.exe`).
2. Build:

   ```bash
   dotnet build Q2/Q2.csproj
   ```

3. Đặt **Startup Project** là **Q2** → **F5** / **Ctrl+F5**.
4. Profile `http` trong [`Q2/Properties/launchSettings.json`](Q2/Properties/launchSettings.json) thường là **`http://localhost:5177`** (đổi nếu bạn sửa file).
5. Trình duyệt mở `/` → redirect sang **`/Book`** (theo `Program.cs`).

### Bước 5: Kiểm tra chức năng Q2

1. Trang **Danh sách:** `/Book` — bảng sách, dropdown tác giả, **Filter**.
2. Trang **Chi tiết:** bấm **View Authors** hoặc vào `/Book/1`, `/Book/2`, …

### Bước 6: Gỡ lỗi thường gặp (Q2)

| Triệu chứng | Cách xử lý |
|-------------|------------|
| **404** khi gọi API | Đảm bảo **GivenBooksAPI** đang chạy **trước** Q2; đúng URL trong `GivenAPIBaseUrl`. |
| **HttpRequestException** tại `EnsureSuccessStatusCode` | API không chạy, sai URL, hoặc sai endpoint — mở Swagger trên 5100 để kiểm tra. |
| MSB3021 khi build Q2 | Dừng Q2 đang debug, hoặc Task Manager → kết thúc `Q2.exe`. |
| Hai API cùng cổng 5100 | Chỉ chạy **một** trong: `GivenBooksAPI` **hoặc** `givenAPI` (Movies). |

---

## Chạy nhanh cả hai câu (gợi ý)

1. **Terminal 1:** `cd GivenBooksAPI` → `dotnet run` (giữ cửa sổ mở).
2. **Terminal 2 hoặc Visual Studio:** chạy **Q2** → mở `http://localhost:5177/Book` (hoặc cổng trong `launchSettings` của Q2).
3. **Q1** độc lập: chỉ cần SQL Server + `appsettings` đúng → chạy Q1 → Swagger (cổng xem `Q1/Properties/launchSettings.json`).

---

## Cấu trúc thư mục liên quan (tham khảo)

```
PE_PRN232_GivenSolution/
├── Q1/                 ← Web API + EF + SQL Server
├── Q2/                 ← Razor Pages + HttpClient
├── GivenBooksAPI/      ← API mẫu Books/Authors (test Q2)
└── givenAPI/           ← API mẫu Movies (khác đề Q2; đừng nhầm cổng 5100)
```

Nếu cần chỉnh sửa code theo đề thi chính thức, luôn đối chiếu **wording đề** (route API, tên key config) với từng file tương ứng.
