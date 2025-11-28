using System.Security.Claims;
using Cartify.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cartify.Persistence
{
    internal class AuditInterceptor(IHttpContextAccessor accessor) : SaveChangesInterceptor
    {

        //we have to override both sync and async methods
        public override InterceptionResult<int> SavingChanges(DbContextEventData data,
            InterceptionResult<int> result)
        {
            ApplyAuditInformation(data.Context);
            return base.SavingChanges(data, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData data,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation(data.Context);
            return base.SavingChangesAsync(data, result, cancellationToken);
        }

        private string GetCurrentUser()
        {
            var httpContext = accessor.HttpContext;
            if (httpContext is null)
            {
                return "System";
            }
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                userId = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            }
            return "System";
        }

        private void ApplyAuditInformation(DbContext? context)
        {
            if (context is null)
            {
                return;
            }

            var entries = context
                .ChangeTracker
                .Entries<IAuditing<string>>()
                .Where(e => e.Entity is IAuditing<string>
                &&
                   (e.State == EntityState.Added ||
                   e.State == EntityState.Modified ||
                   e.State == EntityState.Deleted));

            if (!entries.Any()) return;

            var currentUser = GetCurrentUser();

            var now = DateTimeOffset.UtcNow;

            foreach (var entry in entries)
            {
                var auditableEntity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditableEntity.CreatedAtUtc = now;
                        auditableEntity.CreatedBy = currentUser;

                        break;

                    case EntityState.Modified:
                        // Only update if not already soft deleted
                        if (!auditableEntity.IsDeleted)
                        {
                            auditableEntity.UpdatedAtUtc = now;
                            auditableEntity.UpdatedBy = currentUser;
                        }

                        // Prevent modification of Created fields
                        entry.Property(e => e.CreatedAtUtc).IsModified = false;
                        entry.Property(e => e.CreatedBy).IsModified = false;
                        break;

                    case EntityState.Deleted:
                        // Soft delete: convert to modified
                        entry.State = EntityState.Modified;
                        auditableEntity.DeletedAtUtc = now;
                        auditableEntity.DeletedBy = currentUser;

                        // Prevent modification of Created fields
                        entry.Property(e => e.CreatedAtUtc).IsModified = false;
                        entry.Property(e => e.CreatedBy).IsModified = false;
                        break;
                }
            }
        }
    }
}
