using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SweetAndSavoryBakery.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(120)]
        public string CustomerName { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public decimal Total { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Paid, Shipped, Completed, Cancelled
    }

    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
