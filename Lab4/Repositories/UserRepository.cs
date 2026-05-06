using Lab4.Data;
using Lab4.Models;
using System.Threading.Tasks;

namespace Lab4.Repositories
{
    public interface IUserRepository 
    {
        Task AddAsync(User user);
    }

    public class UserRepository : IUserRepository 
    {
        private readonly LibraryDbContext _context;
        public UserRepository(LibraryDbContext context) { _context = context; }
        public async Task AddAsync(User user) { await _context.Users.AddAsync(user); await _context.SaveChangesAsync(); }
    }
}