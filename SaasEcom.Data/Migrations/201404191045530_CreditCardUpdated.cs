namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditCardUpdated : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CreditCards", new[] { "User_Id" });
            DropColumn("dbo.CreditCards", "ApplicationUserId");
            RenameColumn(table: "dbo.CreditCards", name: "User_Id", newName: "ApplicationUserId");
            AlterColumn("dbo.CreditCards", "Cvc", c => c.String(maxLength: 4));
            AlterColumn("dbo.CreditCards", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.CreditCards", "ApplicationUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CreditCards", new[] { "ApplicationUserId" });
            AlterColumn("dbo.CreditCards", "ApplicationUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.CreditCards", "Cvc", c => c.String());
            RenameColumn(table: "dbo.CreditCards", name: "ApplicationUserId", newName: "User_Id");
            AddColumn("dbo.CreditCards", "ApplicationUserId", c => c.Int(nullable: false));
            CreateIndex("dbo.CreditCards", "User_Id");
        }
    }
}
