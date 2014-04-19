namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditCardValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CreditCards", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.CreditCards", "AddressCity", c => c.String(nullable: false));
            AlterColumn("dbo.CreditCards", "AddressCountry", c => c.String(nullable: false));
            AlterColumn("dbo.CreditCards", "AddressLine1", c => c.String(nullable: false));
            AlterColumn("dbo.CreditCards", "AddressZip", c => c.String(nullable: false));
            AlterColumn("dbo.CreditCards", "Cvc", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.CreditCards", "ExpirationMonth", c => c.String(nullable: false));
            AlterColumn("dbo.CreditCards", "ExpirationYear", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CreditCards", "ExpirationYear", c => c.String());
            AlterColumn("dbo.CreditCards", "ExpirationMonth", c => c.String());
            AlterColumn("dbo.CreditCards", "Cvc", c => c.String(maxLength: 4));
            AlterColumn("dbo.CreditCards", "AddressZip", c => c.String());
            AlterColumn("dbo.CreditCards", "AddressLine1", c => c.String());
            AlterColumn("dbo.CreditCards", "AddressCountry", c => c.String());
            AlterColumn("dbo.CreditCards", "AddressCity", c => c.String());
            AlterColumn("dbo.CreditCards", "Name", c => c.String());
        }
    }
}
