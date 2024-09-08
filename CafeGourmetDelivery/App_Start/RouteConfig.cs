using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CafeGourmetDelivery
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //Modifique a rota padrão para apontar para o controlador Cafe e a ação
                defaults: new { controller = "Cafe", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
