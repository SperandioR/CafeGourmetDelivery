using System.Web.Mvc;

namespace CafeGourmetDelivery.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente/Login
       public ActionResult Login()
        {
            return View();
        }

        // Get: Cliente/Cadastrar
        public ActionResult Cadastrar()
        {
            return View();
        }
    }
}
