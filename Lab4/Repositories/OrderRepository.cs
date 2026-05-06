using Lab4.Data;
using Lab4.Models;
using System.Threading.Tasks;

namespace Lab4.Repositories
{
    public interface IOrderRepository 
    {
        Task<Order> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }

    public class OrderRepository : IOrderRepository 
    {
        private readonly LibraryDbContext _context;
        public OrderRepository(LibraryDbContext context) { _context = context; }
        public async Task<Order> GetByIdAsync(int id) => await _context.Orders.FindAsync(id);
        public async Task AddAsync(Order order) { await _context.Orders.AddAsync(order); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Order order) { _context.Orders.Update(order); await _context.SaveChangesAsync(); }
    }
}