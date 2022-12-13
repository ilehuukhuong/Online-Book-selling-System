using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FPT_LIBRARY
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Product",
               url: "product",
               defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "FPT_LIBRARY.Controllers" }
           );
            routes.MapRoute(
              name: "ShoppingCart",
              url: "cart",
              defaults: new { controller = "ShoppingCart", action = "Index", alias = UrlParameter.Optional },
              namespaces: new[] { "FPT_LIBRARY.Controllers" }
          );
            routes.MapRoute(
               name: "CategoryProducts",
               url: "product-category/{alias}-{id}",
               defaults: new { controller = "Products", action = "ProductCategory", id = UrlParameter.Optional },
               namespaces: new[] { "FPT_LIBRARY.Controllers" }
           );
            routes.MapRoute(
               name: "detailProduct",
               url: "detail/{alias}-p{id}",
               defaults: new { controller = "Products", action = "Detail", id = UrlParameter.Optional },
               namespaces: new[] { "FPT_LIBRARY.Controllers" }
           );
            routes.MapRoute(
              name: "Contact",
              url: "contact",
              defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
              namespaces: new[] { "FPT_LIBRARY.Controllers" }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FPT_LIBRARY.Controllers" }

            );
        }
    }
}
