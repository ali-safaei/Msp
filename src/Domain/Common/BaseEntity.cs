using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public interface IBaseEntity
    { }

    public interface IPublished
    {
        bool Published { get; set; }
    }
    public interface ISoftDelete
    {
        bool Deleted { get; set; }
    }
    public interface ISeo
    {
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
    }

    public abstract class BaseEntity<TKey> : IBaseEntity
    {
        public TKey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<Guid>
    { }

    public abstract class AuditableEntity<TKey> : BaseEntity<TKey>
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedUtc { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedUtc { get; set; }

    }

    public abstract class AuditableEntity : AuditableEntity<Guid>
    { }
}
