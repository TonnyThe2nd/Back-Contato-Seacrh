using System.Data;

namespace Teste.Infra.Data
{
    public interface IUnityOfWork : IDisposable
    {
        void BeginTransaction();
        bool CommitTransaction();
        bool RollBack();
        IDbConnection GetConnection();
        IDbTransaction GetTransaction();

    }
}
