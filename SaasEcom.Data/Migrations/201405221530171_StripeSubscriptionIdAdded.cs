namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StripeSubscriptionIdAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "StripeId", c => c.String(maxLength: 50));
            CreateIndex("dbo.Subscriptions", "StripeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Subscriptions", new[] { "StripeId" });
            DropColumn("dbo.Subscriptions", "StripeId");
        }
    }
}
