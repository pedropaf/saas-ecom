namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StripeAccountAdded : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Settings", new[] { "Key" });
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.CreditCards", "StripeAccount_Id", c => c.Int());
            AddColumn("dbo.Invoices", "StripeAccount_Id", c => c.Int());
            AddColumn("dbo.Subscriptions", "StripeAccount_Id", c => c.Int());
            AddColumn("dbo.SubscriptionPlans", "StripeAccount_Id", c => c.Int());
            CreateIndex("dbo.CreditCards", "StripeAccount_Id");
            CreateIndex("dbo.Invoices", "StripeAccount_Id");
            CreateIndex("dbo.Subscriptions", "StripeAccount_Id");
            CreateIndex("dbo.SubscriptionPlans", "StripeAccount_Id");
            AddForeignKey("dbo.Invoices", "StripeAccount_Id", "dbo.StripeAccounts", "Id");
            AddForeignKey("dbo.Subscriptions", "StripeAccount_Id", "dbo.StripeAccounts", "Id");
            AddForeignKey("dbo.SubscriptionPlans", "StripeAccount_Id", "dbo.StripeAccounts", "Id");
            AddForeignKey("dbo.CreditCards", "StripeAccount_Id", "dbo.StripeAccounts", "Id");
            DropTable("dbo.Settings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(maxLength: 100),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.CreditCards", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.SubscriptionPlans", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.Subscriptions", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.StripeAccounts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Invoices", "StripeAccount_Id", "dbo.StripeAccounts");
            DropIndex("dbo.SubscriptionPlans", new[] { "StripeAccount_Id" });
            DropIndex("dbo.Subscriptions", new[] { "StripeAccount_Id" });
            DropIndex("dbo.Invoices", new[] { "StripeAccount_Id" });
            DropIndex("dbo.StripeAccounts", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CreditCards", new[] { "StripeAccount_Id" });
            DropColumn("dbo.SubscriptionPlans", "StripeAccount_Id");
            DropColumn("dbo.Subscriptions", "StripeAccount_Id");
            DropColumn("dbo.Invoices", "StripeAccount_Id");
            DropColumn("dbo.CreditCards", "StripeAccount_Id");
            DropTable("dbo.StripeAccounts");
            CreateIndex("dbo.Settings", "Key", unique: true);
        }
    }
}
