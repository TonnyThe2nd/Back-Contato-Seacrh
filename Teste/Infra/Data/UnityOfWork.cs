using System.Data;
using System.Transactions;
using Microsoft.Data.SqlClient;

namespace Teste.Infra.Data
{
    public class UnityOfWork : IUnityOfWork
    {
        private IDbConnection connection;
        private IDbTransaction transaction;
        public UnityOfWork() {
            connection = new SqlConnection(Configuration.Configurations.ConnectionString);
        }

        public IDbConnection GetConnection() => connection;
        public IDbTransaction GetTransaction() => transaction;
        public void BeginTransaction() => transaction = connection.BeginTransaction();

        public bool CommitTransaction()
        {
            if (transaction != null)
            {
                try
                {
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    transaction.Dispose();
                    transaction = null; 
                }
            }
            else
            {
                return false;
            }
        }

        public bool RollBack()
        {
            if (transaction == null) return false;
            try
            {
                transaction.Rollback();
                transaction = null;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
            if (connection == null) return;
            connection.Dispose();
            connection = null;
        }

    }
}
