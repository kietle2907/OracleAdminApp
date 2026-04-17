namespace OracleAdminApp
{
    public partial class Form1 : Form
    {
        private OracleDbConnection? _dbConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get values from form inputs
            string dataSource = textBoxDataSource.Text.Trim();
            string userId = textBox1.Text.Trim();
            string password = textBox2.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(dataSource))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxDataSource.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            // Create connection
            _dbConnection = new OracleDbConnection(dataSource, userId, password);

            // Test connection
            if (_dbConnection.TestConnection())
            {
                MessageBox.Show("Kết nối đến cơ sở dữ liệu Oracle thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open FormMain and pass the connection
                FormMain mainForm = new FormMain(_dbConnection);
                mainForm.Show();

                // Hide current form (you can also use this.Close() to close it)
                this.Hide();
            }
            else
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu Oracle. Vui lòng kiểm tra lại thông tin.",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

