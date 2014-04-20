namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditCardStripeId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditCards", "StripeId", c => c.String());
            AddColumn("dbo.CreditCards", "StripeToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreditCards", "StripeToken");
            DropColumn("dbo.CreditCards", "StripeId");
        }
    }
}
