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

            // Definindo a imagem com base no nome do produto recebido
            switch (nomeProduto)
            {
                case "Café Expresso":
                    ViewBag.CaminhoImagem = "~/Content/imagens/copodecafe.png";
                    break;
                case "Café Latte":
                    ViewBag.CaminhoImagem = "~/Content/imagens/cafecomleite.png";
                    break;
                case "Cappuccino":
                    ViewBag.CaminhoImagem = "~/Content/imagens/cappuccino.png";
                    break;
                case "Bolo de Cenoura":
                    ViewBag.CaminhoImagem = "~/Content/imagens/bolocenoura.jpg";
                    break;
                case "Cookies de Chocolate":
                    ViewBag.CaminhoImagem = "~/Content/imagens/Cookies.png";
                    break;
                case "Croissant":
                    ViewBag.CaminhoImagem = "~/Content/imagens/croissant.png";
                    break;
                default:
                    ViewBag.CaminhoImagem = "~/Content/imagens/produto-exemplo.png";
                    break;
            }

            return View();
        }

        [HttpPost]
        public ActionResult ConfirmarPedido(string nomeProduto, int quantidade)
        {
            // Calcula o valor total com base no produto selecionado (valores exemplo)
            decimal precoUnitario = 0;
            switch (nomeProduto)
            {
                case "Café Expresso":
                    precoUnitario = 10.00m;
                    break;
                case "Café Latte":
                    precoUnitario = 12.00m;
                    break;
                case "Cappuccino":
                    precoUnitario = 14.00m;
                    break;
                case "Bolo de Cenoura":
                    precoUnitario = 8.00m;
                    break;
                case "Cookies de Chocolate":
                    precoUnitario = 5.00m;
                    break;
                case "Croissant":
                    precoUnitario = 7.00m;
                    break;
            }

            decimal precoTotal = precoUnitario * quantidade;

            // Passa as informações para a view de pagamento
            ViewBag.NomeProduto = nomeProduto;
            ViewBag.Quantidade = quantidade;
            ViewBag.PrecoTotal = precoTotal;

            return View("Pagamento");
        }


        [HttpPost]
        public ActionResult ConfirmarPagamento(string nome, string numeroCartao, string validade, string cvv)
        {
            // Lógica para processar o pagamento (simulação)
            ViewBag.Mensagem = "Pagamento realizado com sucesso! Obrigado por seu pedido.";
            return View("Confirmacao");
        }


    }
}