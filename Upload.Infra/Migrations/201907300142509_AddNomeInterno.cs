namespace Upload.Infra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNomeInterno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.File", "NomeInterno", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.File", "NomeInterno");
        }
    }
}
