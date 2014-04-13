namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoicesAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subscriptions", "SubscriptionPlan_Id", "dbo.SubscriptionPlans");
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionPlan_Id" });
            RenameColumn(table: "dbo.Subscriptions", name: "SubscriptionPlan_Id", newName: "SubscriptionPlanId");
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Last4 = c.String(),
                        Type = c.String(),
                        Fingerprint = c.String(),
                        AddressCity = c.String(),
                        AddressCountry = c.String(),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        AddressState = c.String(),
                        AddressZip = c.String(),
                        Cvc = c.String(),
                        ExpirationMonth = c.String(),
                        ExpirationYear = c.String(),
                        ApplicationUserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.Int(nullable: false),
                        InvoiceNumber = c.String(),
                        Date = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        BillingName = c.String(),
                        BillingAddressLine1 = c.String(),
                        BillingAddressLine2 = c.String(),
                        InvoiceConcept = c.String(),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceStatus = c.Int(nullable: false),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            AddColumn("dbo.Subscriptions", "ApplicationUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Subscriptions", "SubscriptionPlanId", c => c.Int(nullable: false));
            CreateIndex("dbo.Subscriptions", "SubscriptionPlanId");
            AddForeignKey("dbo.Subscriptions", "SubscriptionPlanId", "dbo.SubscriptionPlans", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "SubscriptionPlanId", "dbo.SubscriptionPlans");
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CreditCards", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionPlanId" });
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            DropIndex("dbo.CreditCards", new[] { "User_Id" });
            AlterColumn("dbo.Subscriptions", "SubscriptionPlanId", c => c.Int());
            DropColumn("dbo.Subscriptions", "ApplicationUserId");
            DropTable("dbo.Invoices");
            DropTable("dbo.CreditCards");
            RenameColumn(table: "dbo.Subscriptions", name: "SubscriptionPlanId", newName: "SubscriptionPlan_Id");
            CreateIndex("dbo.Subscriptions", "SubscriptionPlan_Id");
            AddForeignKey("dbo.Subscriptions", "SubscriptionPlan_Id", "dbo.SubscriptionPlans", "Id");
        }
    }
}
