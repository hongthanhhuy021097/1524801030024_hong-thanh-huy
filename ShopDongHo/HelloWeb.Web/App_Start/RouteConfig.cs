using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HelloWeb.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                 name: "Contact",
                 url: "lien-he",
                 defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "HelloWeb.Web.Controllers" }
                );
            routes.MapRoute(
                  name: "Search",
                  url: "tim-kiem",
                  defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                  namespaces: new string[] { "HelloWeb.Web.Controllers" }
              );
            routes.MapRoute(
                 name: "Login",
                 url: "dang-nhap",
                 defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                 namespaces: new string[] { "HelloWeb.Web.Controllers" }
             );
            routes.MapRoute(
               name: "Register",
               url: "dang-ky",
               defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
               namespaces: new string[] { "HelloWeb.Web.Controllers" }
           );
            routes.MapRoute(
            name: "Cart",
            url: "gio-hang",
            defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
            namespaces: new string[] { "HelloWeb.Web.Controllers" }
        );
            routes.MapRoute(
           name: "Checkout",
           url: "thanh-toan",
           defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
           namespaces: new string[] { "HelloWeb.Web.Controllers" }
       );
            routes.MapRoute(
                   name: "Page",
                   url: "trang/{alias}",
                   defaults: new { controller = "Page", action = "Checkout", alias = UrlParameter.Optional },
                   namespaces: new string[] { "HelloWeb.Web.Controllers" }
               );

            routes.MapRoute(
                 name: "Product Category",
                 url: "{alias}pc{id}",
                 defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                   namespaces: new string[] { "HelloWeb.Web.Controllers" }
             );
            routes.MapRoute(
                 name: "Products",
                 url: "ViewAll",
                 defaults: new { controller = "Product", action = "ViewAll", id = UrlParameter.Optional },
                   namespaces: new string[] { "HelloWeb.Web.Controllers" }
             );
            routes.MapRoute(
                 name: "Product",
                 url: "{alias}p{productId}",
                 defaults: new { controller = "Product", action = "Detail", productId = UrlParameter.Optional },
                   namespaces: new string[] { "HelloWeb.Web.Controllers" }
             );
            routes.MapRoute(
                 name: "TagList",
                 url: "tag/{tagId}",
                 defaults: new { controller = "Product", action = "ListByTag", tagId = UrlParameter.Optional },
                   namespaces: new string[] { "HelloWeb.Web.Controllers" }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                  namespaces: new string[] { "HelloWeb.Web.Controllers" }
            );
        }
        }
}
