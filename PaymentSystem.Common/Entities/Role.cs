using Microsoft.AspNet.Identity.EntityFramework;
using PaymentSystem.Common.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.Common.Entities
{
    [Table("Role", Schema = "Identity")]
    public class Role : IdentityRole<Guid, UserRole>, IApplicationEntity
    {
        public Role()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.CreatedById = Guid.Empty; // TODO: set user id OnSave after implementation identity
            this.State = EntityState.Active;
        }

        public Role(string name) : this()
        {
            this.Name = name;
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


    }
}
