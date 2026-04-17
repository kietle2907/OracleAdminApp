using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleAdminApp
{
    public class OracleDbConnection
    {
        private string _connectionString;

        public OracleDbConnection(string dataSource, string userId, string password)
        {
            // Build connection string
            _connectionString = $"Data Source={dataSource};User Id={userId};Password={password};";
        }

        public OracleConnection GetConnection()
        {
            return new OracleConnection(_connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using (OracleConnection conn = GetConnection())
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return false;
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OracleConnection conn = GetConnection())
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Query execution failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        public bool ExecuteCommand(string command)
        {
            try
            {
                using (OracleConnection conn = GetConnection())
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Command execution failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
