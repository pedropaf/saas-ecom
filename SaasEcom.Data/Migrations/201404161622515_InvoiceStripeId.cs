namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceStripeId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "StripeId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "StripeId");
        }
    }
}
