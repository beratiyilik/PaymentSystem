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
    [Table("RoleClaim", Schema = "Identity")]
    public class RoleClaim : /* IdentityRoleClaim<Guid>, */ IApplicationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; protected set; }

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
