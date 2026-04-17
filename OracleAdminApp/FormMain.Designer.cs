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
            exitToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            dataGridView1 = new DataGridView();
            panel1 = new Panel();
            gbUser = new GroupBox();
            gbRole = new GroupBox();
            btnEditRole = new Button();
            btnDeleteRole = new Button();
            btnAddRole = new Button();
            label3 = new Label();
            textBox3 = new TextBox();
            btnDeleteUser = new Button();
            btnEditUser = new Button();
            btnAddUser = new Button();
            label2 = new Label();
            label1 = new Label();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            gbUser.SuspendLayout();
            gbRole.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { usersToolStripMenuItem, rolesToolStripMenuItem, toolStripMenuItem1, exitToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1000, 27);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // usersToolStripMenuItem
            // 
            usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            usersToolStripMenuItem.Size = new Size(47, 23);
            usersToolStripMenuItem.Text = "Users";
            usersToolStripMenuItem.Click += usersToolStripMenuItem_Click;
            // 
            // rolesToolStripMenuItem
            // 
            rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            rolesToolStripMenuItem.Size = new Size(47, 23);
            rolesToolStripMenuItem.Text = "Roles";
            rolesToolStripMenuItem.Click += rolesToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(6, 23);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(37, 23);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1000, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(116, 17);
            toolStripStatusLabel1.Text = "Database Connected";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Left;
            dataGridView1.Location = new Point(0, 27);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(517, 401);
            dataGridView1.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(gbRole);
            panel1.Controls.Add(gbUser);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(516, 27);
            panel1.Name = "panel1";
            panel1.Size = new Size(484, 401);
            panel1.TabIndex = 3;
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
            gbUser.Name = "gbUser";
            gbUser.Size = new Size(484, 401);
            gbUser.TabIndex = 0;
            gbUser.TabStop = false;
            gbUser.Text = "Thông tin User";
            // 
            // gbRole
            // 
            gbRole.Controls.Add(btnEditRole);
            gbRole.Controls.Add(btnDeleteRole);
            gbRole.Controls.Add(btnAddRole);
            gbRole.Controls.Add(label3);
            gbRole.Controls.Add(textBox3);
            gbRole.Dock = DockStyle.Right;
            gbRole.Location = new Point(3, 0);
            gbRole.Name = "gbRole";
            gbRole.Size = new Size(481, 401);
            gbRole.TabIndex = 7;
            gbRole.TabStop = false;
            gbRole.Text = "Thông tin Role";
            // 
            // btnEditRole
            // 
            btnEditRole.BackColor = Color.Yellow;
            btnEditRole.Location = new Point(276, 82);
            btnEditRole.Name = "btnEditRole";
            btnEditRole.Size = new Size(75, 23);
            btnEditRole.TabIndex = 5;
            btnEditRole.Text = "Sửa";
            btnEditRole.UseVisualStyleBackColor = false;
            // 
            // btnDeleteRole
            // 
            btnDeleteRole.BackColor = Color.Red;
            btnDeleteRole.Location = new Point(367, 82);
            btnDeleteRole.Name = "btnDeleteRole";
            btnDeleteRole.Size = new Size(75, 23);
            btnDeleteRole.TabIndex = 4;
            btnDeleteRole.Text = "Xóa";
            btnDeleteRole.UseVisualStyleBackColor = false;
            // 
            // btnAddRole
            // 
            btnAddRole.BackColor = SystemColors.ActiveCaption;
            btnAddRole.Location = new Point(181, 82);
            btnAddRole.Name = "btnAddRole";
            btnAddRole.Size = new Size(75, 23);
            btnAddRole.TabIndex = 2;
            btnAddRole.Text = "Thêm";
            btnAddRole.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(39, 48);
            label3.Name = "label3";
            label3.Size = new Size(82, 15);
            label3.TabIndex = 1;
            label3.Text = "Nhập tên Role";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(156, 44);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 0;
            // 
            // btnDeleteUser
            // 
            btnDeleteUser.BackColor = Color.Red;
            btnDeleteUser.Location = new Point(377, 118);
            btnDeleteUser.Name = "btnDeleteUser";
            btnDeleteUser.Size = new Size(75, 23);
            btnDeleteUser.TabIndex = 6;
            btnDeleteUser.Text = "Xóa";
            btnDeleteUser.UseVisualStyleBackColor = false;
            // 
            // btnEditUser
            // 
            btnEditUser.BackColor = Color.Yellow;
            btnEditUser.Location = new Point(276, 118);
            btnEditUser.Name = "btnEditUser";
            btnEditUser.Size = new Size(75, 23);
            btnEditUser.TabIndex = 5;
            btnEditUser.Text = "Sửa";
            btnEditUser.UseVisualStyleBackColor = false;
            // 
            // btnAddUser
            // 
            btnAddUser.BackColor = SystemColors.ActiveCaption;
            btnAddUser.Location = new Point(173, 118);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(75, 23);
            btnAddUser.TabIndex = 4;
            btnAddUser.Text = "Thêm";
            btnAddUser.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(42, 75);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 3;
            label2.Text = "Mật khẩu";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 38);
            label1.Name = "label1";
            label1.Size = new Size(86, 15);
            label1.TabIndex = 2;
            label1.Text = "Tên đăng nhập";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(156, 72);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(156, 34);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 0;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 450);
            Controls.Add(panel1);
            Controls.Add(dataGridView1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FormMain";
            Text = "Oracle Admin - Main";
            FormClosing += FormMain_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            gbUser.ResumeLayout(false);
            gbUser.PerformLayout();
            gbRole.ResumeLayout(false);
            gbRole.PerformLayout();
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
    }
}