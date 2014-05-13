namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionPlanUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubscriptionPlans", "Disabled", c => c.Boolean(nullable: false, defaultValue: false));
            DropColumn("dbo.SubscriptionPlans", "StatementDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubscriptionPlans", "StatementDescription", c => c.String());
            DropColumn("dbo.SubscriptionPlans", "Disabled");
        }
    }
}
