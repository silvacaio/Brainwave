using Brainwave.Core.Data;
using Microsoft.EntityFrameworkCore;
using Brainwave.Core.Messages;
using Brainwave.ManagementCourses.Domain;
using Brainwave.Core.DomainObjects;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Brainwave.ManagementCourses.Data
{
    public class CoursesContext(DbContextOptions<CoursesContext> options,
                                      IMediator mediator) : DbContext(options), IUnitOfWork
    {

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
            if (isSuccess) await mediator.PublishEvents(this);
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

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoursesContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {

            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.Syllabus, syllabus =>
                {
                    syllabus.Property(s => s.Content)
                            .HasColumnName("SyllabusContent")
                            .IsRequired();

                    syllabus.Property(s => s.DurationInHours)
                            .HasColumnName("SyllabusDurationInHours")
                            .IsRequired();

                    syllabus.Property(s => s.Language)
                            .HasColumnName("SyllabusLanguage")
                            .IsRequired();
                });
        }
        
    }

}
