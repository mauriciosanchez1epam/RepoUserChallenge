using System.Data.Common;

namespace Application.Interfaces
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> CreateConnectionAsync();
    }
}
