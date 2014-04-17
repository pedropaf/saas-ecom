namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceDueDateRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Invoices", "DueDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invoices", "DueDate", c => c.DateTime(nullable: false));
        }
    }
}
