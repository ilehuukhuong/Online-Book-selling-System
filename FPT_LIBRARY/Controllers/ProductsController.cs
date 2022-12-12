﻿using FPT_LIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_LIBRARY.Controllers
{
    public class ProductsController : Controller

    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index(int? id)
        {
            var items = db.Products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.Id == id).ToList();
            }

            return View(items);
        }
        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.IsHome).Take(12).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
    }
}