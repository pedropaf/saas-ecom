namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionPlanPriceToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubscriptionPlans", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubscriptionPlans", "Price", c => c.Single(nullable: false));
        }
    }
}
