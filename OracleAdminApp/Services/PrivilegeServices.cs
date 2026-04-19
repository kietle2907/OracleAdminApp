using Oracle.ManagedDataAccess.Client;
using OracleAdminApp;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace OracleAdminApp.Services
{
    public enum ObjectKind { Table, View, Procedure, Function }

    public class GrantRequest
    {
        public ObjectKind Kind { get; set; }
        public string Schema { get; set; } = "";
        public string ObjectName { get; set; } = "";
        public string Grantee { get; set; } = "";
        public bool WithGrantOption { get; set; }
        public List<string> Privileges { get; set; } = new();
        public List<string> SelectColumns { get; set; } = new();
        public List<string> UpdateColumns { get; set; } = new();
    }

    public static class PrivilegeServices
    {
        // Helper riêng — chạy query qua OracleDbConnection của team
        private static DataTable Run(OracleDbConnection db, string sql, OracleParameter[]? parms = null)
        {
            if (db == null)
                throw new Exception("Chưa kết nối đến cơ sở dữ liệu Oracle.");

            var dt = new DataTable();
            using var conn = db.GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(sql, conn);
            if (parms != null) cmd.Parameters.AddRange(parms);
            using var adapter = new OracleDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        private static int Exec(OracleDbConnection db, string sql)
        {
            if (db == null)
                throw new Exception("Chưa kết nối đến cơ sở dữ liệu Oracle.");

            using var conn = db.GetConnection();
            conn.Open();
            using var cmd = new OracleCommand(sql, conn);
            return cmd.ExecuteNonQuery();
        }

        // ========== QUYỀN TRÊN BẢNG/VIEW/PROC (Object-level) ==========

        public static DataTable GetAllTablePrivileges(OracleDbConnection db)
        {
            const string sql = @"
                SELECT GRANTEE, OWNER, TABLE_NAME, GRANTOR,
                       PRIVILEGE, GRANTABLE, HIERARCHY
                FROM   DBA_TAB_PRIVS
                WHERE  OWNER NOT IN ('SYS','SYSTEM','XDB','MDSYS','CTXSYS',
                                     'ORDSYS','WMSYS','APPQOSSYS','DBSNMP',
                                     'OUTLN','GSMADMIN_INTERNAL','AUDSYS',
                                     'DVSYS','LBACSYS','OLAPSYS','ORDDATA',
                                     'EXFSYS','ANONYMOUS','APEX_040000',
                                     'APEX_PUBLIC_USER','FLOWS_FILES')
                ORDER  BY GRANTEE, OWNER, TABLE_NAME";
            return Run(db, sql);
        }

        public static DataTable GetTablePrivilegesByGrantee(OracleDbConnection db, string grantee)
        {
            const string sql = @"
                SELECT GRANTEE, OWNER, TABLE_NAME, GRANTOR,
                       PRIVILEGE, GRANTABLE, HIERARCHY
                FROM   DBA_TAB_PRIVS
                WHERE  GRANTEE = :grantee
                ORDER  BY GRANTEE, TABLE_NAME";

            var parms = new[]
            {
                new OracleParameter(":grantee", OracleDbType.Varchar2)
                    { Value = grantee.ToUpperInvariant() }
            };
            return Run(db, sql, parms);
        }

        // ========== QUYỀN TRÊN CỘT (Column-level) ==========

        public static DataTable GetAllColumnPrivileges(OracleDbConnection db)
        {
            const string sql = @"
                SELECT GRANTEE, OWNER, TABLE_NAME, COLUMN_NAME,
                       GRANTOR, PRIVILEGE, GRANTABLE
                FROM   DBA_COL_PRIVS
                ORDER  BY GRANTEE, OWNER, TABLE_NAME, COLUMN_NAME";
            return Run(db, sql);
        }

        public static DataTable GetColumnPrivilegesByGrantee(OracleDbConnection db, string grantee)
        {
            const string sql = @"
                SELECT GRANTEE, OWNER, TABLE_NAME, COLUMN_NAME,
                       GRANTOR, PRIVILEGE, GRANTABLE
                FROM   DBA_COL_PRIVS
                WHERE  GRANTEE = :grantee
                ORDER  BY GRANTEE, TABLE_NAME, COLUMN_NAME";

            var parms = new[]
            {
                new OracleParameter(":grantee", OracleDbType.Varchar2)
                    { Value = grantee.ToUpperInvariant() }
            };
            return Run(db, sql, parms);
        }



        // ========== QUYỀN HỆ THỐNG ==========

        public static DataTable GetSystemPrivilegesByGrantee(OracleDbConnection db, string grantee)
        {
            const string sql = @"
                SELECT GRANTEE, PRIVILEGE, ADMIN_OPTION
                FROM   DBA_SYS_PRIVS
                WHERE  GRANTEE = :grantee
                ORDER  BY PRIVILEGE";

            var parms = new[]
            {
                new OracleParameter(":grantee", OracleDbType.Varchar2)
                    { Value = grantee.ToUpperInvariant() }
            };
            return Run(db, sql, parms);
        }
// =================================================================
//  ========== PHẦN BỔ SUNG – THÀNH VIÊN 4 ==========================
//  1) Liệt kê đối tượng / cột / user / role cho combobox.
//  2) Sinh & thực thi GRANT / REVOKE (có mức cột cho SELECT, UPDATE).
//  3) Đọc quyền hiện tại ĐÚNG theo GRANTEE.
// =================================================================

// ---- 1. LIỆT KÊ -------------------------------------------------

        public static DataTable GetUserSchemas(OracleDbConnection db)
        {
            // Lọc bỏ toàn bộ schema do Oracle tạo sẵn.
            const string sql = @"
                        SELECT USERNAME
                        FROM   DBA_USERS
                        WHERE  ORACLE_MAINTAINED = 'N'
                        ORDER  BY USERNAME";
            return Run(db, sql);
        }

        public static DataTable GetTables(OracleDbConnection db, string owner)
        {
            const string sql = @"
                        SELECT OWNER, TABLE_NAME
                        FROM   DBA_TABLES
                        WHERE  OWNER = :o
                        ORDER  BY TABLE_NAME";
            var parms = new[]
            {
                        new OracleParameter(":o", OracleDbType.Varchar2)
                            { Value = owner.ToUpperInvariant() }
                    };
            return Run(db, sql, parms);
        }

        public static DataTable GetViews(OracleDbConnection db, string owner)
        {
            const string sql = @"
                        SELECT OWNER, VIEW_NAME AS OBJECT_NAME
                        FROM   DBA_VIEWS
                        WHERE  OWNER = :o
                        ORDER  BY VIEW_NAME";
            var parms = new[]
            {
                        new OracleParameter(":o", OracleDbType.Varchar2)
                            { Value = owner.ToUpperInvariant() }
                    };
            return Run(db, sql, parms);
        }

        public static DataTable GetRoutines(OracleDbConnection db, string owner, ObjectKind kind)
        {
            string type = kind == ObjectKind.Procedure ? "PROCEDURE" : "FUNCTION";
            const string sql = @"
                        SELECT OWNER, OBJECT_NAME
                        FROM   DBA_OBJECTS
                        WHERE  OWNER = :o
                          AND  OBJECT_TYPE = :t
                          AND  STATUS = 'VALID'
                        ORDER  BY OBJECT_NAME";
            var parms = new[]
            {
                        new OracleParameter(":o", OracleDbType.Varchar2)
                            { Value = owner.ToUpperInvariant() },
                        new OracleParameter(":t", OracleDbType.Varchar2) { Value = type }
                    };
            return Run(db, sql, parms);
        }

        public static DataTable GetColumns(OracleDbConnection db, string owner, string objectName)
        {
            const string sql = @"
                        SELECT COLUMN_NAME, DATA_TYPE, NULLABLE
                        FROM   DBA_TAB_COLUMNS
                        WHERE  OWNER = :o AND TABLE_NAME = :t
                        ORDER  BY COLUMN_ID";
            var parms = new[]
            {
                        new OracleParameter(":o", OracleDbType.Varchar2)
                            { Value = owner.ToUpperInvariant() },
                        new OracleParameter(":t", OracleDbType.Varchar2)
                            { Value = objectName.ToUpperInvariant() }
                    };
            return Run(db, sql, parms);
        }

        public static DataTable GetGrantees(OracleDbConnection db)
        {
            const string sql = @"
                        SELECT USERNAME AS NAME, 'USER' AS KIND
                        FROM   DBA_USERS
                        WHERE  ORACLE_MAINTAINED = 'N'
                        UNION ALL
                        SELECT ROLE AS NAME, 'ROLE' AS KIND
                        FROM   DBA_ROLES
                        WHERE  ORACLE_MAINTAINED = 'N'
                        ORDER  BY KIND, NAME";
            return Run(db, sql);
        }

        // ---- 2. SINH & THỰC THI SQL --------------------------------------

        public static string[] ValidPrivileges(ObjectKind kind) => kind switch
        {
            ObjectKind.Table => new[] { "SELECT", "INSERT", "UPDATE", "DELETE",
                                                      "REFERENCES", "ALTER", "INDEX", "READ" },
            ObjectKind.View => new[] { "SELECT", "INSERT", "UPDATE", "DELETE",
                                                      "REFERENCES" },
            ObjectKind.Procedure => new[] { "EXECUTE", "DEBUG" },
            ObjectKind.Function => new[] { "EXECUTE", "DEBUG" },
            _ => Array.Empty<string>()
        };

        // ====== PHÂN QUYỀN ======
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

        private static readonly HashSet<string> KnownPrivilegeNames = new()
        {
            "SELECT", "INSERT", "UPDATE", "DELETE", "ALTER", "EXECUTE",
            "DEBUG", "UNDER", "FLASHBACK", "REFERENCES"
        };

        private static void ValidateObjectName(string objectName)
        {
            if (string.IsNullOrWhiteSpace(objectName))
                throw new ArgumentException("Tên object không được rỗng.", nameof(objectName));

            var parts = objectName.Split('.');
            if (parts.Length > 2)
                throw new ArgumentException("Tên object không hợp lệ (tối đa SCHEMA.TABLE).", nameof(objectName));

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part) || !IdentifierValidator.IsMatch(part))
                    throw new ArgumentException(
                        "Tên object chứa ký tự không hợp lệ.", nameof(objectName));
            }
        }

        public static void GrantPrivilege(OracleDbConnection db, string grantee, string privilege, string objectName, bool grantOption = false)
        {
            ValidateIdentifier(grantee, nameof(grantee));

            if (string.IsNullOrWhiteSpace(privilege))
                throw new ArgumentException("Privilege không được rỗng.", nameof(privilege));

            string privUpper = privilege.ToUpperInvariant();

            if (!KnownPrivilegeNames.Contains(privUpper))
                throw new ArgumentException($"Privilege không hợp lệ: {privilege}", nameof(privilege));

            ValidateObjectName(objectName);

            string sql = $"GRANT {privUpper} ON {objectName.ToUpperInvariant()} TO {grantee.ToUpperInvariant()}";

            if (grantOption)
                sql += " WITH GRANT OPTION";

            Exec(db, sql);
        }


        // ====== THU HỒI QUYỀN ======
        public static void RevokePrivilege(OracleDbConnection db, string grantee, string privilege, string objectName)
        {
            ValidateIdentifier(grantee, nameof(grantee));

            if (string.IsNullOrWhiteSpace(privilege))
                throw new ArgumentException("Privilege không được rỗng.", nameof(privilege));

            string privUpper = privilege.ToUpperInvariant();
            if (!KnownPrivilegeNames.Contains(privUpper))
                throw new ArgumentException($"Privilege không hợp lệ: {privilege}", nameof(privilege));

            ValidateObjectName(objectName);

            string sql = $"REVOKE {privUpper} ON {objectName.ToUpperInvariant()} FROM {grantee.ToUpperInvariant()}";

            Exec(db, sql);
        }

        // Đúng theo đề: chỉ SELECT và UPDATE mới được phân quyền mức cột.
        private static readonly HashSet<string> ColumnLevelAllowed =
            new() { "SELECT", "UPDATE" };

        public static string BuildGrantSql(GrantRequest r)
        {
            Validate(r);

            // Procedure / Function → GRANT EXECUTE / DEBUG, không có cột.
            if (r.Kind is ObjectKind.Procedure or ObjectKind.Function)
            {
                var sqlExec = new StringBuilder();
                sqlExec.Append("GRANT ")
                       .Append(string.Join(", ", r.Privileges))
                       .Append(" ON ").Append(Qualify(r.Schema, r.ObjectName))
                       .Append(" TO ").Append(QuoteIdent(r.Grantee));
                if (r.WithGrantOption) sqlExec.Append(" WITH GRANT OPTION");
                return sqlExec.ToString();
            }

            // Table / View – có thể kết hợp privilege toàn bảng và mức cột
            // trong cùng 1 câu lệnh: GRANT SELECT (c1,c2), INSERT, UPDATE (c3) ...
            var parts = new List<string>();
            foreach (var priv in r.Privileges)
            {
                if (priv == "SELECT" && r.SelectColumns.Any())
                    parts.Add($"SELECT ({JoinCols(r.SelectColumns)})");
                else if (priv == "UPDATE" && r.UpdateColumns.Any())
                    parts.Add($"UPDATE ({JoinCols(r.UpdateColumns)})");
                else
                    parts.Add(priv);
            }

            var sb = new StringBuilder();
            sb.Append("GRANT ").Append(string.Join(", ", parts))
              .Append(" ON ").Append(Qualify(r.Schema, r.ObjectName))
              .Append(" TO ").Append(QuoteIdent(r.Grantee));
            if (r.WithGrantOption) sb.Append(" WITH GRANT OPTION");
            return sb.ToString();
        }

        public static string BuildRevokeSql(GrantRequest r)
        {
            Validate(r, forRevoke: true);
            var sb = new StringBuilder();
            sb.Append("REVOKE ").Append(string.Join(", ", r.Privileges))
              .Append(" ON ").Append(Qualify(r.Schema, r.ObjectName))
              .Append(" FROM ").Append(QuoteIdent(r.Grantee));
            return sb.ToString();
        }

        public static int Grant(OracleDbConnection db, GrantRequest r)
            => Exec(db, BuildGrantSql(r));

        public static int Revoke(OracleDbConnection db, GrantRequest r)
            => Exec(db, BuildRevokeSql(r));

        /// <summary>
        /// Oracle KHÔNG hỗ trợ REVOKE ở mức cột. Muốn "thay đổi tập cột"
        /// đã cấp cho một privilege: REVOKE toàn bộ privilege đó rồi
        /// GRANT lại với tập cột mới (nếu còn).
        /// </summary>
        public static void ReplaceColumnLevel(OracleDbConnection db,
            string schema, string objectName, string privilege,
            List<string> newColumns, string grantee, bool withGrantOption)
        {
            ValidateObjectName(objectName);
            ValidateIdentifier(grantee, nameof(grantee));
            if (!ColumnLevelAllowed.Contains(privilege))
                throw new ArgumentException(
                    $"Privilege {privilege} không hỗ trợ mức cột.");

            // Bước 1: REVOKE privilege đó (kéo theo mọi cột hiện có).
            Exec(db,
                $"REVOKE {privilege} ON {Qualify(schema, objectName)} " +
                $"FROM {QuoteIdent(grantee)}");

            // Bước 2: GRANT lại với danh sách cột mới (nếu còn cột nào).
            if (newColumns is { Count: > 0 })
            {
                foreach (var col in newColumns)
                    ValidateIdentifier(col, nameof(newColumns));

                var sql = $"GRANT {privilege} ({JoinCols(newColumns)}) " +
                          $"ON {Qualify(schema, objectName)} " +
                          $"TO {QuoteIdent(grantee)}" +
                          (withGrantOption ? " WITH GRANT OPTION" : "");
                Exec(db, sql);
            }
        }

        // ---- 3. XEM QUYỀN HIỆN TẠI – LỌC ĐÚNG THEO GRANTEE --------------

        public static DataTable GetTablePrivilegesForGrantee(OracleDbConnection db, string grantee)
        {
            const string sql = @"
                        SELECT OWNER, TABLE_NAME AS OBJECT_NAME,
                               PRIVILEGE, GRANTABLE, GRANTOR
                        FROM   DBA_TAB_PRIVS
                        WHERE  GRANTEE = :g
                        ORDER  BY OWNER, TABLE_NAME, PRIVILEGE";
            var parms = new[]
            {
                        new OracleParameter(":g", OracleDbType.Varchar2)
                            { Value = grantee.ToUpperInvariant() }
                    };
            return Run(db, sql, parms);
        }

        public static DataTable GetColumnPrivilegesForGrantee(OracleDbConnection db, string grantee)
        {
            const string sql = @"
                        SELECT OWNER, TABLE_NAME AS OBJECT_NAME, COLUMN_NAME,
                               PRIVILEGE, GRANTABLE, GRANTOR
                        FROM   DBA_COL_PRIVS
                        WHERE  GRANTEE = :g
                        ORDER  BY OWNER, TABLE_NAME, COLUMN_NAME, PRIVILEGE";
            var parms = new[]
            {
                        new OracleParameter(":g", OracleDbType.Varchar2)
                            { Value = grantee.ToUpperInvariant() }
                    };
            return Run(db, sql, parms);
        }

        // =================================================================
        //  Private helpers
        // =================================================================

        private static void Validate(GrantRequest r, bool forRevoke = false)
        {
            if (r == null) throw new ArgumentNullException(nameof(r));
            ValidateIdentifier(r.Schema, nameof(r.Schema));
            ValidateIdentifier(r.ObjectName, nameof(r.ObjectName));
            ValidateIdentifier(r.Grantee, nameof(r.Grantee));
            if (r.Privileges == null || r.Privileges.Count == 0)
                throw new ArgumentException("Chưa chọn privilege");

            var allowed = new HashSet<string>(ValidPrivileges(r.Kind));
            foreach (var p in r.Privileges)
                if (!allowed.Contains(p))
                    throw new ArgumentException(
                        $"Privilege {p} không áp dụng cho {r.Kind}");

            if (!forRevoke && r.Kind is not (ObjectKind.Procedure or ObjectKind.Function))
            {
                if (r.SelectColumns is { Count: > 0 } && !r.Privileges.Contains("SELECT"))
                    throw new ArgumentException("Đã chọn cột cho SELECT nhưng chưa tick privilege SELECT");
                if (r.UpdateColumns is { Count: > 0 } && !r.Privileges.Contains("UPDATE"))
                    throw new ArgumentException("Đã chọn cột cho UPDATE nhưng chưa tick privilege UPDATE");
            }

            if (r.SelectColumns is { Count: > 0 })
            {
                foreach (var col in r.SelectColumns)
                    ValidateIdentifier(col, nameof(r.SelectColumns));
            }

            if (r.UpdateColumns is { Count: > 0 })
            {
                foreach (var col in r.UpdateColumns)
                    ValidateIdentifier(col, nameof(r.UpdateColumns));
            }
        }

        private static string QuoteIdent(string id)
        {
            if (string.IsNullOrEmpty(id)) return id;
            if (id.StartsWith("\"") && id.EndsWith("\"")) return id;
            return id.ToUpperInvariant();
        }

        private static string Qualify(string owner, string obj)
            => $"{QuoteIdent(owner)}.{QuoteIdent(obj)}";

        private static string JoinCols(IEnumerable<string> cols)
            => string.Join(", ", cols.Select(QuoteIdent));
            }
        }