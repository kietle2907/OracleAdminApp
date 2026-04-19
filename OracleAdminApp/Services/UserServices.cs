using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using OracleAdminApp.Models;

namespace OracleAdminApp.Services
{
    public static class UserServices
    {
        private static readonly Regex IdentifierValidator = new("^[A-Za-z][A-Za-z0-9_$#]*$");

        private static void ValidateIdentifier(string identifier, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Giá trị không được rỗng.", parameterName);

            if (!IdentifierValidator.IsMatch(identifier))
                throw new ArgumentException(
                    "Tên phải bắt đầu bằng chữ cái và chỉ chứa chữ cái, số, _, $ hoặc #.",
                    parameterName);
        }

        private static string EscapePassword(string password)
        {
            return password?.Replace("\"", "\"\"") ?? string.Empty;
        }

        public static DataTable GetAllUsers(OracleDbConnection dbConnection)
        {
            // Kiểm tra xem kết nối có tồn tại chưa
            if (dbConnection == null || dbConnection.GetConnection() == null)
            {
                throw new Exception("Chưa kết nối đến cơ sở dữ liệu Oracle.");
            }

            DataTable dataTable = new DataTable();

            try
            {
                // Câu lệnh SQL lấy danh sách toàn bộ User trong Oracle
                string query = "SELECT USERNAME, USER_ID, ACCOUNT_STATUS, CREATED, PROFILE FROM DBA_USERS ORDER BY CREATED DESC";

                // Tạo lệnh thực thi
                using (OracleCommand command = new OracleCommand(query, dbConnection.GetConnection()))
                {
                    // Dùng DataAdapter để tự động đọc dữ liệu và đổ vào DataTable
                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi truy vấn DBA_USERS: " + ex.Message);
            }

            return dataTable;
        }

        /// <summary>
        /// Lấy danh sách tất cả user từ DBA_USERS.
        /// </summary>
        public static DataTable GetAllUsersTable(OracleDbConnection dbConnection)
        {
            const string sql = @"
        SELECT USERNAME, USER_ID, ACCOUNT_STATUS, 
               DEFAULT_TABLESPACE, PROFILE,
               TO_CHAR(CREATED,'DD-MM-YYYY HH24:MI:SS') AS CREATED
        FROM   DBA_USERS
        ORDER  BY USERNAME";
            return dbConnection.ExecuteQuery(sql);
        }
        public static OracleUser? GetUserByName(OracleDbConnection dbConnection, string username)
        {
            ValidateIdentifier(username, nameof(username));
            const string sql = @"SELECT USERNAME, ACCOUNT_STATUS, DEFAULT_TABLESPACE, PROFILE,
                                       TO_CHAR(CREATED, 'DD-MON-YYYY HH24:MI:SS') CREATED
                                FROM DBA_USERS
                                WHERE USERNAME = :username";

            var parms = new[]
            {
                new OracleParameter(":username", OracleDbType.Varchar2) { Value = username.ToUpperInvariant() }
            };
            var dt = dbConnection.ExecuteQuery(sql, parms);
            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new OracleUser
            {
                Username = row["USERNAME"]?.ToString(),
                AccountStatus = row["ACCOUNT_STATUS"]?.ToString(),
                DefaultTablespace = row["DEFAULT_TABLESPACE"]?.ToString(),
                Profile = row["PROFILE"]?.ToString(),
                Created = row["CREATED"]?.ToString()
            };
        }

        public static void CreateUser(OracleDbConnection dbConnection, string username,
                                      string password,
                                      string defaultTablespace = "USERS",
                                      string profile = "DEFAULT")
        {
            ValidateIdentifier(username, nameof(username));
            ValidateIdentifier(defaultTablespace, nameof(defaultTablespace));
            ValidateIdentifier(profile, nameof(profile));

            string authClause = string.IsNullOrWhiteSpace(password)
                ? "IDENTIFIED EXTERNALLY"
                : $"IDENTIFIED BY \"{EscapePassword(password)}\"";

            string sql = $@"CREATE USER {username.ToUpperInvariant()} {authClause}
                            DEFAULT TABLESPACE {defaultTablespace.ToUpperInvariant()}
                            TEMPORARY TABLESPACE TEMP
                            PROFILE {profile.ToUpperInvariant()}";

            dbConnection.ExecuteCommand(sql);
        }

        public static void DropUser(OracleDbConnection dbConnection, string username, bool cascade = true)
        {
            ValidateIdentifier(username, nameof(username));
            string sql = cascade
                ? $"DROP USER {username.ToUpperInvariant()} CASCADE"
                : $"DROP USER {username.ToUpperInvariant()}";
            dbConnection.ExecuteCommand(sql);
        }

        public static void LockUser(OracleDbConnection dbConnection, string username)
        {
            ValidateIdentifier(username, nameof(username));
            string sql = $"ALTER USER {username.ToUpperInvariant()} ACCOUNT LOCK";
            dbConnection.ExecuteCommand(sql);
        }

        public static void UnlockUser(OracleDbConnection dbConnection, string username)
        {
            ValidateIdentifier(username, nameof(username));
            string sql = $"ALTER USER {username.ToUpperInvariant()} ACCOUNT UNLOCK";
            dbConnection.ExecuteCommand(sql);
        }

        public static void ChangePassword(OracleDbConnection dbConnection, string username, string newPassword)
        {
            ValidateIdentifier(username, nameof(username));
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Mật khẩu mới không được rỗng.", nameof(newPassword));

            string sql = $"ALTER USER {username.ToUpperInvariant()} IDENTIFIED BY \"{EscapePassword(newPassword)}\"";
            dbConnection.ExecuteCommand(sql);
        }

        public static List<string> GetUserRoles(OracleDbConnection dbConnection, string username)
        {
            ValidateIdentifier(username, nameof(username));
            const string sql = @"SELECT GRANTED_ROLE
                                 FROM DBA_ROLE_PRIVS
                                 WHERE GRANTEE = :username
                                 ORDER BY GRANTED_ROLE";
            var parms = new[]
            {
                new OracleParameter(":username", OracleDbType.Varchar2) { Value = username.ToUpperInvariant() }
            };

            var dt = dbConnection.ExecuteQuery(sql, parms);
            var roles = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                var role = row["GRANTED_ROLE"]?.ToString();
                if (!string.IsNullOrEmpty(role))
                    roles.Add(role);
            }

            return roles;
        }

        public static void GrantRoleToUser(OracleDbConnection dbConnection, string username, string roleName, bool adminOption = false)
        {
            ValidateIdentifier(username, nameof(username));
            ValidateIdentifier(roleName, nameof(roleName));
            string sql = $"GRANT {roleName.ToUpperInvariant()} TO {username.ToUpperInvariant()}";
            if (adminOption)
                sql += " WITH ADMIN OPTION";

            dbConnection.ExecuteCommand(sql);
        }

        public static void RevokeRoleFromUser(OracleDbConnection dbConnection, string username, string roleName)
        {
            ValidateIdentifier(username, nameof(username));
            ValidateIdentifier(roleName, nameof(roleName));
            string sql = $"REVOKE {roleName.ToUpperInvariant()} FROM {username.ToUpperInvariant()}";
            dbConnection.ExecuteCommand(sql);
        }
    }
}
