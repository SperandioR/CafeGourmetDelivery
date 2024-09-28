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

        // Método de Ação para fazer o pedido de um produto
        public ActionResult Pedido(string nomeProduto)
        {
            ViewBag.NomeProduto = nomeProduto;
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmarPedido(string nomeProduto, int quantidade)
        {
            // Lógica para salvar o pedido ou redirecionar o cliente a uma página de confirmação
            ViewBag.Mensagem = $"Pedido confirmado: {quantidade}x {nomeProduto}. Obrigado por comprar conosco!";
            return View("Confirmacão");
        }

    }
}