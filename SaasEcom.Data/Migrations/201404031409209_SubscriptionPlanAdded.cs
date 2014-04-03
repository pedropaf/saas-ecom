namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionPlanAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubscriptionPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendlyId = c.String(),
                        Name = c.String(),
                        Price = c.Single(nullable: false),
                        Interval = c.Int(nullable: false),
                        TrialPeriodInDays = c.Int(nullable: false),
                        StatementDescription = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.SubscriptionPlans");
        }
    }
}
