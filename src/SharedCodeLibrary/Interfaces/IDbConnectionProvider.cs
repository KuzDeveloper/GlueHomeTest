using System.Data;
using System.Threading.Tasks;

namespace SharedCodeLibrary.Interfaces
{
    public interface IDbConnectionProvider
    {
        Task<IDbConnection> GetConnection();
    }
}
