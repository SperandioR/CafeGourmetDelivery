using CafeGourmetDelivery.Models;
using System.Data.Entity;

public class ApplicationDbContext : DbContext
{
    // Construtor que chama o DbContext base com a string de conexão
    public ApplicationDbContext() : base("name=DefaultConnection")
    {
    }

    // DbSet para as entidades (tabelas) do seu banco de dados
    public DbSet<Produto> Produtos { get; set; }
    // Adicione outras entidades aqui, como:
    // public DbSet<Cliente> Clientes { get; set; }
}
