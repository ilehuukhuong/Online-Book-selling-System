using FPT_LIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;
using System.Web.Services.Description;

namespace FPT_LIBRARY.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db= new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var items=db.Orders.OrderByDescending(x=>x.CreateDate).ToList();

            if(page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize =pageSize;
            ViewBag.PageSize = pageNumber;


            return View(items.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult View(int id)
        {
            var items = db.Orders.Find(id);
            return View(items);

        }
        public ActionResult Partial_Product(int id)
        {
            var items = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }
        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.TypePayment= status;
                db.Entry(item).Property(x=>x.TypePayment).IsModified=true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }
    }
}