﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_LIBRARY.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Administrator")]
        //[Authorize(Roles = "Administrator,Staff")]
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}