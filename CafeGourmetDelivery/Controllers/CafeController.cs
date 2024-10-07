using CafeGourmetDelivery.Models;
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
        
        // Ação para a página de pagamento
        public ActionResult Pagamento()
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

        // Método para adicionar um item ao carrinho
        public ActionResult AdicionarAoCarrinho(string nomeProduto, string caminhoImagem, decimal preco, int quantidade)
        {
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();
            carrinho.AdicionarItem(nomeProduto, caminhoImagem, preco, quantidade);
            Session["Carrinho"] = carrinho;

            return RedirectToAction("VerCarrinho");
        }

        // Página do carrinho
        public ActionResult VerCarrinho()
        {
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();
            return View(carrinho);
        }

        [HttpPost]
        public ActionResult ConfirmarPedido(string nomeProduto, int quantidade)
        {
            // Calcula o valor total com base no produto selecionado
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
        public ActionResult AtualizarCarrinho(Dictionary<string, int> quantidades)
        {
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();

            foreach (var item in quantidades)
            {
                var nomeProduto = item.Key;
                var novaQuantidade = item.Value;

                if (novaQuantidade > 0)
                {
                    var produto = carrinho.Itens.FirstOrDefault(i => i.NomeProduto == nomeProduto);
                    if (produto != null)
                    {
                        produto.Quantidade = novaQuantidade; // Atualiza a quantidade
                    }
                    else
                    {
                        // Caso o produto não exista no carrinho, adicione um novo
                        // Aqui, você pode precisar buscar a informação do produto de um repositório ou banco de dados
                        // Exemplo:
                        // var produtoInfo = ...; // Busque o produto
                        // carrinho.AdicionarItem(produtoInfo.Nome, produtoInfo.Imagem, produtoInfo.Preco, novaQuantidade);
                    }
                }
                else
                {
                    // Se a quantidade é 0, remova o item do carrinho
                    carrinho.Itens.RemoveAll(i => i.NomeProduto == nomeProduto);
                }
            }

            Session["Carrinho"] = carrinho; // Atualiza a sessão com o carrinho modificado

            return RedirectToAction("VerCarrinho"); // Redireciona para a página do carrinho
        }


        [HttpPost]
        public ActionResult ConfirmarPagamento(string nome, string numeroCartao, string validade, string cvv)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(numeroCartao) ||
                string.IsNullOrWhiteSpace(validade) || string.IsNullOrWhiteSpace(cvv))
            {
                ViewBag.Mensagem = "Por favor, preencha todos os campos.";
                return View("Pagamento"); // Retorne para a mesma view com a mensagem
            }

            // Lógica para processar o pagamento (simulação)
            ViewBag.Mensagem = "Pagamento realizado com sucesso! Obrigado por seu pedido.";
            return View("Confirmacao");
        }

    }
}