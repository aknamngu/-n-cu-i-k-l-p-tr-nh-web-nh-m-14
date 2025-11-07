using SweetAndSavoryBakery.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SweetAndSavoryBakery.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string CartKey = "CART";

        // Lấy giỏ hàng trong session
        private List<CartItem> GetCart()
        {
            if (Session[CartKey] == null)
                Session[CartKey] = new List<CartItem>();
            return (List<CartItem>)Session[CartKey];
        }

        // Thêm sản phẩm
        public ActionResult Add(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();

            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.ProductId == id);

            if (existing != null)
                existing.Quantity++;
            else
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = 1
                });

            return RedirectToAction("Index");
        }

        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCart();
            ViewBag.Total = cart.Sum(x => x.Quantity * x.Price);
            return View(cart);
        }

        // Xóa sản phẩm
        public ActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item != null) cart.Remove(item);
            return RedirectToAction("Index");
        }

        // Thanh toán
        public ActionResult Checkout()
        {
            ViewBag.Total = GetCart().Sum(x => x.Quantity * x.Price);
            return View();
        }
    }
}
