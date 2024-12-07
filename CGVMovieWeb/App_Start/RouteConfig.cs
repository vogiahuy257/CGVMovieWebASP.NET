using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CGVMovieWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Account",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Showtimes",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Showtimes", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Seats",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Seats", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Admin",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Admin", action = "RevenueStatistics", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Tickets",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Tickets", action = "Details", id = UrlParameter.Optional }
           );
        }
    }
}
