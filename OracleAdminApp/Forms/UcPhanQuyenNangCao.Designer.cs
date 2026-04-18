// =====================================================================
//  UcPhanQuyenNangCao.Designer.cs
//  Designer file cho UserControl. Thông thường file này được Visual
//  Studio tự sinh; ở đây mình viết tay cho gọn, bạn có thể mở bằng
//  Designer để kéo-thả chỉnh tiếp.
// =====================================================================

namespace OracleAdminApp.Forms
{
    partial class UcPhanQuyenNangCao
    {
        private System.ComponentModel.IContainer components = null;

        // ---- Vùng chọn đối tượng ----
        private System.Windows.Forms.GroupBox  grpObject;
        private System.Windows.Forms.Label     lblObjectType;
        private System.Windows.Forms.ComboBox  cboObjectType;
        private System.Windows.Forms.Label     lblSchema;
        private System.Windows.Forms.ComboBox  cboSchema;
        private System.Windows.Forms.Label     lblObject;
        private System.Windows.Forms.ComboBox  cboObject;

        // ---- Vùng chọn privilege ----
        private System.Windows.Forms.GroupBox      grpPrivileges;
        private System.Windows.Forms.CheckedListBox clbPrivileges;

        // ---- Vùng chọn cột cho SELECT / UPDATE ----
        private System.Windows.Forms.GroupBox      grpColumns;
        private System.Windows.Forms.Label         lblSelectCols;
        private System.Windows.Forms.CheckedListBox clbSelectCols;
        private System.Windows.Forms.Label         lblUpdateCols;
        private System.Windows.Forms.CheckedListBox clbUpdateCols;
        private System.Windows.Forms.Label         lblColumnsHint;

        // ---- Vùng grantee và tùy chọn ----
        private System.Windows.Forms.GroupBox grpGrantee;
        private System.Windows.Forms.Label    lblGrantee;
        private System.Windows.Forms.ComboBox cboGrantee;
        private System.Windows.Forms.CheckBox chkWithGrantOption;

        // ---- Nút thao tác + hiển thị SQL ----
        private System.Windows.Forms.Button   btnPreviewSql;
        private System.Windows.Forms.Button   btnGrant;
        private System.Windows.Forms.Button   btnRevoke;
        private System.Windows.Forms.Button   btnRefresh;
        private System.Windows.Forms.TextBox  txtSqlPreview;

        // ---- Hiển thị quyền hiện tại của grantee ----
        private System.Windows.Forms.GroupBox      grpCurrent;
        private System.Windows.Forms.TabControl    tcCurrent;
        private System.Windows.Forms.TabPage       tpTablePrivs;
        private System.Windows.Forms.TabPage       tpColumnPrivs;
        private System.Windows.Forms.DataGridView  dgvTablePrivs;
        private System.Windows.Forms.DataGridView  dgvColumnPrivs;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpObject       = new System.Windows.Forms.GroupBox();
            this.lblObjectType   = new System.Windows.Forms.Label();
            this.cboObjectType   = new System.Windows.Forms.ComboBox();
            this.lblSchema       = new System.Windows.Forms.Label();
            this.cboSchema       = new System.Windows.Forms.ComboBox();
            this.lblObject       = new System.Windows.Forms.Label();
            this.cboObject       = new System.Windows.Forms.ComboBox();

            this.grpPrivileges   = new System.Windows.Forms.GroupBox();
            this.clbPrivileges   = new System.Windows.Forms.CheckedListBox();

            this.grpColumns      = new System.Windows.Forms.GroupBox();
            this.lblSelectCols   = new System.Windows.Forms.Label();
            this.clbSelectCols   = new System.Windows.Forms.CheckedListBox();
            this.lblUpdateCols   = new System.Windows.Forms.Label();
            this.clbUpdateCols   = new System.Windows.Forms.CheckedListBox();
            this.lblColumnsHint  = new System.Windows.Forms.Label();

            this.grpGrantee       = new System.Windows.Forms.GroupBox();
            this.lblGrantee       = new System.Windows.Forms.Label();
            this.cboGrantee       = new System.Windows.Forms.ComboBox();
            this.chkWithGrantOption = new System.Windows.Forms.CheckBox();

            this.btnPreviewSql   = new System.Windows.Forms.Button();
            this.btnGrant        = new System.Windows.Forms.Button();
            this.btnRevoke       = new System.Windows.Forms.Button();
            this.btnRefresh      = new System.Windows.Forms.Button();
            this.txtSqlPreview   = new System.Windows.Forms.TextBox();

            this.grpCurrent      = new System.Windows.Forms.GroupBox();
            this.tcCurrent       = new System.Windows.Forms.TabControl();
            this.tpTablePrivs    = new System.Windows.Forms.TabPage();
            this.tpColumnPrivs   = new System.Windows.Forms.TabPage();
            this.dgvTablePrivs   = new System.Windows.Forms.DataGridView();
            this.dgvColumnPrivs  = new System.Windows.Forms.DataGridView();

            this.SuspendLayout();

            // ===== grpObject =====
            this.grpObject.Text = "1. Chọn đối tượng cần phân quyền";
            this.grpObject.Location = new System.Drawing.Point(12, 12);
            this.grpObject.Size     = new System.Drawing.Size(430, 150);
            this.grpObject.Controls.Add(this.lblObjectType);
            this.grpObject.Controls.Add(this.cboObjectType);
            this.grpObject.Controls.Add(this.lblSchema);
            this.grpObject.Controls.Add(this.cboSchema);
            this.grpObject.Controls.Add(this.lblObject);
            this.grpObject.Controls.Add(this.cboObject);

            this.lblObjectType.Text     = "Loại đối tượng:";
            this.lblObjectType.Location = new System.Drawing.Point(15, 30);
            this.lblObjectType.AutoSize = true;

            this.cboObjectType.Location  = new System.Drawing.Point(120, 27);
            this.cboObjectType.Size      = new System.Drawing.Size(280, 23);
            this.cboObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblSchema.Text     = "Schema (owner):";
            this.lblSchema.Location = new System.Drawing.Point(15, 65);
            this.lblSchema.AutoSize = true;

            this.cboSchema.Location  = new System.Drawing.Point(120, 62);
            this.cboSchema.Size      = new System.Drawing.Size(280, 23);
            this.cboSchema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblObject.Text     = "Đối tượng:";
            this.lblObject.Location = new System.Drawing.Point(15, 100);
            this.lblObject.AutoSize = true;

            this.cboObject.Location  = new System.Drawing.Point(120, 97);
            this.cboObject.Size      = new System.Drawing.Size(280, 23);
            this.cboObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // ===== grpPrivileges =====
            this.grpPrivileges.Text = "2. Privilege áp dụng";
            this.grpPrivileges.Location = new System.Drawing.Point(12, 170);
            this.grpPrivileges.Size     = new System.Drawing.Size(200, 215);
            this.grpPrivileges.Controls.Add(this.clbPrivileges);

            this.clbPrivileges.Location = new System.Drawing.Point(10, 22);
            this.clbPrivileges.Size     = new System.Drawing.Size(180, 185);
            this.clbPrivileges.CheckOnClick = true;

            // ===== grpColumns =====
            this.grpColumns.Text = "3. (3c) Cột cho SELECT / UPDATE – bỏ trống nếu toàn bảng";
            this.grpColumns.Location = new System.Drawing.Point(220, 170);
            this.grpColumns.Size     = new System.Drawing.Size(430, 215);
            this.grpColumns.Controls.Add(this.lblSelectCols);
            this.grpColumns.Controls.Add(this.clbSelectCols);
            this.grpColumns.Controls.Add(this.lblUpdateCols);
            this.grpColumns.Controls.Add(this.clbUpdateCols);
            this.grpColumns.Controls.Add(this.lblColumnsHint);

            this.lblSelectCols.Text     = "Cột cho SELECT:";
            this.lblSelectCols.Location = new System.Drawing.Point(10, 22);
            this.lblSelectCols.AutoSize = true;

            this.clbSelectCols.Location = new System.Drawing.Point(10, 40);
            this.clbSelectCols.Size     = new System.Drawing.Size(200, 140);
            this.clbSelectCols.CheckOnClick = true;

            this.lblUpdateCols.Text     = "Cột cho UPDATE:";
            this.lblUpdateCols.Location = new System.Drawing.Point(220, 22);
            this.lblUpdateCols.AutoSize = true;

            this.clbUpdateCols.Location = new System.Drawing.Point(220, 40);
            this.clbUpdateCols.Size     = new System.Drawing.Size(200, 140);
            this.clbUpdateCols.CheckOnClick = true;

            this.lblColumnsHint.Text     =
                "INSERT/DELETE không hỗ trợ mức cột – tick không có tác dụng.";
            this.lblColumnsHint.Location = new System.Drawing.Point(10, 185);
            this.lblColumnsHint.AutoSize = true;
            this.lblColumnsHint.ForeColor = System.Drawing.Color.DimGray;

            // ===== grpGrantee =====
            this.grpGrantee.Text = "4. Người/vai trò được cấp";
            this.grpGrantee.Location = new System.Drawing.Point(12, 395);
            this.grpGrantee.Size     = new System.Drawing.Size(430, 85);
            this.grpGrantee.Controls.Add(this.lblGrantee);
            this.grpGrantee.Controls.Add(this.cboGrantee);
            this.grpGrantee.Controls.Add(this.chkWithGrantOption);

            this.lblGrantee.Text     = "Grantee:";
            this.lblGrantee.Location = new System.Drawing.Point(15, 30);
            this.lblGrantee.AutoSize = true;

            this.cboGrantee.Location  = new System.Drawing.Point(120, 27);
            this.cboGrantee.Size      = new System.Drawing.Size(280, 23);
            this.cboGrantee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.chkWithGrantOption.Text     = "WITH GRANT OPTION";
            this.chkWithGrantOption.Location = new System.Drawing.Point(120, 56);
            this.chkWithGrantOption.AutoSize = true;

            // ===== các nút + SQL preview =====
            this.btnPreviewSql.Text     = "Xem SQL";
            this.btnPreviewSql.Location = new System.Drawing.Point(460, 395);
            this.btnPreviewSql.Size     = new System.Drawing.Size(85, 30);

            this.btnGrant.Text     = "GRANT";
            this.btnGrant.Location = new System.Drawing.Point(460, 430);
            this.btnGrant.Size     = new System.Drawing.Size(85, 30);
            this.btnGrant.BackColor = System.Drawing.Color.LightGreen;

            this.btnRevoke.Text     = "REVOKE";
            this.btnRevoke.Location = new System.Drawing.Point(555, 430);
            this.btnRevoke.Size     = new System.Drawing.Size(85, 30);
            this.btnRevoke.BackColor = System.Drawing.Color.LightSalmon;

            this.btnRefresh.Text     = "Làm mới danh sách";
            this.btnRefresh.Location = new System.Drawing.Point(555, 395);
            this.btnRefresh.Size     = new System.Drawing.Size(130, 30);

            this.txtSqlPreview.Location = new System.Drawing.Point(12, 490);
            this.txtSqlPreview.Size     = new System.Drawing.Size(673, 60);
            this.txtSqlPreview.Multiline = true;
            this.txtSqlPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSqlPreview.ReadOnly = true;
            this.txtSqlPreview.Font = new System.Drawing.Font("Consolas", 9F);

            // ===== grpCurrent (quyền hiện tại của grantee) =====
            this.grpCurrent.Text = "Quyền hiện tại của grantee đang chọn";
            this.grpCurrent.Location = new System.Drawing.Point(700, 12);
            this.grpCurrent.Size     = new System.Drawing.Size(520, 538);
            this.grpCurrent.Controls.Add(this.tcCurrent);

            this.tcCurrent.Location = new System.Drawing.Point(10, 22);
            this.tcCurrent.Size     = new System.Drawing.Size(500, 505);
            this.tcCurrent.Controls.Add(this.tpTablePrivs);
            this.tcCurrent.Controls.Add(this.tpColumnPrivs);

            this.tpTablePrivs.Text = "Quyền mức đối tượng";
            this.tpTablePrivs.Controls.Add(this.dgvTablePrivs);

            this.tpColumnPrivs.Text = "Quyền mức cột";
            this.tpColumnPrivs.Controls.Add(this.dgvColumnPrivs);

            this.dgvTablePrivs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTablePrivs.ReadOnly = true;
            this.dgvTablePrivs.AllowUserToAddRows = false;
            this.dgvTablePrivs.RowHeadersVisible  = false;
            this.dgvTablePrivs.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;

            this.dgvColumnPrivs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvColumnPrivs.ReadOnly = true;
            this.dgvColumnPrivs.AllowUserToAddRows = false;
            this.dgvColumnPrivs.RowHeadersVisible  = false;
            this.dgvColumnPrivs.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;

            // ===== UserControl =====
            this.Controls.Add(this.grpObject);
            this.Controls.Add(this.grpPrivileges);
            this.Controls.Add(this.grpColumns);
            this.Controls.Add(this.grpGrantee);
            this.Controls.Add(this.btnPreviewSql);
            this.Controls.Add(this.btnGrant);
            this.Controls.Add(this.btnRevoke);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtSqlPreview);
            this.Controls.Add(this.grpCurrent);

            this.Name  = "UcPhanQuyenNangCao";
            this.Size  = new System.Drawing.Size(1235, 565);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
