using GivenBooksAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GivenBooksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    [HttpGet]
    [Route("/api/[controller]")]
    public ActionResult<IEnumerable<Author>> GetAuthors()
    {
        return Ok(LibraryData.Authors);
    }
}
