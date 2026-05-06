using Lab4.DTOs;
using Lab4.Models;
using Lab4.Repositories;
using System.Threading.Tasks;

namespace Lab4.Services
{
    public interface IUserService 
    {
        Task<User> RegisterUserAsync(CreateUserDto dto);
    }

    public class UserService : IUserService 
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository) { _repository = repository; }

        public async Task<User> RegisterUserAsync(CreateUserDto dto)
        {
            var user = new User { FullName = dto.FullName, Phone = dto.Phone };
            await _repository.AddAsync(user);
            return user;
        }
    }
}