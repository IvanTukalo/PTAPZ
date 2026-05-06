using Lab4.Data;
using Lab4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab4.Repositories
{
    public interface IBookRepository 
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
    }

    public class BookRepository : IBookRepository 
    {
        private readonly LibraryDbContext _context;
        public BookRepository(LibraryDbContext context) { _context = context; }
        
        public async Task<IEnumerable<Book>> GetAllAsync() => await _context.Books.ToListAsync();
        public async Task<Book> GetByIdAsync(int id) => await _context.Books.FindAsync(id);
        public async Task AddAsync(Book book) { await _context.Books.AddAsync(book); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Book book) { _context.Books.Update(book); await _context.SaveChangesAsync(); }
    }
}