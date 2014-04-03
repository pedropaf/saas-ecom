namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(),
                        End = c.DateTime(),
                        TrialStart = c.DateTime(),
                        TrialEnd = c.DateTime(),
                        SubscriptionPlan_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubscriptionPlans", t => t.SubscriptionPlan_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.SubscriptionPlan_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subscriptions", "SubscriptionPlan_Id", "dbo.SubscriptionPlans");
            DropIndex("dbo.Subscriptions", new[] { "User_Id" });
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionPlan_Id" });
            DropTable("dbo.Subscriptions");
        }
    }
}
