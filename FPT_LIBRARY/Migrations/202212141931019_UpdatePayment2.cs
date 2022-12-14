namespace FPT_LIBRARY.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePayment2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.tb_OrderDetail");
            AddColumn("dbo.tb_OrderDetail", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.tb_OrderDetail", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.tb_OrderDetail");
            DropColumn("dbo.tb_OrderDetail", "Id");
            AddPrimaryKey("dbo.tb_OrderDetail", new[] { "OrderId", "ProductId" });
        }
    }
}
