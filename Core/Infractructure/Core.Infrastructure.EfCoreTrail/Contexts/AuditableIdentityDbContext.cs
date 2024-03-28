using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Extensions.EfCoreTrail
{
    /// <summary>
    /// Clase abstracta que extiende funcionalidad para un contexto de base de datos de Identity, agregando 
    /// funcionalidad de Audit.
    /// </summary>
    public abstract class AuditableIdentityDbContext : IdentityDbContext<ApplicationUser,
                            ApplicationRole,
                            string,
                            IdentityUserClaim<string>,
                            IdentityUserRole<string>,
                            IdentityUserLogin<string>,
                            ApplicationRoleClaim,
                            IdentityUserToken<string>>
    {
        public AuditableIdentityDbContext(DbContextOptions options) : base(options) { }


        public DbSet<Audit> AuditLogs { get; set; }


        public virtual async Task<int> SaveChangesAsync(string? userId = null)
        {
            // Asignacion compuesta nueva en el lenguaje.
            // Supuestamente, usar lis nuevos atajos que se permiten, hacen escribir menos.
            // Yo no estoy de acuerdo con esto, sin embargo, parece que se no se usa, no eres
            // elegante, aunque tengas que mirar por internet qué significaba esto:
            userId ??= string.Empty;
            // Si pongo esto: 
            //    userId = userId ?? string.Empty;
            // puedes que te ahorres googlear
            // o incluso con esto:
            //   userid = userId != null ? userId : string.Empty;
            // no tengas que googlear!, pero alguien por ahí que no conozco dice que no eres elegante...

            var auditEntries = OnBeforeSaveChanges(userId);
            var result = await base.SaveChangesAsync();
            await OnAfterSaveChanges(auditEntries);
            return result;
        }

        private List<AuditModel> OnBeforeSaveChanges(string userId)
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
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<AuditModel> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

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
                AuditLogs.Add(auditEntry.ToAudit());
            }
            return SaveChangesAsync();
        }
    }
}