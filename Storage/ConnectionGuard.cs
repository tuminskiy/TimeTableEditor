using System;
using System.Data;

namespace TimeTable.Storage
{
    public class ConnectionGuard : IDisposable
    {
        private IDbConnection connection_;

        public ConnectionGuard(IDbConnection connection)
        {
            connection_ = connection;
            connection_.Open();
        }

        public void Dispose()
        {
            connection_.Close();
        }
    }
}
