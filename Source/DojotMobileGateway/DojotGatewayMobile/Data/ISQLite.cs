using SQLite;

namespace DojotGatewayMobile.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
