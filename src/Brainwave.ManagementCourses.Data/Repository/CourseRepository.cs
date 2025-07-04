using Brainwave.Core.Data;
using Brainwave.ManagementCourses.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.ManagementCourses.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CoursesContext _context;
        protected readonly DbSet<Course> DbSet;
        protected readonly DbSet<Lesson> DbSetLesson;

        public CourseRepository(CoursesContext context)
        {
            _context = context;
            DbSet = _context.Set<Course>();
            DbSetLesson = _context.Set<Lesson>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Course course)
        {
            _context.Courses.Add(course);
        }

        public void Update(Course course)
        {
            _context.Courses.Update(course);
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _context.Courses
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Course?> GetById(Guid id, bool addLessons = false)
        {
            var query = _context.Courses
                .Include(c => c.Syllabus) // Importante: garante que o ValueObject seja carregado

                .AsQueryable();

            if (addLessons)
                query = query.Include(c => c.Lessons);

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Add(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
        }

        public void Update(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<Course?> GetByTitle(string title)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Title == title);
        }

        public void Delete(Course course)
        {
            _context.Attach(course);
            _context.Entry(course).State = EntityState.Deleted;

            DbSet.Remove(course);
        }

        public async Task<IEnumerable<Course>> GetCoursesNotIn(Guid[] enrolledCourseIds)
        {
            return await DbSet
                .AsNoTracking()
                .Where(c => !enrolledCourseIds.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<Lesson?> GetLessonByCourseIdAndTitle(Guid courseId, string title)
        {
            return await DbSetLesson
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.CourseId == courseId && l.Title == title);
        }

        public async Task<Lesson?> GetLessonByIdAndCourseId(Guid lessonId, Guid courseId)
        {
            return await DbSetLesson
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.CourseId == courseId && l.Id == lessonId);
        }
    }
}
