using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CafeGourmetDelivery.Models
{
    public class ItemCarrinho
    {
        // Propriedade que representa o nome do produto no carrinho.
        public string NomeProduto { get; set; }

        // Propriedade que guarda o caminho da imagem do produto para exibição.
        public string CaminhoImagem { get; set; }

        // Propriedade que representa o preço do produto.
        public decimal Preco { get; set; }

        // Propriedade que armazena a quantidade do produto que foi adicionada ao carrinho.
        public int Quantidade { get; set; }
    }

    public class Carrinho
    {
        // Lista de itens no carrinho, representada por objetos da classe ItemCarrinho.
        public List<ItemCarrinho> Itens { get; set; } = new List<ItemCarrinho>();

        // Método que adiciona um item ao carrinho.
        public void AdicionarItem(string nomeProduto, string caminhoImagem, decimal preco, int quantidade)
        {
            // Verifica se o produto já está no carrinho.
            var itemExistente = Itens.FirstOrDefault(i => i.NomeProduto == nomeProduto);

            // Se o item já existe, apenas incrementa a quantidade.
            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
            }
            // Se o item não existe, cria um novo item e o adiciona à lista.
            else
            {
                Itens.Add(new ItemCarrinho
                {
                    NomeProduto = nomeProduto,
                    CaminhoImagem = caminhoImagem,
                    Preco = preco,
                    Quantidade = quantidade
                });
            }
        }

        // Método que calcula o valor total dos itens no carrinho.
        public decimal ObterTotal()
        {
            // Multiplica o preço pela quantidade para cada item no carrinho e soma os resultados.
            return Itens.Sum(i => i.Preco * i.Quantidade);
        }
    }
}