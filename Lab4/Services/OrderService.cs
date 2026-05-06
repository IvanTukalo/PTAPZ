using Lab4.DTOs;
using Lab4.Models;
using Lab4.Repositories;
using System;
using System.Threading.Tasks;

namespace Lab4.Services
{
    public interface IOrderService 
    {
        Task<Order> CreateOrderAsync(CreateOrderDto dto);
        Task ReturnBookAsync(int orderId);
    }

    public class OrderService : IOrderService 
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IBookRepository _bookRepo;

        public OrderService(IOrderRepository orderRepo, IBookRepository bookRepo)
        {
            _orderRepo = orderRepo;
            _bookRepo = bookRepo;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto dto) 
        {
            var book = await _bookRepo.GetByIdAsync(dto.BookId);
            
            // Перевірка надійності (Reliability)
            if (book == null || book.AvailableCopies <= 0) 
            {
                throw new Exception("Немає вільних примірників цієї книги.");
            }

            book.AvailableCopies -= 1;
            await _bookRepo.UpdateAsync(book);

            var order = new Order 
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                IssueDate = DateTime.UtcNow,
                Status = "issued"
            };
            
            await _orderRepo.AddAsync(order);
            return order;
        }

        public async Task ReturnBookAsync(int orderId) 
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null || order.Status == "returned") throw new Exception("Ордер не знайдено або вже закрито.");

            order.Status = "returned";
            await _orderRepo.UpdateAsync(order);

            var book = await _bookRepo.GetByIdAsync(order.BookId);
            book.AvailableCopies += 1;
            await _bookRepo.UpdateAsync(book);
        }
    }
}