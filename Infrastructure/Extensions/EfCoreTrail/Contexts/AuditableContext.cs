using Microsoft.EntityFrameworkCore;

namespace SuperMarket.Infrastructure.Extensions.EfCoreTrail
{
    public abstract class AuditableContext : DbContext
    {
        public AuditableContext(DbContextOptions options) : base(options) { }

        public DbSet<Audit> Audits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.HasDefaultSchema("Tracking");
            builder.Entity<Audit>(entity =>
            {
                entity.ToTable(name: "Audits");
                entity.HasIndex(e => e.UserId).HasDatabaseName("IX_Audits_UserId");
            });
        }

        public virtual List<AuditModel> OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();

            var auditEntries = new List<AuditModel>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditModel(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries.Where(q => !q.HasTemporaryProperties))
            {
                Audits.Add(auditEntry.ToAudit());
            }

            return auditEntries.Where(q => q.HasTemporaryProperties).ToList();
        }

        public virtual async Task OnAfterSaveChanges(List<AuditModel> auditEntries, CancellationToken ct)
        {
            if (auditEntries == null || auditEntries.Count == 0) return;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                Audits.Add(auditEntry.ToAudit());
            }

            await base.SaveChangesAsync(ct);
        }
    }
}