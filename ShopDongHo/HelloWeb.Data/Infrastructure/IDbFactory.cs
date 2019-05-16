using HelloWeb.Data;
using System;

namespace HelloWeb.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        HelloWebDbContext Init();
    }
}