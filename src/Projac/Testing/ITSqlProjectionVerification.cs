using System.Data.SqlClient;

namespace Projac.Testing
{
    public interface ITSqlProjectionVerification
    {
        bool Verify(SqlTransaction transaction);
    }
}