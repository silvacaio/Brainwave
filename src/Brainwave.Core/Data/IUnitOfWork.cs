namespace Brainwave.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
