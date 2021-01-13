using PaymentSystem.Common.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;

namespace PaymentSystem.Common.Data
{
    public class PaymentSystemContext : IdentityDbContext<User, Role, Guid, UserLogin, UserRole, UserClaim>
    {
        public PaymentSystemContext() : base(ConfigurationManager.ConnectionStrings["PaymentSystem"].ConnectionString)
        {

        }

        static PaymentSystemContext()
        {
            // Database.SetInitializer<PaymentSystemContext>(new PaymentSystemInitializer());
        }

        public static PaymentSystemContext Create()
        {
            return new PaymentSystemContext();
        }


        #region Identity

        /*
        * User	 	Represents the user.
        * Role		Represents a role.
        * UserClaim	Represents a claim that a user possesses.
        * UserToken	Represents an authentication token for a user.
        * UserLogin	Associates a user with a login.
        * RoleClaim	Represents a claim that's granted to all users within a role.
        * UserRole 	A join entity that associates users and roles.
        */

        public virtual IDbSet<UserClaim> UserClaims { get; set; }
        /* public virtual IDbSet<UserToken> UserTokens { get; set; } */
        public virtual IDbSet<UserLogin> UserLogins { get; set; }
        /* public virtual IDbSet<RoleClaim> RoleClaims { get; set; } */
        public virtual IDbSet<UserRole> UserRoles { get; set; }

        #endregion

        #region PaymentSystem

        public virtual IDbSet<AccrualLoan> AccrualLoan { get; set; }

        public virtual IDbSet<CollectionRecord> CollectionRecords { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
