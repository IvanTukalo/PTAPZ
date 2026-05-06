using System;
namespace Lab4.Models
{
    public class Order 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; } // "booked", "returned"
    }
}