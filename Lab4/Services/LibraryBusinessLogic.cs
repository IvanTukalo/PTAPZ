using Lab4.DTOs;
using Lab4.Models;
using Lab4.Repositories;

namespace Lab4.Services
{
    public class LibraryBusinessLogic
    {
        private readonly LibraryRepository _repo;
        public LibraryBusinessLogic(LibraryRepository repo) { _repo = repo; }

        // БРОНЮВАННЯ (QA: Reliability)
        public async Task<Order> BookAsync(CreateOrderDto dto)
        {
            var book = await _repo.GetBookAsync(dto.BookId);
            if (book == null || book.FreeCopies <= 0) throw new Exception("Немає вільних примірників.");

            book.FreeCopies -= 1;
            book.BookedCopies += 1;
            await _repo.UpdateBookAsync(book);

            var order = new Order {
                UserId = dto.UserId, BookId = dto.BookId,
                IssueDate = DateTime.UtcNow, ReturnDate = DateTime.UtcNow.AddDays(dto.DurationDays),
                Status = "booked"
            };
            await _repo.AddOrderAsync(order);
            return order;
        }

        // СКАСУВАННЯ БРОНЮВАННЯ
        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _repo.GetOrderAsync(orderId);
            if (order == null || order.Status == "returned") return;

            var book = await _repo.GetBookAsync(order.BookId);
            book.FreeCopies += 1;
            book.BookedCopies -= 1;
            await _repo.UpdateBookAsync(book);
            
            await _repo.DeleteOrderAsync(order);
        }

        // ВИДАЛЕННЯ АКАУНТА З ПЕРЕВІРКОЮ
        public async Task DeleteUserSafeAsync(int userId)
        {
            var orders = await _repo.GetUserOrdersAsync(userId);
            if (orders.Any(o => o.Status != "returned")) throw new Exception("Видалення неможливе через наявність активних бронювань.");
            
            var user = await _repo.GetUserAsync(userId);
            if (user != null) await _repo.DeleteUserAsync(user);
        }
    }
}
