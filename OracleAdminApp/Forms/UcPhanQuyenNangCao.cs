// =====================================================================
//  UcPhanQuyenNangCao.cs
//  UserControl phân quyền nâng cao – Thành viên 4.
//  Đã cập nhật để dùng PrivilegeServices (static) của team.
// =====================================================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using OracleAdminApp.Services;   // ← namespace của team

namespace OracleAdminApp.Forms    // ← đổi cho khớp folder dự kiến (Forms)
{
    public partial class UcPhanQuyenNangCao : UserControl
    {
        private OracleDbConnection? _db;

        public UcPhanQuyenNangCao()
        {
            InitializeComponent();

            cboObjectType.SelectedIndexChanged += (s, e) => OnObjectTypeChanged();
            cboSchema.SelectedIndexChanged += (s, e) => LoadObjects();
            cboObject.SelectedIndexChanged += (s, e) => LoadColumns();
            cboGrantee.SelectedIndexChanged += (s, e) => LoadCurrentPrivileges();
            clbPrivileges.ItemCheck += (s, e) => BeginInvoke((Action)UpdateColumnsEnabled);

            btnPreviewSql.Click += (s, e) => PreviewSql();
            btnGrant.Click += (s, e) => DoGrant();
            btnRevoke.Click += (s, e) => DoRevoke();
            btnRefresh.Click += (s, e) => LoadAll();
        }

        /// <summary>Gọi sau khi login thành công và đã có kết nối Oracle.</summary>
        public void Initialize(OracleDbConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            LoadAll();
        }

        // =================================================================
        //  Nạp dữ liệu
        // =================================================================

        private void LoadAll()
        {
            if (_db == null) return;

            cboObjectType.Items.Clear();
            cboObjectType.Items.AddRange(new object[]
                { ObjectKind.Table, ObjectKind.View,
                  ObjectKind.Procedure, ObjectKind.Function });
            cboObjectType.SelectedIndex = 0;

            var schemas = PrivilegeServices.GetUserSchemas(_db);
            cboSchema.DataSource = schemas;
            cboSchema.DisplayMember = "USERNAME";
            cboSchema.ValueMember = "USERNAME";

            var grantees = PrivilegeServices.GetGrantees(_db);
            grantees.Columns.Add("DISPLAY", typeof(string), "NAME + ' (' + KIND + ')'");
            cboGrantee.DataSource = grantees;
            cboGrantee.DisplayMember = "DISPLAY";
            cboGrantee.ValueMember = "NAME";

            OnObjectTypeChanged();
        }

        private void OnObjectTypeChanged()
        {
            if (_db == null) return;

            var kind = (ObjectKind)cboObjectType.SelectedItem!;
            clbPrivileges.Items.Clear();
            foreach (var p in PrivilegeServices.ValidPrivileges(kind))
                clbPrivileges.Items.Add(p, false);

            bool isRoutine = kind is ObjectKind.Procedure or ObjectKind.Function;
            grpColumns.Enabled = !isRoutine;
            clbSelectCols.Items.Clear();
            clbUpdateCols.Items.Clear();

            LoadObjects();
        }

        private void LoadObjects()
        {
            if (_db == null || cboSchema.SelectedValue == null) return;

            string owner = cboSchema.SelectedValue.ToString()!;
            var kind = (ObjectKind)cboObjectType.SelectedItem!;
            DataTable dt;
            string displayCol;

            switch (kind)
            {
                case ObjectKind.Table:
                    dt = PrivilegeServices.GetTables(_db, owner);
                    displayCol = "TABLE_NAME";
                    break;
                case ObjectKind.View:
                    dt = PrivilegeServices.GetViews(_db, owner);
                    displayCol = "OBJECT_NAME";
                    break;
                default:
                    dt = PrivilegeServices.GetRoutines(_db, owner, kind);
                    displayCol = "OBJECT_NAME";
                    break;
            }

            cboObject.DataSource = dt;
            cboObject.DisplayMember = displayCol;
            cboObject.ValueMember = displayCol;
        }

        private void LoadColumns()
        {
            clbSelectCols.Items.Clear();
            clbUpdateCols.Items.Clear();

            if (_db == null) return;
            var kind = (ObjectKind)cboObjectType.SelectedItem!;
            if (kind is ObjectKind.Procedure or ObjectKind.Function) return;
            if (cboSchema.SelectedValue == null || cboObject.SelectedValue == null) return;

            var cols = PrivilegeServices.GetColumns(
                _db,
                cboSchema.SelectedValue.ToString()!,
                cboObject.SelectedValue.ToString()!);

            foreach (DataRow r in cols.Rows)
            {
                string name = r["COLUMN_NAME"].ToString()!;
                clbSelectCols.Items.Add(name, false);
                clbUpdateCols.Items.Add(name, false);
            }

            UpdateColumnsEnabled();
        }

        private void UpdateColumnsEnabled()
        {
            var checkedPrivs = GetCheckedPrivileges();
            clbSelectCols.Enabled = checkedPrivs.Contains("SELECT");
            clbUpdateCols.Enabled = checkedPrivs.Contains("UPDATE");
        }

        private void LoadCurrentPrivileges()
        {
            if (_db == null || cboGrantee.SelectedValue == null) return;
            string grantee = cboGrantee.SelectedValue.ToString()!;

            // Dùng bản "ForGrantee" – lọc đúng theo GRANTEE.
            dgvTablePrivs.DataSource = PrivilegeServices.GetTablePrivilegesForGrantee(_db, grantee);
            dgvColumnPrivs.DataSource = PrivilegeServices.GetColumnPrivilegesForGrantee(_db, grantee);
        }

        // =================================================================
        //  Xử lý nút
        // =================================================================

        private void PreviewSql()
        {
            try
            {
                var req = BuildRequest();
                txtSqlPreview.Text = PrivilegeServices.BuildGrantSql(req) + ";";
            }
            catch (Exception ex)
            {
                txtSqlPreview.Text = "-- Lỗi: " + ex.Message;
            }
        }

        private void DoGrant()
        {
            if (_db == null) return;

            GrantRequest req;
            try { req = BuildRequest(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = PrivilegeServices.BuildGrantSql(req);
            if (MessageBox.Show($"Thực thi:\n\n{sql};\n\nTiếp tục?",
                    "Xác nhận GRANT", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) != DialogResult.OK) return;

            try
            {
                PrivilegeServices.Grant(_db, req);
                txtSqlPreview.Text = sql + ";   -- OK";
                LoadCurrentPrivileges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oracle trả lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSqlPreview.Text = sql + ";   -- LỖI: " + ex.Message;
            }
        }

        private void DoRevoke()
        {
            if (_db == null) return;

            GrantRequest req;
            try { req = BuildRequest(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool hasColumns = req.SelectColumns.Count > 0 || req.UpdateColumns.Count > 0;

            if (hasColumns)
            {
                var msg = "Oracle không cho REVOKE ở mức cột.\n\n" +
                          "YES: Hệ thống sẽ REVOKE toàn bộ privilege rồi " +
                          "GRANT lại trên các cột bạn đã tick.\n\n" +
                          "NO: REVOKE toàn bộ privilege (bỏ qua cột đã tick).";
                var ans = MessageBox.Show(msg, "Xử lý REVOKE mức cột",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (ans == DialogResult.Cancel) return;

                if (ans == DialogResult.Yes)
                {
                    try
                    {
                        if (req.Privileges.Contains("SELECT"))
                            PrivilegeServices.ReplaceColumnLevel(_db,
                                req.Schema, req.ObjectName, "SELECT",
                                req.SelectColumns, req.Grantee, req.WithGrantOption);
                        if (req.Privileges.Contains("UPDATE"))
                            PrivilegeServices.ReplaceColumnLevel(_db,
                                req.Schema, req.ObjectName, "UPDATE",
                                req.UpdateColumns, req.Grantee, req.WithGrantOption);

                        txtSqlPreview.Text = "-- Đã thực hiện REVOKE + GRANT lại mức cột.";
                        LoadCurrentPrivileges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Oracle trả lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                req.SelectColumns.Clear();
                req.UpdateColumns.Clear();
            }

            string sql = PrivilegeServices.BuildRevokeSql(req);
            if (MessageBox.Show($"Thực thi:\n\n{sql};\n\nTiếp tục?",
                    "Xác nhận REVOKE", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) != DialogResult.OK) return;

            try
            {
                PrivilegeServices.Revoke(_db, req);
                txtSqlPreview.Text = sql + ";   -- OK";
                LoadCurrentPrivileges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oracle trả lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSqlPreview.Text = sql + ";   -- LỖI: " + ex.Message;
            }
        }

        // =================================================================
        //  Helpers
        // =================================================================

        private GrantRequest BuildRequest()
        {
            if (cboObjectType.SelectedItem == null) throw new Exception("Chưa chọn loại đối tượng");
            if (cboSchema.SelectedValue == null) throw new Exception("Chưa chọn schema");
            if (cboObject.SelectedValue == null) throw new Exception("Chưa chọn đối tượng");
            if (cboGrantee.SelectedValue == null) throw new Exception("Chưa chọn grantee");

            var privs = GetCheckedPrivileges();
            if (privs.Count == 0) throw new Exception("Chưa chọn privilege nào");

            return new GrantRequest
            {
                Kind = (ObjectKind)cboObjectType.SelectedItem,
                Schema = cboSchema.SelectedValue.ToString()!,
                ObjectName = cboObject.SelectedValue.ToString()!,
                Grantee = cboGrantee.SelectedValue.ToString()!,
                Privileges = privs,
                SelectColumns = GetCheckedItems(clbSelectCols),
                UpdateColumns = GetCheckedItems(clbUpdateCols),
                WithGrantOption = chkWithGrantOption.Checked,
            };
        }

        private List<string> GetCheckedPrivileges()
        {
            var list = new List<string>();
            foreach (var item in clbPrivileges.CheckedItems) list.Add(item.ToString()!);
            return list;
        }

        private static List<string> GetCheckedItems(CheckedListBox clb)
        {
            var list = new List<string>();
            foreach (var item in clb.CheckedItems) list.Add(item.ToString()!);
            return list;
        }
    }
}