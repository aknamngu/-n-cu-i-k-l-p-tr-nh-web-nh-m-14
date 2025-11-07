using SweetAndSavoryBakery.Models;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SweetAndSavoryBakery.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Danh sách sản phẩm
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        // Thêm sản phẩm
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase imageFile)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                string fileName = Path.GetFileName(imageFile.FileName);
                string path = Path.Combine(Server.MapPath("~/Upload/"), fileName);
                imageFile.SaveAs(path);
                product.ImageUrl = "/Upload/" + fileName;
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // Sửa sản phẩm
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                // 🔹 Bước 1: Lấy bản ghi gốc từ DB
                var existing = db.Products.Find(product.Id);
                if (existing == null)
                    return HttpNotFound();

                // 🔹 Bước 2: Cập nhật các trường cơ bản
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.CategoryId = product.CategoryId;

                // 🔹 Bước 3: Nếu có ảnh mới thì lưu ảnh và cập nhật đường dẫn
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload/"), fileName);
                    imageFile.SaveAs(path);
                    existing.ImageUrl = "/Upload/" + fileName;
                }

                // 🔹 Bước 4: Lưu thay đổi
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }


        // Xóa sản phẩm
        public ActionResult Delete(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("DeleteConfirmed")]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
