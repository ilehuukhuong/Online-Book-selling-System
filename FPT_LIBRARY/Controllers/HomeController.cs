using FPT_LIBRARY.Models;
using FPT_LIBRARY.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_LIBRARY.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_Subcribe() {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subcribe(Subscribe req)
        {
            if(ModelState.IsValid)
            {
                db.Subscribes.Add(new Subscribe {Email= req.Email,CreatedDate=DateTime.Now});
                db.SaveChanges();
                return Json(new {Success=true});
            }
            return View("Partial_Subcribe", req);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}