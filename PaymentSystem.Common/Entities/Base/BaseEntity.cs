using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.Common.Entities.Base
{
    public abstract class BaseEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; protected set; }

        public virtual TKey CreatedById { get; set; }

        private Nullable<DateTime> _createdAt = null;
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get => _createdAt ?? DateTime.UtcNow; set => _createdAt = value; }

        [DataType(DataType.DateTime)]
        public Nullable<DateTime> LastModifiedAt { get; set; }
        
        public virtual Nullable<TKey> LastModifiedById { get; set; }

        private Nullable<EntityState> _state = null;
        public EntityState State { get => _state ?? EntityState.Active; set => _state = value; }

        [Timestamp]
        public byte[] Version { get; set; }

        int? _requestedHashCode;
        public bool IsTransient()
        {
            return Id.Equals(default(TKey));
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseEntity<TKey>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (BaseEntity<TKey>)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item == this;
        }
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }
        public static bool operator ==(BaseEntity<TKey> left, BaseEntity<TKey> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }
        public static bool operator !=(BaseEntity<TKey> left, BaseEntity<TKey> right)
        {
            return !(left == right);
        }
    }
}
