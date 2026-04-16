using Oracle.ManagedDataAccess.Client;

public class OracleHelper
{
    private static string _connStr;

    public static bool Connect(string host, string port, 
                               string service, string user, string pass, bool isSysDba)
    {
        var dbaPriv = isSysDba ? ";DBA Privilege=SYSDBA" : "";
        _connStr = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)" +
                   $"(HOST={host})(PORT={port}))(CONNECT_DATA=(SERVICE_NAME={service})))" +
                   $";User Id={user};Password={pass}{dbaPriv};";
        using var conn = new OracleConnection(_connStr);
        conn.Open(); // ném exception nếu lỗi
        return true;
    }

    public static OracleConnection GetConnection() 
        => new OracleConnection(_connStr);

    public static DataTable ExecuteQuery(string sql, 
                                         OracleParameter[] parms = null)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new OracleCommand(sql, conn);
        if (parms != null) cmd.Parameters.AddRange(parms);
        var dt = new DataTable();
        new OracleDataAdapter(cmd).Fill(dt);
        return dt;
    }

    public static void ExecuteNonQuery(string sql, 
                                        OracleParameter[] parms = null)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = new OracleCommand(sql, conn);
        if (parms != null) cmd.Parameters.AddRange(parms);
        cmd.ExecuteNonQuery();
    }
}