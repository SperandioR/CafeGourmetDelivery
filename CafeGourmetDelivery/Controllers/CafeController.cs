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
                // Busca os produtos do banco de dados
                var produtos = context.Produtos.ToList();
                return View(produtos); // Envia os produtos para a view
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

        // Ação para a página de pagamento
        public ActionResult Pagamento()
        {
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();

            if (carrinho.Itens.Count == 0)
            {
                ViewBag.Mensagem = "Seu carrinho está vazio!";
                return View();
            }

            // Passando informações do carrinho para o ViewBag
            ViewBag.NomeProduto = carrinho.Itens.FirstOrDefault()?.NomeProduto;
            ViewBag.Quantidade = carrinho.Itens.FirstOrDefault()?.Quantidade;
            ViewBag.PrecoTotal = carrinho.ObterTotal();

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
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();

            // Verifique se o carrinho tem itens
            if (carrinho.Itens.Count == 0)
            {
                ViewBag.Mensagem = "O carrinho está vazio. Não é possível processar o pagamento.";
                return View("Pagamento"); // Retorna para a página de pagamento com a mensagem de erro
            }

            // Simulação de validação do pagamento (aqui você pode integrar com um serviço real de pagamento)
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(numeroCartao) ||
                string.IsNullOrWhiteSpace(validade) || string.IsNullOrWhiteSpace(cvv))
            {
                ViewBag.Mensagem = "Por favor, preencha todos os campos corretamente.";
                return View("Pagamento"); // Retorna para a mesma view com a mensagem
            }

            // Se o pagamento for simulado como bem-sucedido, atualize o estoque dos produtos
            using (var db = new ApplicationDbContext())
            {
                foreach (var item in carrinho.Itens)
                {
                    // Localiza o produto no banco de dados
                    var produto = db.Produtos.FirstOrDefault(p => p.NomeProduto == item.NomeProduto);
                    if (produto != null)
                    {
                        // Verifica se há quantidade disponível
                        if (produto.QuantidadeDisponivel >= item.Quantidade)
                        {
                            // Subtrai a quantidade comprada do estoque
                            produto.QuantidadeDisponivel -= item.Quantidade;
                        }
                        else
                        {
                            ViewBag.Mensagem = $"Estoque insuficiente para o produto {produto.NomeProduto}.";
                            return View("Pagamento"); // Retorna para a página de pagamento com a mensagem de erro
                        }
                    }
                }

                // Salva as alterações no banco de dados
                db.SaveChanges();
            }

            // Limpa o carrinho após o pagamento bem-sucedido
            Session["Carrinho"] = null;

            // Exibe a página de confirmação com sucesso
            ViewBag.Mensagem = "Pagamento realizado com sucesso! Obrigado por seu pedido.";
            return View("Confirmacao");
        }
    }
}