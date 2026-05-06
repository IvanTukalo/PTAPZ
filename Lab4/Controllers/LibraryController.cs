using Lab4.DTOs;
using Lab4.Models;
using Lab4.Repositories;
using Lab4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers
{
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryRepository _repo;
        private readonly LibraryBusinessLogic _logic;

        public LibraryController(LibraryRepository repo, LibraryBusinessLogic logic)
        {
            _repo = repo; _logic = logic;
        }

        // --- BOOKS ---
        [HttpGet("api/books")]
        public async Task<IActionResult> GetBooks([FromQuery] string author, [FromQuery] bool? is_digitized)
        {
            var books = await _repo.GetBooksAsync();
            if (!string.IsNullOrEmpty(author)) books = books.Where(b => b.Authors.Any(a => a.Contains(author))).ToList();
            if (is_digitized.HasValue) books = books.Where(b => b.IsDigitized == is_digitized.Value).ToList();
            return Ok(books);
        }

        [HttpPost("api/books")]
        public async Task<IActionResult> AddBook([FromBody] BookInputDto dto)
        {
            var book = new Book { Title = dto.Title, Authors = dto.Authors, Publisher = dto.Publisher, Year = dto.Year, FreeCopies = dto.FreeCopies, IsDigitized = dto.IsDigitized };
            await _repo.AddBookAsync(book);
            return StatusCode(201, book);
        }

        [HttpGet("api/books/{book_id}")]
        public async Task<IActionResult> GetBook(int book_id) => Ok(await _repo.GetBookAsync(book_id));

        [HttpDelete("api/books/{book_id}")]
        public async Task<IActionResult> DeleteBook(int book_id)
        {
            var b = await _repo.GetBookAsync(book_id);
            if (b != null) await _repo.DeleteBookAsync(b);
            return NoContent();
        }

        [HttpGet("api/books/{book_id}/content")]
        public async Task<IActionResult> ReadDigitalBook(int book_id)
        {
            var b = await _repo.GetBookAsync(book_id);
            if (b == null || !b.IsDigitized) return StatusCode(403, new { message = "Книга не відцифрована." });
            return Ok(new { content = "Текст книги..." }); // Мок контенту
        }

        // --- AUTH & USERS ---
        [HttpPost("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _repo.GetUserByPhoneAsync(dto.Phone);
            if (user == null || user.Password != dto.Password) return Unauthorized();
            return Ok(new { access_token = "mock_jwt_token_123" });
        }

        [HttpPost("api/users")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto dto)
        {
            var u = new User { FullName = dto.FullName, Phone = dto.Phone, Password = "123" }; // Пароль моковий для лаби
            await _repo.AddUserAsync(u);
            return StatusCode(201, u);
        }

        [HttpDelete("api/users/{user_id}")]
        public async Task<IActionResult> DeleteUser(int user_id)
        {
            try { await _logic.DeleteUserSafeAsync(user_id); return NoContent(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // --- ORDERS ---
        [HttpPost("api/orders")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            try { return StatusCode(201, await _logic.BookAsync(dto)); }
            catch (Exception ex) { return UnprocessableEntity(new { message = ex.Message }); } // 422 згідно Swagger
        }

        [HttpDelete("api/orders/{order_id}")]
        public async Task<IActionResult> CancelOrder(int order_id)
        {
            await _logic.CancelOrderAsync(order_id);
            return NoContent();
        }

        [HttpPatch("api/orders/{order_id}")]
        public async Task<IActionResult> UpdateOrderStatus(int order_id, [FromBody] UpdateOrderStatusDto dto)
        {
            var order = await _repo.GetOrderAsync(order_id);
            if (dto.Status == "returned")
            {
                var book = await _repo.GetBookAsync(order.BookId);
                book.FreeCopies += 1; book.BookedCopies -= 1;
                await _repo.UpdateBookAsync(book);
            }
            order.Status = dto.Status;
            await _repo.UpdateOrderAsync(order);
            return Ok(order);
        }
    }
}
