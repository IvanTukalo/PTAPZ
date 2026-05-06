using Lab4.DTOs;
using Lab4.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lab4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase 
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService) { _orderService = orderService; }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto) 
        {
            try 
            {
                var order = await _orderService.CreateOrderAsync(dto);
                return StatusCode(201, order); 
            } 
            catch (Exception ex) 
            {
                return BadRequest(new { Message = ex.Message }); 
            }
        }

        [HttpPatch("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id) 
        {
            try 
            {
                await _orderService.ReturnBookAsync(id);
                return Ok(new { Message = "Книгу успішно повернуто" });
            } 
            catch (Exception ex) 
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}