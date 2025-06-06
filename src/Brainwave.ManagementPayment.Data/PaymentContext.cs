using Brainwave.Core.Communication.Mediator;
using Brainwave.Core.Data;
using Brainwave.Core.Messages;
using Brainwave.ManagementPayment.Application;
using Brainwave.ManagementPayment.Business;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.ManagementPayment.Data
{
    public class PaymentContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public PaymentContext(DbContextOptions<PaymentContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties().Where(p => p.ClrType == typeof(string)))
                {
                    property.SetColumnType("varchar(100)");
                }
            }
            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedAt").IsModified = false;
                }
            }

            var isSuccess = await base.SaveChangesAsync() > 0;
            if (isSuccess) await _mediatorHandler.PublishEvents(this);
            return isSuccess;
        }
    }
}
