namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionPlanValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubscriptionPlans", "FriendlyId", c => c.String(nullable: false));
            AlterColumn("dbo.SubscriptionPlans", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubscriptionPlans", "Name", c => c.String());
            AlterColumn("dbo.SubscriptionPlans", "FriendlyId", c => c.String());
        }
    }
}
