namespace SaasEcom.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIpAddressAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IPAddress", c => c.String());
            AddColumn("dbo.AspNetUsers", "IPAddressCountry", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IPAddressCountry");
            DropColumn("dbo.AspNetUsers", "IPAddress");
        }
    }
}
