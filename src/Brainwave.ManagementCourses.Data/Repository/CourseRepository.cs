using Brainwave.Core.Data;
using Brainwave.ManagementCourses.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.ManagementCourses.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseContext _context;

        public CourseRepository(CourseContext context)
        {
            _context = context;
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

        public async Task<Course?> GetById(Guid id)
        {
            return await _context.Courses.FindAsync(id);
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
    }
}
