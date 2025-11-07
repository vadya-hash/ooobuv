using System;
using System.Windows.Forms;

namespace ShoeStoreApp
{
    public partial class UserEditForm : Form
    {
        private TextBox txtLogin;
        private TextBox txtPassword;
        private TextBox txtFullName;
        private ComboBox cmbRole;
        private Button btnOk;
        private Button btnCancel;

        public string Login => txtLogin.Text;
        public string Password => txtPassword.Text;
        public string FullName => txtFullName.Text;
        public string Role => cmbRole.SelectedItem?.ToString();

        public UserEditForm()
        {
            InitializeComponent();
            this.Text = "Добавить пользователя";
        }

        public UserEditForm(int id, string login, string password, string fullName, string role) : this()
        {
            txtLogin.Text = login;
            txtPassword.Text = password;
            txtFullName.Text = fullName;
            cmbRole.SelectedItem = role;
            this.Text = "Редактировать пользователя";
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Labels
            var lblLogin = new Label { Text = "Логин:", Location = new System.Drawing.Point(10, 20), Width = 80 };
            var lblPassword = new Label { Text = "Пароль:", Location = new System.Drawing.Point(10, 50), Width = 80 };
            var lblFullName = new Label { Text = "ФИО:", Location = new System.Drawing.Point(10, 80), Width = 80 };
            var lblRole = new Label { Text = "Роль:", Location = new System.Drawing.Point(10, 110), Width = 80 };

            // TextBoxes
            this.txtLogin = new TextBox { Location = new System.Drawing.Point(100, 17), Width = 200 };
            this.txtPassword = new TextBox { Location = new System.Drawing.Point(100, 47), Width = 200 };
            this.txtFullName = new TextBox { Location = new System.Drawing.Point(100, 77), Width = 200 };

            // ComboBox
            this.cmbRole = new ComboBox { Location = new System.Drawing.Point(100, 107), Width = 200 };
            cmbRole.Items.AddRange(new[] { "Администратор", "Менеджер", "Авторизированный клиент" });
            cmbRole.SelectedIndex = 0;

            // Buttons
            this.btnOk = new Button { Text = "OK", Location = new System.Drawing.Point(100, 150), Width = 80 };
            this.btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(190, 150), Width = 80 };

            btnOk.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            // Form
            this.Text = "Пользователь";
            this.Size = new System.Drawing.Size(330, 220);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Controls.AddRange(new Control[] {
                lblLogin, lblPassword, lblFullName, lblRole,
                txtLogin, txtPassword, txtFullName, cmbRole,
                btnOk, btnCancel
            });

            this.ResumeLayout(false);
        }
    }
}