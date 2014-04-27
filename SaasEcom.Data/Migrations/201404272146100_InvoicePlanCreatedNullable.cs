namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoicePlanCreatedNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LineItems", "Plan_Created", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LineItems", "Plan_Created", c => c.DateTime(nullable: false));
        }
    }
}
