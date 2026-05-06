using Lab4.Data;
using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Repositories
{
    public class LibraryRepository
    {
        public readonly LibraryDbContext _context;
        public LibraryRepository(LibraryDbContext context) { _context = context; }
        
        // Books
        public async Task<List<Book>> GetBooksAsync() => await _context.Books.ToListAsync();
        public async Task<Book> GetBookAsync(int id) => await _context.Books.FindAsync(id);
        public async Task AddBookAsync(Book b) { _context.Books.Add(b); await _context.SaveChangesAsync(); }
        public async Task UpdateBookAsync(Book b) { _context.Books.Update(b); await _context.SaveChangesAsync(); }
        public async Task DeleteBookAsync(Book b) { _context.Books.Remove(b); await _context.SaveChangesAsync(); }

        // Users
        public async Task<User> GetUserByPhoneAsync(string phone) => await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        public async Task<User> GetUserAsync(int id) => await _context.Users.FindAsync(id);
        public async Task AddUserAsync(User u) { _context.Users.Add(u); await _context.SaveChangesAsync(); }
        public async Task DeleteUserAsync(User u) { _context.Users.Remove(u); await _context.SaveChangesAsync(); }

        // Orders
        public async Task<Order> GetOrderAsync(int id) => await _context.Orders.FindAsync(id);
        public async Task<List<Order>> GetUserOrdersAsync(int userId) => await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        public async Task AddOrderAsync(Order o) { _context.Orders.Add(o); await _context.SaveChangesAsync(); }
        public async Task UpdateOrderAsync(Order o) { _context.Orders.Update(o); await _context.SaveChangesAsync(); }
        public async Task DeleteOrderAsync(Order o) { _context.Orders.Remove(o); await _context.SaveChangesAsync(); }
    }
}
