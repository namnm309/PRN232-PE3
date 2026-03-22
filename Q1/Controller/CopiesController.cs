using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1.Data;
using Q1.DTOs;
using Q1.Models;

namespace Q1.Controller
{
    [ApiController]
    [Route("api/copies")]
    public class CopiesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public CopiesController(LibraryContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<CopyReponse> AddCopy(CreateCopyRequest request)
        {
            //check tồn tại 
            var bookExits = _context.Books.FirstOrDefault(x => x.BookId == request.bookId);
            if (bookExits == null)
            {
                return NotFound();
            }

            var copy = new BookCopy
            {
                BookId = request.bookId,
                Status = "Available"
            };

            _context.BookCopies.Add(copy);
            _context.SaveChanges();
            return Ok(new CopyReponse
            {
                CopyId = copy.CopyId,
                BookId = copy.BookId
            });
        }

        [HttpDelete("{copyId:int}")]
        public async Task<IActionResult> DeleteCopy(int copyId)
        {
            var copy = await _context.BookCopies.FirstOrDefaultAsync(x => x.CopyId == copyId);
            if (copy == null)
            {
                return NotFound("No copy found with provided CopyId");
            }

            var isBorrowed = await _context.BorrowHistories
                .AnyAsync(x => x.CopyId == copyId && x.ReturnDate == null);

            if (isBorrowed)
            {
                return BadRequest("Cannot delete a copy that is currently borrowed.");
            }

            _context.BookCopies.Remove(copy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
