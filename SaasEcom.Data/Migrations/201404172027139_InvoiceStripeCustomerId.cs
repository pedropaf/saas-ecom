namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceStripeCustomerId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "StripeCustomerId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "StripeCustomerId");
        }
    }
}
