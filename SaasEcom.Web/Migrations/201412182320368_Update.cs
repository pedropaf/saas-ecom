namespace SaasEcom.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.StripeAccounts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subscriptions", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.SubscriptionPlans", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.CreditCards", "StripeAccount_Id", "dbo.StripeAccounts");
            DropIndex("dbo.CreditCards", new[] { "StripeAccount_Id" });
            DropIndex("dbo.StripeAccounts", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Invoices", new[] { "StripeAccount_Id" });
            DropIndex("dbo.Subscriptions", new[] { "StripeAccount_Id" });
            DropIndex("dbo.SubscriptionPlans", new[] { "StripeAccount_Id" });
            RenameColumn(table: "dbo.CreditCards", name: "ApplicationUserId", newName: "SaasEcomUserId");
            RenameIndex(table: "dbo.CreditCards", name: "IX_ApplicationUserId", newName: "IX_SaasEcomUserId");
            AddColumn("dbo.Subscriptions", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.CreditCards", "StripeAccount_Id");
            DropColumn("dbo.Invoices", "StripeAccount_Id");
            DropColumn("dbo.Subscriptions", "ApplicationUserId");
            DropColumn("dbo.Subscriptions", "StripeAccount_Id");
            DropColumn("dbo.SubscriptionPlans", "StripeAccount_Id");
            DropTable("dbo.StripeAccounts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StripeAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LiveMode = c.Boolean(nullable: false),
                        StripeLivePublicApiKey = c.String(),
                        StripeLiveSecretApiKey = c.String(),
                        StripeTestPublicApiKey = c.String(),
                        StripeTestSecretApiKey = c.String(),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SubscriptionPlans", "StripeAccount_Id", c => c.Int());
            AddColumn("dbo.Subscriptions", "StripeAccount_Id", c => c.Int());
            AddColumn("dbo.Subscriptions", "ApplicationUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Invoices", "StripeAccount_Id", c => c.Int());
            AddColumn("dbo.CreditCards", "StripeAccount_Id", c => c.Int());
            DropColumn("dbo.Subscriptions", "UserId");
            RenameIndex(table: "dbo.CreditCards", name: "IX_SaasEcomUserId", newName: "IX_ApplicationUserId");
            RenameColumn(table: "dbo.CreditCards", name: "SaasEcomUserId", newName: "ApplicationUserId");
            CreateIndex("dbo.SubscriptionPlans", "StripeAccount_Id");
            CreateIndex("dbo.Subscriptions", "StripeAccount_Id");
            CreateIndex("dbo.Invoices", "StripeAccount_Id");
            CreateIndex("dbo.StripeAccounts", "ApplicationUser_Id");
            CreateIndex("dbo.CreditCards", "StripeAccount_Id");
            AddForeignKey("dbo.CreditCards", "StripeAccount_Id", "dbo.StripeAccounts", "Id");
            AddForeignKey("dbo.SubscriptionPlans", "StripeAccount_Id", "dbo.StripeAccounts", "Id");
            AddForeignKey("dbo.Subscriptions", "StripeAccount_Id", "dbo.StripeAccounts", "Id");
            AddForeignKey("dbo.StripeAccounts", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Invoices", "StripeAccount_Id", "dbo.StripeAccounts", "Id");
        }
    }
}
