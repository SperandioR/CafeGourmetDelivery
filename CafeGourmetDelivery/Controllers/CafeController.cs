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
            using (var context = new ApplicationDbContext())
            {
                var produtos = context.Produtos.ToList();
                return View(produtos);
            }
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

        // Ação para a página de promoções 
        public ActionResult Promocoes()
        {
            var promocoes = new List<Produto>
            {
                new Produto { Id = 1, NomeProduto = "Promoção Café Expresso", Preco = 8.00m, CaminhoImagem = "imagens/copodecafe.png" },
                new Produto { Id = 2, NomeProduto = "Promoção Café com Leite", Preco = 10.00m, CaminhoImagem = "imagens/cafecomleite.png" }
            };

            return View(promocoes);
        }

        // Ação para a página de pagamento
        public ActionResult Pagamento()
        {
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();

            if (carrinho.Itens.Count == 0)
            {
                ViewBag.Mensagem = "Seu carrinho está vazio!";
                return View();
            }

            ViewBag.NomeProduto = carrinho.Itens.FirstOrDefault()?.NomeProduto;
            ViewBag.Quantidade = carrinho.Itens.FirstOrDefault()?.Quantidade;
            ViewBag.PrecoTotal = carrinho.ObterTotal();

            return View();
        }

        // Método de Ação para fazer o pedido de um produto
        public ActionResult Pedido(string nomeProduto)
        {
            ViewBag.NomeProduto = nomeProduto;

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
                        produto.Quantidade = novaQuantidade;
                    }
                    else
                    {
                        carrinho.AdicionarItem(nomeProduto, "~/Content/imagens/produto-exemplo.png", 0, novaQuantidade);
                    }
                }
                else
                {
                    carrinho.Itens.RemoveAll(i => i.NomeProduto == nomeProduto);
                }
            }

            Session["Carrinho"] = carrinho;
            return RedirectToAction("VerCarrinho");
        }

        [HttpPost]
        public ActionResult ConfirmarPagamento(string nome, string numeroCartao, string validade, string cvv)
        {
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();

            if (carrinho.Itens.Count == 0)
            {
                ViewBag.Mensagem = "O carrinho está vazio. Não é possível processar o pagamento.";
                return View("Pagamento");
            }

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(numeroCartao) ||
                string.IsNullOrWhiteSpace(validade) || string.IsNullOrWhiteSpace(cvv))
            {
                ViewBag.Mensagem = "Por favor, preencha todos os campos corretamente.";
                return View("Pagamento");
            }

            using (var db = new ApplicationDbContext())
            {
                foreach (var item in carrinho.Itens)
                {
                    var produto = db.Produtos.FirstOrDefault(p => p.NomeProduto == item.NomeProduto);
                    if (produto != null)
                    {
                        if (produto.QuantidadeDisponivel >= item.Quantidade)
                        {
                            produto.QuantidadeDisponivel -= item.Quantidade;
                        }
                        else
                        {
                            ViewBag.Mensagem = $"Estoque insuficiente para o produto {produto.NomeProduto}.";
                            return View("Pagamento");
                        }
                    }
                }

                db.SaveChanges();
            }

            Session["Carrinho"] = null;
            ViewBag.Mensagem = "Pagamento realizado com sucesso! Obrigado por seu pedido.";
            return View("Confirmacao");
        }
    }
}
