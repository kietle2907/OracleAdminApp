using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OracleAdminApp.Services;

namespace OracleAdminApp
{
    public partial class FormMain : Form
    {
        private OracleDbConnection? _dbConnection;

        public FormMain()
        {
            InitializeComponent();
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
                var users = UserServices.GetAllUsers();

                // Bind to DataGridView
                dataGridView1.DataSource = users;
                dataGridView1.AutoResizeColumns();
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
                // Load roles from database
                var roles = RoleServices.GetAllRoles();

                // Bind to DataGridView
                dataGridView1.DataSource = roles;
                dataGridView1.AutoResizeColumns();
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
    }
}
