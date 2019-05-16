namespace HelloWeb.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}