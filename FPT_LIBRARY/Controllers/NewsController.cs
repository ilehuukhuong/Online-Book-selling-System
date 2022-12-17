using FPT_LIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_LIBRARY.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();   
        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_News_Home()
        {
            var items=db.News.Take(3).ToList();
            return PartialView(items);
        }
    }
}