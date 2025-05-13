using Brainwave.Core.Data;
using Brainwave.Curses.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.Curses.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseContext _context;

        public CourseRepository(CourseContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Course curse)
        {
            _context.Curses.Add(curse);
        }

        public void Update(Course curse)
        {
            _context.Curses.Update(curse);
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _context.Curses.ToListAsync();
        }

        public async Task<Course?> GetById(Guid id)
        {
            return await _context.Curses.FindAsync(id);
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
