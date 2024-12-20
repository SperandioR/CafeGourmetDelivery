﻿using CafeGourmetDelivery.Models;
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
        public ActionResult Pagamento(int? idProduto = null, decimal? precoPromocional = null)
        {
            if (idProduto.HasValue)
            {
                using (var db = new ApplicationDbContext())
                {
                    var produto = db.Produtos.FirstOrDefault(p => p.Id == idProduto.Value);
                    if (produto != null)
                    {
                        // Usar o preço promocional se estiver presente; caso contrário, o preço padrão
                        decimal preco = precoPromocional ?? produto.Preco;

                        // Configura as informações para exibição na página de pagamento
                        ViewBag.NomeProduto = produto.NomeProduto;
                        ViewBag.PrecoTotal = preco;
                        ViewBag.Quantidade = 1;

                        // Armazena o item no "ItensPedido" para exibição na página de confirmação
                        Session["ItensPedido"] = new List<ItemCarrinho>
                {
                    new ItemCarrinho { NomeProduto = produto.NomeProduto, Preco = preco, Quantidade = 1 }
                };

                        return View();
                    }
                    else
                    {
                        ViewBag.Mensagem = "Produto em promoção não encontrado.";
                        return RedirectToAction("Promocoes");
                    }
                }
            }

            // Código para exibir o carrinho, caso nenhum idProduto seja fornecido
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();
            if (carrinho.Itens.Count == 0)
            {
                ViewBag.Mensagem = "Seu carrinho está vazio!";
                return View();
            }

            ViewBag.NomeProduto = carrinho.Itens.FirstOrDefault()?.NomeProduto;
            ViewBag.Quantidade = carrinho.Itens.FirstOrDefault()?.Quantidade;
            ViewBag.PrecoTotal = carrinho.ObterTotal();

            Session["ItensPedido"] = carrinho.Itens;

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
        public ActionResult AdicionarAoCarrinho(string nomeProduto, string caminhoImagem, decimal preco, int quantidade, bool redirectPagamento = false)
        {
            // Recupera o carrinho da sessão ou cria um novo
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();

            // Adiciona o item com o preço promocional
            carrinho.AdicionarItem(nomeProduto, caminhoImagem, preco, quantidade);
            Session["Carrinho"] = carrinho;

            // Verifica se deve redirecionar diretamente para o pagamento
            if (redirectPagamento)
            {
                return RedirectToAction("Pagamento");
            }

            //Metodo para Atualizar a Sessão com a Contagem de Itens no Carrinho
            Session["CarrinhoItemCount"] = carrinho.Itens.Sum(i => i.Quantidade);

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

            //Metodo para Atualizar a Sessão com a Contagem de Itens no Carrinho
            Session["CarrinhoItemCount"] = carrinho.Itens.Sum(i => i.Quantidade);

            Session["Carrinho"] = carrinho;
            return RedirectToAction("VerCarrinho");
        }

        [HttpPost]
        public ActionResult ConfirmarPagamento(string nome, string numeroCartao, string validade, string cvv)
        {
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();

            // Verifique se o carrinho tem itens
            if (carrinho.Itens.Count == 0)
            {
                ViewBag.Mensagem = "O carrinho está vazio. Não é possível processar o pagamento.";
                return View("Pagamento");
            }

            // Simulação de validação do pagamento
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

            // Passa os itens do carrinho para a confirmação
            ViewBag.ItensPedido = carrinho.Itens;
            ViewBag.PrecoTotal = carrinho.ObterTotal();
            ViewBag.Mensagem = "Pagamento realizado com sucesso! Obrigado por seu pedido.";

            // Limpa o carrinho após o pagamento
            Session["Carrinho"] = null;

            // Zera o contador de itens no carrinho
            Session["CarrinhoItemCount"] = 0;

            return View("Confirmacao");
        }

        public ActionResult RemoverDoCarrinho(string nomeProduto)
        {
            // Recupera o carrinho da sessão
            var carrinho = Session["Carrinho"] as Carrinho ?? new Carrinho();

            // Encontra o item no carrinho com o nome especificado
            var itemParaRemover = carrinho.Itens.FirstOrDefault(i => i.NomeProduto == nomeProduto);

            if (itemParaRemover != null)
            {
                // Remove o item do carrinho
                carrinho.Itens.Remove(itemParaRemover);
                // Atualiza a sessão com o carrinho modificado
                Session["Carrinho"] = carrinho;
            }

            //Metodo para Atualizar a Sessão com a Contagem de Itens no Carrinho
            Session["CarrinhoItemCount"] = carrinho.Itens.Sum(i => i.Quantidade);

            // Redireciona de volta para a página do carrinho
            return RedirectToAction("VerCarrinho");
        }


    }
}
