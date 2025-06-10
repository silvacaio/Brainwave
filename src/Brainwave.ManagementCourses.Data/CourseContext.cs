using Brainwave.Core.Communication.Mediator;
using Brainwave.Core.Data;
using Microsoft.EntityFrameworkCore;
using Brainwave.Core.Messages;
using Brainwave.ManagementCourses.Domain;
using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementCourses.Data
{
    public class CourseContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CourseContext(DbContextOptions<CourseContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entityEntry in ChangeTracker.Entries<Entity>())
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }
                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property("CreatedAt").IsModified = false;
                }
                if (entityEntry.State == EntityState.Deleted)
                {
                    entityEntry.State = EntityState.Modified;
                    entityEntry.Property("CreatedAt").IsModified = false;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> Commit()
        {
            var isSuccess = await base.SaveChangesAsync() > 0;
            if (isSuccess) await _mediatorHandler.PublishEvents(this);
            return isSuccess;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new CourseConfiguration());
            //   modelBuilder.ApplyConfiguration(new LessonConfiguration());
            //            modelBuilder.ApplyConfiguration(new ProgressLessonsConfiguration());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties().Where(p => p.ClrType == typeof(string)))
                {
                    property.SetColumnType("varchar(100)");
                }
            }


            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }

}
