﻿namespace SuperMarket.Domain.Abstractions
{
    public abstract class AuditableEntity : IAuditableBaseEntity
    {
        public int Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }
    }
}