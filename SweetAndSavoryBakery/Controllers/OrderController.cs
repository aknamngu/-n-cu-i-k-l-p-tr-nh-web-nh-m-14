using SweetAndSavoryBakery.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SweetAndSavoryBakery.Controllers
{
    [Authorize(Roles = "Customer,Admin")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string CART_KEY = "CART";

        public ActionResult Checkout() => View();

        [HttpPost]
        public ActionResult Checkout(string customerName, string address, string phone)
        {
            var cart = Session[CART_KEY] as List<CartItem> ?? new List<CartItem>();
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            var order = new Order
            {
                CustomerName = customerName,
                Address = address,
                Phone = phone,
                Total = cart.Sum(i => i.LineTotal),
                Items = cart.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Name,
                    UnitPrice = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };

            db.Orders.Add(order);
            db.SaveChanges();

            Session[CART_KEY] = new List<CartItem>();
            return RedirectToAction("Success", new { id = order.Id });
        }

        public ActionResult Success(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null) return HttpNotFound();
            return View(order);
        }
    }
}
