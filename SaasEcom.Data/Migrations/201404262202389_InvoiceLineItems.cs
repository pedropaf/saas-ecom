namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceLineItems : DbMigration
    {
        public override void Up()
        {
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
                        Plan_Created = c.DateTime(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LineItems", "Invoice_Id", "dbo.Invoices");
            DropIndex("dbo.LineItems", new[] { "Invoice_Id" });
            DropTable("dbo.LineItems");
        }
    }
}
