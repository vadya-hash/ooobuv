using System;
using System.Windows.Forms;

namespace ShoeStoreApp
{
    public partial class LoginForm : Form
    {
        private DatabaseHelper db = new DatabaseHelper();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (db.ValidateUser(login, password))
            {
                string role = db.GetUserRole(login);
                OpenMainForm(role, login);
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            OpenMainForm("Гость", "Гость");
        }

        private void OpenMainForm(string role, string username)
        {
            this.Hide();
            MainForm mainForm = new MainForm(role, username);
            mainForm.ShowDialog();
            this.Close();
        }
    }
}