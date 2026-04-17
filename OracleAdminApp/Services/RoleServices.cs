using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using OracleAdminApp.Helpers;
using OracleAdminApp.Models;

namespace OracleAdminApp.Services
{
    public static class RoleServices
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

        public static DataTable GetAllRoles(OracleDbConnection dbConnection)
        {
            // Đã cập nhật thành GetConnection()
            if (dbConnection == null || dbConnection.GetConnection() == null)
            {
                throw new Exception("Chưa kết nối đến cơ sở dữ liệu Oracle.");
            }

            DataTable dataTable = new DataTable();

            try
            {
                string query = "SELECT ROLE, ROLE_ID, PASSWORD_REQUIRED, AUTHENTICATION_TYPE FROM DBA_ROLES ORDER BY ROLE ASC";

                // Đã cập nhật thành GetConnection()
                using (OracleCommand command = new OracleCommand(query, dbConnection.GetConnection()))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi truy vấn DBA_ROLES: " + ex.Message);
            }

            return dataTable;
        }

/*
        public static List<OracleRole> GetAllRoles()
        {
            const string sql = @"SELECT ROLE, PASSWORD_REQUIRED, AUTHENTICATION_TYPE
                                 FROM DBA_ROLES
                                 ORDER BY ROLE";
            var dt = OracleHelper.ExecuteQuery(sql);

            var roles = new List<OracleRole>();
            foreach (DataRow row in dt.Rows)
            {
                roles.Add(new OracleRole
                {
                    RoleName = row["ROLE"]?.ToString(),
                    PasswordRequired = row["PASSWORD_REQUIRED"]?.ToString(),
                    Authentication = row["AUTHENTICATION_TYPE"]?.ToString()
                });
            }

            return roles;
        }

*/

        public static OracleRole GetRoleByName(string roleName)
        {
            ValidateIdentifier(roleName, nameof(roleName));
            const string sql = @"SELECT ROLE, PASSWORD_REQUIRED, AUTHENTICATION_TYPE
                                 FROM DBA_ROLES
                                 WHERE ROLE = :roleName";

            var parms = new[]
            {
                new OracleParameter(":roleName", OracleDbType.Varchar2) { Value = roleName.ToUpperInvariant() }
            };
            var dt = OracleHelper.ExecuteQuery(sql, parms);
            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new OracleRole
            {
                RoleName = row["ROLE"]?.ToString(),
                PasswordRequired = row["PASSWORD_REQUIRED"]?.ToString(),
                Authentication = row["AUTHENTICATION_TYPE"]?.ToString()
            };
        }

        public static void CreateRole(string roleName,
                                      string password = null,
                                      bool identifiedExternally = false,
                                      bool identifiedGlobally = false)
        {
            ValidateIdentifier(roleName, nameof(roleName));
            if (identifiedExternally && identifiedGlobally)
                throw new ArgumentException("Chỉ được chọn một trong IDENTIFIED EXTERNALLY hoặc IDENTIFIED GLOBALLY.");

            string authClause;
            if (!string.IsNullOrWhiteSpace(password))
            {
                authClause = $"IDENTIFIED BY \"{EscapePassword(password)}\"";
            }
            else if (identifiedExternally)
            {
                authClause = "IDENTIFIED EXTERNALLY";
            }
            else if (identifiedGlobally)
            {
                authClause = "IDENTIFIED GLOBALLY";
            }
            else
            {
                authClause = string.Empty;
            }

            string sql = string.IsNullOrWhiteSpace(authClause)
                ? $"CREATE ROLE {roleName.ToUpperInvariant()}"
                : $"CREATE ROLE {roleName.ToUpperInvariant()} {authClause}";

            OracleHelper.ExecuteNonQuery(sql);
        }

        public static void DropRole(string roleName)
        {
            ValidateIdentifier(roleName, nameof(roleName));
            string sql = $"DROP ROLE {roleName.ToUpperInvariant()}";
            OracleHelper.ExecuteNonQuery(sql);
        }

        public static List<string> GetRoleGrants(string roleName)
        {
            ValidateIdentifier(roleName, nameof(roleName));
            const string sql = @"SELECT GRANTEE, ADMIN_OPTION, DEFAULT_ROLE
                                 FROM DBA_ROLE_PRIVS
                                 WHERE GRANTED_ROLE = :roleName
                                 ORDER BY GRANTEE";
            var parms = new[]
            {
                new OracleParameter(":roleName", OracleDbType.Varchar2) { Value = roleName.ToUpperInvariant() }
            };
            var dt = OracleHelper.ExecuteQuery(sql, parms);
            var grants = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                grants.Add($"{row["GRANTEE"]} | ADMIN_OPTION={row["ADMIN_OPTION"]} | DEFAULT_ROLE={row["DEFAULT_ROLE"]}");
            }

            return grants;
        }

        public static List<string> GetRolesForGrantee(string grantee)
        {
            ValidateIdentifier(grantee, nameof(grantee));
            const string sql = @"SELECT GRANTED_ROLE
                                 FROM DBA_ROLE_PRIVS
                                 WHERE GRANTEE = :grantee
                                 ORDER BY GRANTED_ROLE";
            var parms = new[]
            {
                new OracleParameter(":grantee", OracleDbType.Varchar2) { Value = grantee.ToUpperInvariant() }
            };
            var dt = OracleHelper.ExecuteQuery(sql, parms);
            var roles = new List<string>();
            foreach (DataRow row in dt.Rows)
                roles.Add(row["GRANTED_ROLE"]?.ToString());

            return roles;
        }

        public static void GrantRole(string roleName, string grantee, bool adminOption = false)
        {
            ValidateIdentifier(roleName, nameof(roleName));
            ValidateIdentifier(grantee, nameof(grantee));
            string sql = $"GRANT {roleName.ToUpperInvariant()} TO {grantee.ToUpperInvariant()}";
            if (adminOption)
                sql += " WITH ADMIN OPTION";
            OracleHelper.ExecuteNonQuery(sql);
        }

        public static void RevokeRole(string roleName, string grantee)
        {
            ValidateIdentifier(roleName, nameof(roleName));
            ValidateIdentifier(grantee, nameof(grantee));
            string sql = $"REVOKE {roleName.ToUpperInvariant()} FROM {grantee.ToUpperInvariant()}";
            OracleHelper.ExecuteNonQuery(sql);
        }
    }
}
