using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.Common.Entities.Base
{
    public abstract class BaseApplicationEntity : BaseEntity<Guid>, IApplicationEntity
    {
        public BaseApplicationEntity()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.CreatedById = Guid.Empty; // TODO: set user id OnSave after implementation identity
            this.State = EntityState.Active;
        }


        [NotMapped]
        public string StateAsText
        {
            get
            {
                var value = 0 + this.State;
                switch (value)
                {
                    case EntityState.Active: return "Active";
                    case EntityState.Passive: return "Passive";
                    case EntityState.Deleted: return "Deleted";
                    default: return "Unknown";
                }
            }
            private set { /*for serilization*/ }
        }
        
        [NotMapped]
        public bool IsActive
        {
            get
            {
                return  this.State == EntityState.Active;
            }
            private set { /*for serilization*/ }
        }

        [NotMapped]
        public bool IsDeleted
        {
            get
            {
                return this.State == EntityState.Deleted;
            }
            private set { /*for serilization*/ }
        }
    }
}
