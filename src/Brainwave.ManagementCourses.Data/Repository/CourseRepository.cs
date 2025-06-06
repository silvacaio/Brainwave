using Brainwave.Core.Data;
using Brainwave.ManagementCourses.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.ManagementCourses.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseContext _context;
        protected readonly DbSet<Course> DbSet;
        protected readonly DbSet<Lesson> DbSetLesson;


        public CourseRepository(CourseContext context)
        {
            _context = context;
            DbSet = _context.Set<Course>();
            DbSetLesson = _context.Set<Lesson>();

            DbSet.AsTracking();
            DbSetLesson.AsTracking();

        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Course Course)
        {
            _context.Courses.Add(Course);
        }

        public void Update(Course Course)
        {
            _context.Courses.Update(Course);
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetById(Guid id, bool addLessons = false)
        {
            if (addLessons == false)
                return await _context.Courses.FindAsync(id);


            return await _context.Courses
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == id);
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
            return await DbSet.FirstOrDefaultAsync(e => e.Title == title);

        }

        public void Delete(Course course)
        {
            DbSet.Remove(course);
        }

        public async Task<IEnumerable<Course>> GetCoursesNotIn(Guid[] enrolledCourseIds)
        {
            return DbSet.Where(c => !enrolledCourseIds.Contains(c.Id));
        }

        public async Task<Lesson?> GetLessonByCourseIdAndTitle(Guid courseId, string title)
        {
            return await DbSetLesson.Where(l => l.CourseId == courseId && l.Title == title).FirstOrDefaultAsync();
        }
    }
}
