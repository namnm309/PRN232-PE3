using Microsoft.AspNetCore.Mvc;
using Q1.Data;
using Q1.DTOs;
using Q1.Models;
using Microsoft.EntityFrameworkCore;

namespace Q1.Controller
{
    [ApiController]//mở framework sp
    [Route("api")]//define đường dẫn theo đề 
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        
        private IQueryable<AvailableBook> BookQuery()
        {
            //Đề yêu cầu là available book là book có return date khác null 
            var borrowedBook = _context.BorrowHistories
                .Where(x => x.ReturnDate == null)
                .Select(x => x.CopyId);

            return _context.Books
                .OrderBy(x => x.BookId) //sắp xếp theo thứ tự cho chắc 
                .Select(x => new AvailableBook
                {
                    BookId = x.BookId,
                    Title = x.Title,
                    PublicationYear = x.PublicationYear,
                    AvailableCopines = _context.BookCopies.Count(c => c.BookId == x.BookId && !borrowedBook.Contains(c.CopyId))
                });
        }

        [HttpGet("books")] //kĩ cái này ko bay màu , đặt tên giống đề bài ví dụ /api/books ( cái api đặt ở đầu rồi ấy Route ấy ) 
        public ActionResult<List<AvailableBook>> GetBooks()
        {
            var books=BookQuery().ToList();
            return Ok(books);
        }

        [HttpGet("paginationbooks")]
        public ActionResult<BookPaginationResponse> GetBookPagination([FromQuery] int page = 1
                                                                    , [FromQuery] int pageSize = 10) { 
            //Validate theo đề 
            //Nếu trang và tổng phần tử của trang âm hoặc ko xác định trả lỗi 400 
            if (page <= 0 && pageSize <= 0 )
            {
                return BadRequest("page và pageSize phải là số nguyên dương");
            }

            //Công thức phân trang là 

            var totalBooks = _context.Books.Count();
            var totalPages = totalBooks == 0 ? 0 : (int)Math.Ceiling(totalBooks / (double)pageSize);

            List<AvailableBook> books;

            if (totalBooks > 0 && page > totalPages)
            {
                books = [];
            } else
            {
                books = BookQuery()
                        .Skip((page-1)*pageSize) //skip trang trước 
                        .Take(pageSize)
                        .ToList();
            }

            var reponse = new BookPaginationResponse
            {
                TotalBooks = totalBooks,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Data = books
            };

            return Ok(reponse);
        
        }
    }
}
