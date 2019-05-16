namespace HelloWeb.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private HelloWebDbContext dbContext;

        public HelloWebDbContext Init()
        {
            return dbContext ?? (dbContext = new HelloWebDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}