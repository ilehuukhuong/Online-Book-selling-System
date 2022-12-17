using FPT_LIBRARY.Models;
using FPT_LIBRARY.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_LIBRARY.Areas.Admin.Controllers
{
    public class SettingSystemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/SettingSystem
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_Setting() {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddSetting(SettingSystemViewModel req)
        {
            SystemSetting set = null;
            var checkTitle = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingTitle"));
            if (checkTitle==null || string.IsNullOrEmpty(checkTitle.SettingValue)) 
            {
                set = new SystemSetting();
                set.SettingValue = "SettingTitle";
                set.SettingKey = req.SettingTitle;
                db.SystemSettings.Add(set);                
            }
            else
            {
                checkTitle.SettingValue= req.SettingTitle;
                db.Entry(checkTitle).State = System.Data.Entity.EntityState.Modified;
            }
            var checkLogo = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingLogo"));
            if (checkLogo == null || string.IsNullOrEmpty(checkLogo.SettingValue))
            {
                set = new SystemSetting();
                set.SettingValue = "SettingLogo";
                set.SettingKey = req.SettingLogo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkLogo.SettingValue = req.SettingLogo;
                db.Entry(checkTitle).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return View("Partial_Setting");
        }
    }
}