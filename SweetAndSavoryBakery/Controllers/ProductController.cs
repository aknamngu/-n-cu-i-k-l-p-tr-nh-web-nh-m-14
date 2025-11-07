using SweetAndSavoryBakery.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SweetAndSavoryBakery.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Hiển thị tất cả sản phẩm
        [AllowAnonymous]
        public ActionResult Index(int? categoryId)
        {
            var products = db.Products.Include(p => p.Category).ToList();
            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.SelectedCategory = categoryId;
            return View(products);
        }

        // Xem chi tiết sản phẩm
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index"); // hoặc return HttpNotFound();

            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

    }
}
