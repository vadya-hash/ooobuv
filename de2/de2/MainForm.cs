using System;
using System.Data;
using System.Windows.Forms;

namespace ShoeStoreApp
{
    public partial class MainForm : Form
    {
        private string userRole;
        private string username;
        private DatabaseHelper db = new DatabaseHelper();

        public MainForm(string role, string username)
        {
            InitializeComponent();
            this.userRole = role;
            this.username = username;
            ConfigureUIByRole();
            LoadData();
        }

        private void ConfigureUIByRole()
        {
            this.Text = $"Обувной магазин - {username} ({userRole})";

            bool isAdmin = userRole == "Администратор";
            bool isManager = userRole == "Менеджер" || isAdmin;

            // Скрыть вкладки если нет прав
            if (!isManager)
            {
                mainTabControl.TabPages.Remove(tabOrders);
            }
            if (!isAdmin)
            {
                mainTabControl.TabPages.Remove(tabUsers);
            }

            // Настройка кнопок управления
            btnAddProduct.Visible = isAdmin;
            btnEditProduct.Visible = isAdmin;
            btnDeleteProduct.Visible = isAdmin;

            // Настройка поиска
            txtSearch.Visible = isManager;
            btnSearch.Visible = isManager;
            lblSearch.Visible = isManager;
        }

        private void LoadData()
        {
            try
            {
                LoadProducts();
                if (mainTabControl.TabPages.Contains(tabOrders))
                    LoadOrders();
                if (mainTabControl.TabPages.Contains(tabUsers))
                    LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts(string search = "")
        {
            dataGridViewProducts.DataSource = db.GetProducts(search);
        }

        private void LoadOrders()
        {
            dataGridViewOrders.DataSource = db.GetOrders();
        }

        private void LoadUsers()
        {
            dataGridViewUsers.DataSource = db.GetUsers();
        }

        // === ОБРАБОТЧИКИ ДЛЯ ТОВАРОВ ===
        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dataGridViewProducts.SelectedRows[0];
            string article = row.Cells["Артикул"].Value.ToString();
            string name = row.Cells["Наименование"].Value.ToString();
            decimal price = decimal.Parse(row.Cells["Цена"].Value.ToString());
            string category = row.Cells["Категория"].Value.ToString();
            int stockQuantity = int.Parse(row.Cells["Количество"].Value.ToString());
            string manufacturer = row.Cells["Производитель"].Value.ToString();

           
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dataGridViewProducts.SelectedRows[0];
            string article = row.Cells["Артикул"].Value.ToString();
            string name = row.Cells["Наименование"].Value.ToString();

            var result = MessageBox.Show($"Вы уверены, что хотите удалить товар {name}?",
                                       "Подтверждение удаления",
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = db.DeleteProduct(article);
                if (success)
                {
                    MessageBox.Show("Товар удален", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                }
            }
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dataGridViewOrders.SelectedRows[0];
            int id = int.Parse(row.Cells["ID"].Value.ToString());
            int orderNumber = int.Parse(row.Cells["Номер заказа"].Value.ToString());
            DateTime orderDate = DateTime.Parse(row.Cells["Дата заказа"].Value.ToString());
            DateTime deliveryDate = DateTime.Parse(row.Cells["Дата доставки"].Value.ToString());
            string clientName = row.Cells["Клиент"].Value.ToString();
            string receiveCode = row.Cells["Код получения"].Value.ToString();
            string status = row.Cells["Статус"].Value.ToString();

            
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dataGridViewOrders.SelectedRows[0];
            int id = int.Parse(row.Cells["ID"].Value.ToString());
            int orderNumber = int.Parse(row.Cells["Номер заказа"].Value.ToString());

            var result = MessageBox.Show($"Вы уверены, что хотите удалить заказ №{orderNumber}?",
                                       "Подтверждение удаления",
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = db.DeleteOrder(id);
                if (success)
                {
                    MessageBox.Show("Заказ удален", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrders();
                }
            }
        }

        // === ОБРАБОТЧИКИ ДЛЯ ПОЛЬЗОВАТЕЛЕЙ ===
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // Реализация добавления пользователя
            MessageBox.Show("Добавление пользователя", "Пользователи", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            // Реализация редактирования пользователя
            MessageBox.Show("Редактирование пользователя", "Пользователи", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            // Реализация удаления пользователя
            MessageBox.Show("Удаление пользователя", "Пользователи", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefreshUsers_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}