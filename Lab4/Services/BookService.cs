using Lab4.DTOs;
using Lab4.Models;
using Lab4.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab4.Services
{
    public interface IBookService 
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> AddBookAsync(CreateBookDto dto);
    }

    public class BookService : IBookService 
    {
        private readonly IBookRepository _repository;
        public BookService(IBookRepository repository) { _repository = repository; }

        public async Task<IEnumerable<Book>> GetAllBooksAsync() => await _repository.GetAllAsync();

        public async Task<Book> AddBookAsync(CreateBookDto dto)
        {
            var book = new Book { Title = dto.Title, Author = dto.Author, AvailableCopies = dto.AvailableCopies };
            await _repository.AddAsync(book);
            return book;
        }
    }
}