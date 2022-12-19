using FPT_LIBRARY.Models;
using FPT_LIBRARY.Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_LIBRARY.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        [Authorize(Roles = "User,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity= x.Quantity,
                        Price= x.Price,
                    }));
                    order.TotalAmount=cart.Items.Sum(x=>(x.Price*x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreateDate= DateTime.Now;
                    order.ModifiedDate= DateTime.Now;
                    order.CreateBy = req.Phone;
                    order.Email= req.Email;
                    Random rd = new Random();
                    order.Code="OD"+ rd.Next(0,9)+ rd.Next(0, 9)+ rd.Next(0, 9)+ rd.Next(0, 9)+ rd.Next(0, 9)+ rd.Next(0, 9);
                    db.Orders.Add(order);
                    db.SaveChanges();
                    //send mail to customer
                    var strProduct = "";
                    var price = decimal.Zero;
                    var total = decimal.Zero;
                    foreach(var sp in cart.Items)
                    {
                        strProduct += "<tr>";
                        strProduct += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word\">" + sp.ProductName + "</td>";
                        strProduct += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word\">" + sp.Quantity + "</td>";
                        strProduct += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word\">" + FPT_LIBRARY.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strProduct += "</tr>";
                        price += sp.Price * sp.Quantity;;
                    }
                    var tax = decimal.Zero;
                    tax = price / 100 * 9;
                    total =price + tax;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{OrderId}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{Product}}", strProduct);
                    contentCustomer = contentCustomer.Replace("{{Day}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{FullName}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{Address}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{Price}}", FPT_LIBRARY.Common.Common.FormatNumber(price, 0));
                    contentCustomer = contentCustomer.Replace("{{Tax}}", FPT_LIBRARY.Common.Common.FormatNumber(tax, 0));
                    contentCustomer = contentCustomer.Replace("{{Total}}", FPT_LIBRARY.Common.Common.FormatNumber(total, 0));
                    FPT_LIBRARY.Common.Common.SendMail("FPT Library", "Order #" + order.Code, contentCustomer.ToString(), req.Email);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{OrderId}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{Product}}", strProduct);
                    contentAdmin = contentAdmin.Replace("{{Day}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{FullName}}", order.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    contentAdmin = contentAdmin.Replace("{{Address}}", order.Address);
                    contentAdmin = contentAdmin.Replace("{{Price}}", FPT_LIBRARY.Common.Common.FormatNumber(price, 0));
                    contentAdmin = contentAdmin.Replace("{{Tax}}", FPT_LIBRARY.Common.Common.FormatNumber(tax, 0));
                    contentAdmin = contentAdmin.Replace("{{Total}}", FPT_LIBRARY.Common.Common.FormatNumber(total, 0));
                    FPT_LIBRARY.Common.Common.SendMail("FPT Library", "New order #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    cart.ClearCart();
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }

        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        [Authorize(Roles = "User,Administrator")]
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.ChekCart=cart;
            }
            return View();
        }
        public ActionResult Partial_Item_CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count },JsonRequestBehavior.AllowGet);

            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count=0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if(cart == null) 
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias=checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                if (checkProduct.IsSale == true && checkProduct.PriceSale != null && checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;

                }
                else
                {
                    item.Price = (decimal)checkProduct.Price;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg="Add to Cart Sucessfully!", code= 1, Count =cart.Items.Count };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var chekProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (chekProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }               
            }
            return Json(code);

        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null )
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {

                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
}