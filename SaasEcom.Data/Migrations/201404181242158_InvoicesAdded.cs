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
                        StripeId = c.String(maxLength: 50),
                        StripeCustomerId = c.String(maxLength: 50),
                        Date = c.DateTime(nullable: false),
                        PeriodStart = c.DateTime(nullable: false),
                        PeriodEnd = c.DateTime(nullable: false),
                        Subtotal = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        Attempted = c.Boolean(nullable: false),
                        Closed = c.Boolean(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        AttemptCount = c.Int(nullable: false),
                        AmountDue = c.Int(nullable: false),
                        Currency = c.String(),
                        StartingBalance = c.Int(nullable: false),
                        EndingBalance = c.Int(),
                        NextPaymentAttempt = c.DateTime(),
                        Charge = c.Int(),
                        Discount = c.Int(),
                        ApplicationFee = c.Int(),
                        Customer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id)
                .Index(t => t.StripeId, unique: true)
                .Index(t => t.StripeCustomerId)
                .Index(t => t.Paid)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.LineItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StripeLineItemId = c.String(),
                        Type = c.String(),
                        Amount = c.Int(nullable: false),
                        Currency = c.String(),
                        Proration = c.Boolean(nullable: false),
                        Period_Start = c.DateTime(nullable: false),
                        Period_End = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Plan_StripePlanId = c.String(),
                        Plan_Interval = c.String(),
                        Plan_Name = c.String(),
                        Plan_Created = c.DateTime(nullable: false),
                        Plan_AmountInCents = c.Int(nullable: false),
                        Plan_Currency = c.String(),
                        Plan_IntervalCount = c.Int(nullable: false),
                        Plan_TrialPeriodDays = c.Int(nullable: false),
                        Plan_StatementDescription = c.String(),
                        Invoice_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
                .Index(t => t.Invoice_Id);
            
            AddColumn("dbo.Subscriptions", "ApplicationUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Subscriptions", "SubscriptionPlanId", c => c.Int(nullable: false));
            CreateIndex("dbo.Subscriptions", "SubscriptionPlanId");
            AddForeignKey("dbo.Subscriptions", "SubscriptionPlanId", "dbo.SubscriptionPlans", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "SubscriptionPlanId", "dbo.SubscriptionPlans");
            DropForeignKey("dbo.LineItems", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CreditCards", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionPlanId" });
            DropIndex("dbo.LineItems", new[] { "Invoice_Id" });
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            DropIndex("dbo.Invoices", new[] { "Paid" });
            DropIndex("dbo.Invoices", new[] { "StripeCustomerId" });
            DropIndex("dbo.Invoices", new[] { "StripeId" });
            DropIndex("dbo.CreditCards", new[] { "User_Id" });
            AlterColumn("dbo.Subscriptions", "SubscriptionPlanId", c => c.Int());
            DropColumn("dbo.Subscriptions", "ApplicationUserId");
            DropTable("dbo.LineItems");
            DropTable("dbo.Invoices");
            DropTable("dbo.CreditCards");
            RenameColumn(table: "dbo.Subscriptions", name: "SubscriptionPlanId", newName: "SubscriptionPlan_Id");
            CreateIndex("dbo.Subscriptions", "SubscriptionPlan_Id");
            AddForeignKey("dbo.Subscriptions", "SubscriptionPlan_Id", "dbo.SubscriptionPlans", "Id");
        }
    }
}
