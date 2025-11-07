using System;
using System.Data;
using System.Windows.Forms;

namespace ShoeStoreApp
{
    public partial class UsersForm : Form
    {
        private DatabaseHelper db = new DatabaseHelper();
        private DataGridView dataGridViewUsers;
        private Panel panelUsers;
        private Button btnAddUser;
        private Button btnEditUser;
        private Button btnDeleteUser;
        private Button btnRefresh;

        public UsersForm()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // DataGridView для пользователей
            this.dataGridViewUsers = new DataGridView();
            this.dataGridViewUsers.Dock = DockStyle.Fill;
            this.dataGridViewUsers.ReadOnly = true;
            this.dataGridViewUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Кнопки управления
            this.btnAddUser = new Button();
            this.btnAddUser.Text = "Добавить";
            this.btnAddUser.Location = new System.Drawing.Point(10, 10);
            this.btnAddUser.Size = new System.Drawing.Size(100, 30);
            this.btnAddUser.Click += BtnAddUser_Click;

            this.btnEditUser = new Button();
            this.btnEditUser.Text = "Редактировать";
            this.btnEditUser.Location = new System.Drawing.Point(120, 10);
            this.btnEditUser.Size = new System.Drawing.Size(100, 30);
            this.btnEditUser.Click += BtnEditUser_Click;

            this.btnDeleteUser = new Button();
            this.btnDeleteUser.Text = "Удалить";
            this.btnDeleteUser.Location = new System.Drawing.Point(230, 10);
            this.btnDeleteUser.Size = new System.Drawing.Size(80, 30);
            this.btnDeleteUser.Click += BtnDeleteUser_Click;

            this.btnRefresh = new Button();
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Location = new System.Drawing.Point(320, 10);
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.Click += BtnRefresh_Click;

            // Панель для кнопок
            this.panelUsers = new Panel();
            this.panelUsers.Dock = DockStyle.Top;
            this.panelUsers.Height = 50;
            this.panelUsers.Controls.Add(btnAddUser);
            this.panelUsers.Controls.Add(btnEditUser);
            this.panelUsers.Controls.Add(btnDeleteUser);
            this.panelUsers.Controls.Add(btnRefresh);

            // Основная форма
            this.Text = "Управление пользователями";
            this.Size = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.Add(dataGridViewUsers);
            this.Controls.Add(panelUsers);

            this.ResumeLayout(false);
        }

        private void LoadUsers()
        {
            dataGridViewUsers.DataSource = db.GetUsers();
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            using (var form = new UserEditForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    bool success = db.AddUser(form.Login, form.Password, form.FullName, form.Role);
                    if (success)
                    {
                        MessageBox.Show("Пользователь добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsers();
                    }
                }
            }
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dataGridViewUsers.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["ID"].Value);
            string login = row.Cells["Логин"].Value.ToString();
            string fullName = row.Cells["ФИО"].Value.ToString();
            string role = row.Cells["Роль"].Value.ToString();

            using (var form = new UserEditForm(id, login, "", fullName, role))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    bool success = db.UpdateUser(id, form.Login, form.Password, form.FullName, form.Role);
                    if (success)
                    {
                        MessageBox.Show("Пользователь обновлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsers();
                    }
                }
            }
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dataGridViewUsers.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["ID"].Value);
            string login = row.Cells["Логин"].Value.ToString();

            var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя {login}?",
                                       "Подтверждение удаления",
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = db.DeleteUser(id);
                if (success)
                {
                    MessageBox.Show("Пользователь удален", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}