using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CafeGourmetDelivery.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public decimal Preco { get; set; }
        public string CaminhoImagem { get; set; }
        
        //Verifica a quantidade disponivel em estoque.
        public int QuantidadeDisponivel { get; set; }
    }
}