namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionPlanCurrency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubscriptionPlans", "Currency", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubscriptionPlans", "Currency");
        }
    }
}
