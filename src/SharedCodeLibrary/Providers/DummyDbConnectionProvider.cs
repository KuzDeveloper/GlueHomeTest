using SharedCodeLibrary.Interfaces;
using System.Data;
using System.Threading.Tasks;

namespace SharedCodeLibrary.Providers
{
    public class DummyDbConnectionProvider : IDbConnectionProvider
    {
        public Task<IDbConnection> GetConnection()
        {
            return Task.FromResult(default(IDbConnection));
        }
    }
}
