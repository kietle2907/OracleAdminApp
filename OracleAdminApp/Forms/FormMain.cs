using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OracleAdminApp.Services;
using Microsoft.VisualBasic;

namespace OracleAdminApp
{
    public partial class FormMain : Form
    {
        private OracleDbConnection? _dbConnection;

        public FormMain()
        {
            InitializeComponent();

            gbUser.Visible = false;
            gbRole.Visible = false;
        }

        public FormMain(OracleDbConnection dbConnection) : this()
        {
            _dbConnection = dbConnection;
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dbConnection == null)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Load users from database
                var users = UserServices.GetAllUsers(_dbConnection);

                // Bind to DataGridView
                dataGridView1.DataSource = users;
                dataGridView1.AutoResizeColumns();

                gbUser.Visible = true;  // Hiện khung nhập User
                gbRole.Visible = false; // Ẩn khung nhập Role
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách người dùng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dbConnection == null)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Load roles from database
                var roles = RoleServices.GetAllRoles(_dbConnection);

                // Bind to DataGridView
                dataGridView1.DataSource = roles;
                dataGridView1.AutoResizeColumns();

                gbRole.Visible = true;  // Hiện khung nhập Role
                gbUser.Visible = false; // Ẩn khung nhập User
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách role: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close database connection when form closes
            _dbConnection = null;

            // Exit the entire application
            Application.Exit();
        }

        private void tabPrivsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dbConnection == null)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Hỏi người dùng muốn xem tất cả hay của riêng 1 user/role
                var result = MessageBox.Show(
                    "Bạn muốn xem quyền của 1 user/role cụ thể?\n\n" +
                    "• YES → Nhập tên User/Role\n" +
                    "• NO  → Xem tất cả",
                    "Truy vấn DBA_TAB_PRIVS",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                DataTable data;

                if (result == DialogResult.Yes)
                {
                    // Hiển thị InputBox lấy tên grantee
                    string grantee = Microsoft.VisualBasic.Interaction.InputBox(
                        "Nhập tên USER hoặc ROLE cần xem quyền:",
                        "Chọn Grantee",
                        "HR");

                    if (string.IsNullOrWhiteSpace(grantee))
                        return;

                    data = PrivilegeServices.GetTablePrivilegesByGrantee(_dbConnection, grantee);
                }
                else if (result == DialogResult.No)
                {
                    data = PrivilegeServices.GetAllTablePrivileges(_dbConnection);
                }
                else
                {
                    return; // Cancel
                }

                // Bind dữ liệu
                dataGridView1.DataSource = data;
                dataGridView1.AutoResizeColumns();

                // Ẩn 2 groupbox user/role vì bây giờ đang xem privileges
                gbUser.Visible = false;
                gbRole.Visible = false;

                toolStripStatusLabel1.Text = $"DBA_TAB_PRIVS: {data.Rows.Count} dòng";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải DBA_TAB_PRIVS: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void colPrivsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dbConnection == null)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    "Bạn muốn xem quyền cột của 1 user/role cụ thể?\n\n" +
                    "• YES → Nhập tên User/Role\n" +
                    "• NO  → Xem tất cả",
                    "Truy vấn DBA_COL_PRIVS",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                DataTable data;

                if (result == DialogResult.Yes)
                {
                    string grantee = Microsoft.VisualBasic.Interaction.InputBox(
                        "Nhập tên USER hoặc ROLE cần xem quyền cột:",
                        "Chọn Grantee",
                        "");

                    if (string.IsNullOrWhiteSpace(grantee))
                        return;

                    data = PrivilegeServices.GetColumnPrivilegesByGrantee(_dbConnection, grantee);
                }
                else if (result == DialogResult.No)
                {
                    data = PrivilegeServices.GetAllColumnPrivileges(_dbConnection);
                }
                else
                {
                    return;
                }

                dataGridView1.DataSource = data;
                dataGridView1.AutoResizeColumns();

                gbUser.Visible = false;
                gbRole.Visible = false;

                toolStripStatusLabel1.Text = $"DBA_COL_PRIVS: {data.Rows.Count} dòng";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải DBA_COL_PRIVS: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sysPrivsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dbConnection == null)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string grantee = Microsoft.VisualBasic.Interaction.InputBox(
                    "Nhập tên USER hoặc ROLE cần xem quyền hệ thống:",
                    "Xem DBA_SYS_PRIVS",
                    "HR");

                if (string.IsNullOrWhiteSpace(grantee))
                    return;

                DataTable data = PrivilegeServices.GetSystemPrivilegesByGrantee(_dbConnection, grantee);

                dataGridView1.DataSource = data;
                dataGridView1.AutoResizeColumns();

                gbUser.Visible = false;
                gbRole.Visible = false;

                toolStripStatusLabel1.Text = $"DBA_SYS_PRIVS của {grantee.ToUpper()}: {data.Rows.Count} dòng";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải DBA_SYS_PRIVS: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
