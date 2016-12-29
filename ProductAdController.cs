using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;
using OnLineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductAdController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Admin/ProductAd/

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
            var items = new List<Product>();
            items = db.Products.ToList();

            var pager = Pager.Set("prods", 15, 4, items.Count());
            return View(items.Skip(pager.StartIndex).Take(pager.PageSize));
        }

        //
        // GET: /Admin/ProductAd/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Admin/ProductAd/Create

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "NameVN");
            //ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            return View();
        }

        //
        // POST: /Admin/ProductAd/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {


            if (ModelState.IsValid)
            {
               
                HttpPostedFileBase file = Request.Files["uploadimg"];
                if (file != null && file.ContentLength > 0)
                {
                    String path = Server.MapPath("~/Content/Images/" + file.FileName);
                    file.SaveAs(path);
                    product.Picture = file.FileName;
                }
                else
                {
                    product.Picture = "NoImage.jpg";
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "NameVN", product.CategoryID);
            //ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        //
        // GET: /Admin/ProductAd/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "NameVN", product.CategoryID);
            //ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        //
        // POST: /Admin/ProductAd/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "NameVN", product.CategoryID);
            //ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        //
        // GET: /Admin/ProductAd/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Admin/ProductAd/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}