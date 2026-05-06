using Lab4.DTOs;
using Lab4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lab4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService) { _bookService = bookService; }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto dto)
        {
            var newBook = await _bookService.AddBookAsync(dto);
            return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, newBook);
        }
    }
}