namespace CafeGourmetDelivery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionarQuantidadeDisponivel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produtoes", "QuantidadeDisponivel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produtoes", "QuantidadeDisponivel");
        }
    }
}
