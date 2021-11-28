using SharedCodeLibrary.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SharedCodeLibrary.Services
{
    public abstract class DbOperatingService
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        protected DbOperatingService(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        protected async Task<TResult> WithConnectionTransaction<TResult>(Func<IDbConnection, IDbTransaction, Task<TResult>> func)
        {
            if (func == null)
                return default;

            if (_dbConnectionProvider != null)
            {
                using var connection = await _dbConnectionProvider.GetConnection().ConfigureAwait(false);

                if (connection != null)
                {
                    using var transaction = connection.BeginTransaction();

                    TResult result = await func(connection, transaction).ConfigureAwait(false);

                    transaction.Commit();

                    return result;
                }
            }
            
            return await func(null, null).ConfigureAwait(false);
        }

        protected async Task WithConnectionTransaction(Func<IDbConnection, IDbTransaction, Task> func)
        {
            if(_dbConnectionProvider != null)
            {
                using var connection = await _dbConnectionProvider.GetConnection().ConfigureAwait(false);

                if (connection != null)
                {
                    using var transaction = connection.BeginTransaction();

                    await func(connection, transaction).ConfigureAwait(false);

                    transaction.Commit();

                    return;
                }
            }
         
            await func(null, null).ConfigureAwait(false);
        }
    }
}
