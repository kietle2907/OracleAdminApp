namespace OracleAdminApp
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            usersToolStripMenuItem = new ToolStripMenuItem();
            rolesToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            tabPrivsToolStripMenuItem = new ToolStripMenuItem();
            colPrivsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            dataGridView1 = new DataGridView();
            panel1 = new Panel();
            gbRole = new GroupBox();
            btnEditRole = new Button();
            btnDeleteRole = new Button();
            btnAddRole = new Button();
            label3 = new Label();
            textBox3 = new TextBox();
            gbUser = new GroupBox();
            btnDeleteUser = new Button();
            btnEditUser = new Button();
            btnAddUser = new Button();
            label2 = new Label();
            label1 = new Label();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            sysPrivsToolStripMenuItem = new ToolStripMenuItem();
            phanQuyenNangCaoToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            gbRole.SuspendLayout();
            gbUser.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { usersToolStripMenuItem, rolesToolStripMenuItem, toolStripMenuItem1, tabPrivsToolStripMenuItem, colPrivsToolStripMenuItem, sysPrivsToolStripMenuItem, phanQuyenNangCaoToolStripMenuItem, exitToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(9, 3, 0, 3);
            menuStrip1.Size = new Size(1429, 35);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // usersToolStripMenuItem
            // 
            usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            usersToolStripMenuItem.Size = new Size(71, 29);
            usersToolStripMenuItem.Text = "Users";
            usersToolStripMenuItem.Click += usersToolStripMenuItem_Click;
            // 
            // rolesToolStripMenuItem
            // 
            rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            rolesToolStripMenuItem.Size = new Size(70, 29);
            rolesToolStripMenuItem.Text = "Roles";
            rolesToolStripMenuItem.Click += rolesToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(6, 29);
            // 
            // tabPrivsToolStripMenuItem
            // 
            tabPrivsToolStripMenuItem.Name = "tabPrivsToolStripMenuItem";
            tabPrivsToolStripMenuItem.Size = new Size(165, 29);
            tabPrivsToolStripMenuItem.Text = "Quyền đối tượng";
            tabPrivsToolStripMenuItem.Click += tabPrivsToolStripMenuItem_Click;
            // 
            // colPrivsToolStripMenuItem
            // 
            colPrivsToolStripMenuItem.Name = "colPrivsToolStripMenuItem";
            colPrivsToolStripMenuItem.Size = new Size(110, 29);
            colPrivsToolStripMenuItem.Text = "Quyền cột";
            colPrivsToolStripMenuItem.Click += colPrivsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(55, 29);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 718);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 20, 0);
            statusStrip1.Size = new Size(1429, 32);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(176, 25);
            toolStripStatusLabel1.Text = "Database Connected";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Left;
            dataGridView1.Location = new Point(0, 35);
            dataGridView1.Margin = new Padding(4, 5, 4, 5);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(739, 683);
            dataGridView1.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(gbRole);
            panel1.Controls.Add(gbUser);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(738, 35);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(691, 683);
            panel1.TabIndex = 3;
            // 
            // gbRole
            // 
            gbRole.Controls.Add(btnEditRole);
            gbRole.Controls.Add(btnDeleteRole);
            gbRole.Controls.Add(btnAddRole);
            gbRole.Controls.Add(label3);
            gbRole.Controls.Add(textBox3);
            gbRole.Dock = DockStyle.Right;
            gbRole.Location = new Point(4, 0);
            gbRole.Margin = new Padding(4, 5, 4, 5);
            gbRole.Name = "gbRole";
            gbRole.Padding = new Padding(4, 5, 4, 5);
            gbRole.Size = new Size(687, 683);
            gbRole.TabIndex = 7;
            gbRole.TabStop = false;
            gbRole.Text = "Thông tin Role";
            // 
            // btnEditRole
            // 
            btnEditRole.BackColor = Color.Yellow;
            btnEditRole.Location = new Point(394, 137);
            btnEditRole.Margin = new Padding(4, 5, 4, 5);
            btnEditRole.Name = "btnEditRole";
            btnEditRole.Size = new Size(107, 38);
            btnEditRole.TabIndex = 5;
            btnEditRole.Text = "Sửa";
            btnEditRole.UseVisualStyleBackColor = false;
            // 
            // btnDeleteRole
            // 
            btnDeleteRole.BackColor = Color.Red;
            btnDeleteRole.Location = new Point(524, 137);
            btnDeleteRole.Margin = new Padding(4, 5, 4, 5);
            btnDeleteRole.Name = "btnDeleteRole";
            btnDeleteRole.Size = new Size(107, 38);
            btnDeleteRole.TabIndex = 4;
            btnDeleteRole.Text = "Xóa";
            btnDeleteRole.UseVisualStyleBackColor = false;
            // 
            // btnAddRole
            // 
            btnAddRole.BackColor = SystemColors.ActiveCaption;
            btnAddRole.Location = new Point(259, 137);
            btnAddRole.Margin = new Padding(4, 5, 4, 5);
            btnAddRole.Name = "btnAddRole";
            btnAddRole.Size = new Size(107, 38);
            btnAddRole.TabIndex = 2;
            btnAddRole.Text = "Thêm";
            btnAddRole.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(56, 80);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(124, 25);
            label3.TabIndex = 1;
            label3.Text = "Nhập tên Role";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(223, 73);
            textBox3.Margin = new Padding(4, 5, 4, 5);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(141, 31);
            textBox3.TabIndex = 0;
            // 
            // gbUser
            // 
            gbUser.Controls.Add(btnDeleteUser);
            gbUser.Controls.Add(btnEditUser);
            gbUser.Controls.Add(btnAddUser);
            gbUser.Controls.Add(label2);
            gbUser.Controls.Add(label1);
            gbUser.Controls.Add(textBox2);
            gbUser.Controls.Add(textBox1);
            gbUser.Location = new Point(0, 0);
            gbUser.Margin = new Padding(4, 5, 4, 5);
            gbUser.Name = "gbUser";
            gbUser.Padding = new Padding(4, 5, 4, 5);
            gbUser.Size = new Size(691, 668);
            gbUser.TabIndex = 0;
            gbUser.TabStop = false;
            gbUser.Text = "Thông tin User";
            // 
            // btnDeleteUser
            // 
            btnDeleteUser.BackColor = Color.Red;
            btnDeleteUser.Location = new Point(539, 197);
            btnDeleteUser.Margin = new Padding(4, 5, 4, 5);
            btnDeleteUser.Name = "btnDeleteUser";
            btnDeleteUser.Size = new Size(107, 38);
            btnDeleteUser.TabIndex = 6;
            btnDeleteUser.Text = "Xóa";
            btnDeleteUser.UseVisualStyleBackColor = false;
            // 
            // btnEditUser
            // 
            btnEditUser.BackColor = Color.Yellow;
            btnEditUser.Location = new Point(394, 197);
            btnEditUser.Margin = new Padding(4, 5, 4, 5);
            btnEditUser.Name = "btnEditUser";
            btnEditUser.Size = new Size(107, 38);
            btnEditUser.TabIndex = 5;
            btnEditUser.Text = "Sửa";
            btnEditUser.UseVisualStyleBackColor = false;
            // 
            // btnAddUser
            // 
            btnAddUser.BackColor = SystemColors.ActiveCaption;
            btnAddUser.Location = new Point(247, 197);
            btnAddUser.Margin = new Padding(4, 5, 4, 5);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(107, 38);
            btnAddUser.TabIndex = 4;
            btnAddUser.Text = "Thêm";
            btnAddUser.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(60, 125);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(86, 25);
            label2.TabIndex = 3;
            label2.Text = "Mật khẩu";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(60, 63);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(129, 25);
            label1.TabIndex = 2;
            label1.Text = "Tên đăng nhập";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(223, 120);
            textBox2.Margin = new Padding(4, 5, 4, 5);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(141, 31);
            textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(223, 57);
            textBox1.Margin = new Padding(4, 5, 4, 5);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(141, 31);
            textBox1.TabIndex = 0;
            // 
            // sysPrivsToolStripMenuItem
            // 
            sysPrivsToolStripMenuItem.Name = "sysPrivsToolStripMenuItem";
            sysPrivsToolStripMenuItem.Size = new Size(157, 29);
            sysPrivsToolStripMenuItem.Text = "Quyền hệ thống";
            sysPrivsToolStripMenuItem.Click += sysPrivsToolStripMenuItem_Click;
            // 
            // phanQuyenNangCaoToolStripMenuItem
            //
            this.phanQuyenNangCaoToolStripMenuItem.Name = "phanQuyenNangCaoToolStripMenuItem";
            this.phanQuyenNangCaoToolStripMenuItem.Size = new Size(180, 29);
            this.phanQuyenNangCaoToolStripMenuItem.Text = "Phân quyền nâng cao";
            this.phanQuyenNangCaoToolStripMenuItem.Click += new EventHandler(this.phanQuyenNangCaoToolStripMenuItem_Click);
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1429, 750);
            Controls.Add(panel1);
            Controls.Add(dataGridView1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 5, 4, 5);
            Name = "FormMain";
            Text = "Oracle Admin - Main";
            FormClosing += FormMain_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            gbRole.ResumeLayout(false);
            gbRole.PerformLayout();
            gbUser.ResumeLayout(false);
            gbUser.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem usersToolStripMenuItem;
        private ToolStripMenuItem rolesToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private DataGridView dataGridView1;
        private Panel panel1;
        private GroupBox gbUser;
        private Label label2;
        private Label label1;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button btnDeleteUser;
        private Button btnEditUser;
        private Button btnAddUser;
        private GroupBox gbRole;
        private Label label3;
        private TextBox textBox3;
        private Button btnEditRole;
        private Button btnDeleteRole;
        private Button btnAddRole;
        private ToolStripMenuItem tabPrivsToolStripMenuItem;
        private ToolStripMenuItem colPrivsToolStripMenuItem;
        private ToolStripMenuItem sysPrivsToolStripMenuItem;
        private ToolStripMenuItem phanQuyenNangCaoToolStripMenuItem;
    }
}