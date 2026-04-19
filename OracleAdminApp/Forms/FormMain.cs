using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OracleAdminApp.Services;
using Microsoft.VisualBasic;
using OracleAdminApp.Forms;

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

            // Đăng ký sự kiện Click cho User
            btnAddUser.Click += btnAddUser_Click;
            btnEditUser.Click += btnEditUser_Click;
            btnDeleteUser.Click += btnDeleteUser_Click;

            // Đăng ký sự kiện Click cho Role
            btnAddRole.Click += btnAddRole_Click;
            btnEditRole.Click += btnEditRole_Click;
            btnDeleteRole.Click += btnDeleteRole_Click;

            // Đăng ký sự kiện click cho DataGridView
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        public FormMain(OracleDbConnection dbConnection) : this()
        {
            _dbConnection = dbConnection;
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
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

            // Ẩn UC nâng cao nếu đang hiển thị
            if (_ucAdvancedPrivs != null) _ucAdvancedPrivs.Visible = false;

            // Hiện lại giao diện cũ
            dataGridView1.Visible = true;
            panel1.Visible = true;

            try
            {
                var users = UserServices.GetAllUsers(_dbConnection);
                dataGridView1.DataSource = users;
                dataGridView1.AutoResizeColumns();
                gbUser.Visible = true;
                gbRole.Visible = false;
            }
            catch (Exception ex) {  }
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Nếu đang mở tab User
                if (gbUser.Visible)
                {
                    textBox1.Text = row.Cells["USERNAME"].Value?.ToString();
                    textBox2.Text = ""; // Xóa rỗng mật khẩu để đảm bảo an toàn
                }
                // Nếu đang mở tab Role
                else if (gbRole.Visible)
                {
                    textBox3.Text = row.Cells["ROLE"].Value?.ToString();
                }
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đủ Tên đăng nhập và Mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = $"CREATE USER {username} IDENTIFIED BY \"{password}\"";
            if (_dbConnection != null && _dbConnection.ExecuteCommand(sql))
            {
                MessageBox.Show("Thêm User thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                usersToolStripMenuItem_Click(null, null); // Load lại bảng User
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng chọn User và nhập Mật khẩu mới để thay đổi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chức năng Sửa User chủ yếu là thay đổi mật khẩu
            string sql = $"ALTER USER {username} IDENTIFIED BY \"{password}\"";
            if (_dbConnection != null && _dbConnection.ExecuteCommand(sql))
            {
                MessageBox.Show("Đổi mật khẩu User thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                usersToolStripMenuItem_Click(null, null); // Load lại bảng User
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng chọn User cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa user '{username}' và toàn bộ schema của user này (CASCADE)?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = $"DROP USER {username} CASCADE";
                if (_dbConnection != null && _dbConnection.ExecuteCommand(sql))
                {
                    MessageBox.Show("Xóa User thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Clear();
                    textBox2.Clear();
                    usersToolStripMenuItem_Click(null, null); // Load lại bảng User
                }
            }
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            string roleName = textBox3.Text.Trim();
            if (string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Vui lòng nhập tên Role!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = $"CREATE ROLE {roleName}";
            if (_dbConnection != null && _dbConnection.ExecuteCommand(sql))
            {
                MessageBox.Show("Thêm Role thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rolesToolStripMenuItem_Click(null, null); // Load lại bảng Role
            }
        }

        private void btnEditRole_Click(object sender, EventArgs e)
        {
            // Trong Oracle, Role không có nhiều thuộc tính để "Sửa" như Tên. 
            // Thông thường nếu sai tên, người quản trị sẽ Xóa đi và Tạo lại.
            MessageBox.Show("Trong Oracle, Role thường chỉ được cấp/thu hồi quyền chứ không thể dễ dàng đổi tên (RENAME).\n\nNếu bạn muốn đổi tên Role, vui lòng Xóa Role hiện tại và Thêm Role mới.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteRole_Click(object sender, EventArgs e)
        {
            string roleName = textBox3.Text.Trim();
            if (string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Vui lòng chọn Role cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa role '{roleName}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = $"DROP ROLE {roleName}";
                if (_dbConnection != null && _dbConnection.ExecuteCommand(sql))
                {
                    MessageBox.Show("Xóa Role thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox3.Clear();
                    rolesToolStripMenuItem_Click(null, null); // Load lại bảng Role
                }
            }
        }

        private UcPhanQuyenNangCao? _ucAdvancedPrivs;

        private void phanQuyenNangCaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_dbConnection == null)
            {
                MessageBox.Show("Chưa kết nối database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Ẩn các giao diện cũ để lấy không gian hiển thị
            dataGridView1.Visible = false;
            panel1.Visible = false;

            // 2. Khởi tạo UserControl nếu chưa có
            if (_ucAdvancedPrivs == null)
            {
                _ucAdvancedPrivs = new UcPhanQuyenNangCao();
                _ucAdvancedPrivs.Dock = DockStyle.Fill;

                // Thêm vào danh sách control của Form
                this.Controls.Add(_ucAdvancedPrivs);

                // Đưa lên lớp trên cùng để không bị che khuất
                _ucAdvancedPrivs.BringToFront();

                // Quan trọng: Gọi hàm Initialize để nạp dữ liệu từ DB vào các Combobox
                _ucAdvancedPrivs.Initialize(_dbConnection);
            }
            else
            {
                _ucAdvancedPrivs.Visible = true;
                _ucAdvancedPrivs.BringToFront();
            }

            toolStripStatusLabel1.Text = "Đang ở chế độ Phân quyền nâng cao";
        }
    }
}
