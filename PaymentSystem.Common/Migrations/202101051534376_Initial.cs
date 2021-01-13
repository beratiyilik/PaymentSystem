namespace PaymentSystem.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "PaymentSystem.AccrualLoan",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        MembershipId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpiryDate = c.DateTime(nullable: false),
                        RefNumber = c.String(),
                        Type = c.Int(nullable: false),
                        CreatedById = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedAt = c.DateTime(),
                        LastModifiedById = c.Guid(),
                        State = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Identity.User", t => t.MembershipId, cascadeDelete: true)
                .Index(t => t.MembershipId);
            
            CreateTable(
                "PaymentSystem.CollectionRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(),
                        AccrualLoanId = c.Guid(nullable: false),
                        IsRefund = c.Boolean(nullable: false),
                        CreatedById = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedAt = c.DateTime(),
                        LastModifiedById = c.Guid(),
                        State = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PaymentSystem.AccrualLoan", t => t.AccrualLoanId, cascadeDelete: true)
                .Index(t => t.AccrualLoanId);
            
            CreateTable(
                "Identity.User",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedById = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(),
                        LastModifiedAt = c.DateTime(),
                        State = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        FirstName = c.String(),
                        LastName = c.String(),
                        NationalIdentificationNumber = c.String(),
                        TaxNumber = c.String(),
                        UserType = c.Int(nullable: false),
                        EntityType = c.Int(nullable: false),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Identity.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedById = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(),
                        LastModifiedAt = c.DateTime(),
                        State = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Identity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Identity.UserLogin",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreatedById = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(),
                        LastModifiedAt = c.DateTime(),
                        State = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Identity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Identity.UserRole",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreatedById = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(),
                        LastModifiedAt = c.DateTime(),
                        State = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Identity.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Identity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Identity.Role",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedById = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastModifiedById = c.Guid(),
                        LastModifiedAt = c.DateTime(),
                        State = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Identity.UserRole", "UserId", "Identity.User");
            DropForeignKey("Identity.UserRole", "RoleId", "Identity.Role");
            DropForeignKey("Identity.UserLogin", "UserId", "Identity.User");
            DropForeignKey("Identity.UserClaim", "UserId", "Identity.User");
            DropForeignKey("PaymentSystem.AccrualLoan", "MembershipId", "Identity.User");
            DropForeignKey("PaymentSystem.CollectionRecord", "AccrualLoanId", "PaymentSystem.AccrualLoan");
            DropIndex("Identity.UserRole", new[] { "RoleId" });
            DropIndex("Identity.UserRole", new[] { "UserId" });
            DropIndex("Identity.UserLogin", new[] { "UserId" });
            DropIndex("Identity.UserClaim", new[] { "UserId" });
            DropIndex("PaymentSystem.CollectionRecord", new[] { "AccrualLoanId" });
            DropIndex("PaymentSystem.AccrualLoan", new[] { "MembershipId" });
            DropTable("Identity.Role");
            DropTable("Identity.UserRole");
            DropTable("Identity.UserLogin");
            DropTable("Identity.UserClaim");
            DropTable("Identity.User");
            DropTable("PaymentSystem.CollectionRecord");
            DropTable("PaymentSystem.AccrualLoan");
        }
    }
}
