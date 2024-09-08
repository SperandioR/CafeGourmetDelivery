using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CafeGourmetDelivery.Controllers
{
    public class CafeController : Controller
    {
        // GET: Cafe
        public ActionResult Index()
        {
            return View();
        }

        // Página do menu
        public ActionResult Menu()
        {
            // Aqui você pode adicionar lógica para buscar itens de um banco de dados
            return View();
        }

        // Página sobre nós
        public ActionResult Sobre()
        {
            return View();
        }

        // Página de contato
        public ActionResult Contato()
        {
            return View();
        }
    }
}