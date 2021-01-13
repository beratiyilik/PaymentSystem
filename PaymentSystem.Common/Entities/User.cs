using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PaymentSystem.Common.Entities.Base;
using PaymentSystem.Common.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.Common.Entities
{
    [Table("User", Schema = "Identity")]
    public class User : IdentityUser<Guid, UserLogin, UserRole, UserClaim>, IApplicationEntity
    {
        public User()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.CreatedById = Guid.Empty; // TODO: set user id OnSave after implementation identity
            this.State = EntityState.Active;
            this.UserType = UserType.Membership;
            this.EntityType = EntityType.NaturalPerson;
            this.AccrualLoans = new List<AccrualLoan>();
        }

        public Guid CreatedById { get; set; }

        private Nullable<DateTime> _createdAt = null;
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get => _createdAt ?? DateTime.UtcNow; set => _createdAt = value; }

        public Nullable<Guid> LastModifiedById { get; set; }

        [DataType(DataType.DateTime)]
        public Nullable<DateTime> LastModifiedAt { get; set; }

        private Nullable<EntityState> _state = null;
        public EntityState State { get => _state ?? EntityState.Active; set => _state = value; }

        [Timestamp]
        public byte[] Version { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public string TaxNumber { get; set; }

        public UserType UserType { get; set; }

        public EntityType EntityType { get; set; }

        public virtual ICollection<AccrualLoan> AccrualLoans { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUserManager manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUserManager manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}
