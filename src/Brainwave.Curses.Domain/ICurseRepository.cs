using Brainwave.Core.Data;

namespace Brainwave.Curses.Domain
{
    public interface ICurseRepository : IRepository<Curse>
    {
        Task<IEnumerable<Curse>> GetAll();
        Task<Curse?> GetById(Guid id);

        void Add(Curse curse);
        void Update(Curse curse);

        void Add(Lesson lesson);
        void Update(Lesson lesson);
    }
}
