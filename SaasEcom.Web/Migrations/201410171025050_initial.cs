namespace SaasEcom.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StripeId = c.String(),
                        StripeToken = c.String(),
                        Name = c.String(nullable: false),
                        Last4 = c.String(),
                        Type = c.String(),
                        Fingerprint = c.String(),
                        AddressCity = c.String(nullable: false),
                        AddressCountry = c.String(nullable: false),
                        AddressLine1 = c.String(nullable: false),
                        AddressLine2 = c.String(),
                        AddressState = c.String(),
                        AddressZip = c.String(nullable: false),
                        Cvc = c.String(nullable: false, maxLength: 4),
                        ExpirationMonth = c.String(nullable: false),
                        ExpirationYear = c.String(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        StripeAccount_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.StripeAccounts", t => t.StripeAccount_Id)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.StripeAccount_Id);
            
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
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        StripeCustomerId = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StripeId = c.String(maxLength: 50),
                        StripeCustomerId = c.String(maxLength: 50),
                        Date = c.DateTime(),
                        PeriodStart = c.DateTime(),
                        PeriodEnd = c.DateTime(),
                        Subtotal = c.Int(),
                        Total = c.Int(),
                        Attempted = c.Boolean(),
                        Closed = c.Boolean(),
                        Paid = c.Boolean(),
                        AttemptCount = c.Int(),
                        AmountDue = c.Int(),
                        StartingBalance = c.Int(),
                        EndingBalance = c.Int(),
                        NextPaymentAttempt = c.DateTime(),
                        Charge = c.Int(),
                        Discount = c.Int(),
                        ApplicationFee = c.Int(),
                        Currency = c.String(),
                        Customer_Id = c.String(maxLength: 128),
                        StripeAccount_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Customer_Id)
                .ForeignKey("dbo.StripeAccounts", t => t.StripeAccount_Id)
                .Index(t => t.StripeId, unique: true)
                .Index(t => t.StripeCustomerId)
                .Index(t => t.Paid)
                .Index(t => t.Customer_Id)
                .Index(t => t.StripeAccount_Id);
            
            CreateTable(
                "dbo.LineItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StripeLineItemId = c.String(),
                        Type = c.String(),
                        Amount = c.Int(),
                        Currency = c.String(),
                        Proration = c.Boolean(nullable: false),
                        Period_Start = c.DateTime(),
                        Period_End = c.DateTime(),
                        Quantity = c.Int(),
                        Plan_StripePlanId = c.String(),
                        Plan_Interval = c.String(),
                        Plan_Name = c.String(),
                        Plan_Created = c.DateTime(),
                        Plan_AmountInCents = c.Int(),
                        Plan_Currency = c.String(),
                        Plan_IntervalCount = c.Int(nullable: false),
                        Plan_TrialPeriodDays = c.Int(),
                        Plan_StatementDescription = c.String(),
                        Invoice_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
                .Index(t => t.Invoice_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        TrialStart = c.DateTime(),
                        TrialEnd = c.DateTime(),
                        SubscriptionPlanId = c.Int(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        StripeId = c.String(maxLength: 50),
                        StripeAccount_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StripeAccounts", t => t.StripeAccount_Id)
                .ForeignKey("dbo.SubscriptionPlans", t => t.SubscriptionPlanId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.SubscriptionPlanId)
                .Index(t => t.StripeId)
                .Index(t => t.StripeAccount_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.SubscriptionPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendlyId = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        Currency = c.String(),
                        Interval = c.Int(nullable: false),
                        TrialPeriodInDays = c.Int(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        StripeAccount_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StripeAccounts", t => t.StripeAccount_Id)
                .Index(t => t.StripeAccount_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CreditCards", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.Subscriptions", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subscriptions", "SubscriptionPlanId", "dbo.SubscriptionPlans");
            DropForeignKey("dbo.SubscriptionPlans", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.Subscriptions", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.StripeAccounts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Invoices", "StripeAccount_Id", "dbo.StripeAccounts");
            DropForeignKey("dbo.LineItems", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CreditCards", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SubscriptionPlans", new[] { "StripeAccount_Id" });
            DropIndex("dbo.Subscriptions", new[] { "User_Id" });
            DropIndex("dbo.Subscriptions", new[] { "StripeAccount_Id" });
            DropIndex("dbo.Subscriptions", new[] { "StripeId" });
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionPlanId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.LineItems", new[] { "Invoice_Id" });
            DropIndex("dbo.Invoices", new[] { "StripeAccount_Id" });
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            DropIndex("dbo.Invoices", new[] { "Paid" });
            DropIndex("dbo.Invoices", new[] { "StripeCustomerId" });
            DropIndex("dbo.Invoices", new[] { "StripeId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.StripeAccounts", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CreditCards", new[] { "StripeAccount_Id" });
            DropIndex("dbo.CreditCards", new[] { "ApplicationUserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SubscriptionPlans");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.LineItems");
            DropTable("dbo.Invoices");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.StripeAccounts");
            DropTable("dbo.CreditCards");
        }
    }
}
