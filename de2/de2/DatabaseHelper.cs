using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ShoeStoreApp
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=adclg1;Database=ooobuv;Integrated Security=True;";

        // === АВТОРИЗАЦИЯ ===
        public bool ValidateUser(string login, string password)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT role FROM users WHERE login = @login AND password = @password";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);

                        var result = command.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public string GetUserRole(string login)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT role FROM users WHERE login = @login";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        return command.ExecuteScalar()?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения роли: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // === ТОВАРЫ ===
        public DataTable GetProducts(string search = "")
        {
            string query = @"SELECT article as 'Артикул', name as 'Наименование', price as 'Цена', 
                           category as 'Категория', stock_quantity as 'Количество', 
                           manufacturer as 'Производитель' FROM products";

            if (!string.IsNullOrEmpty(search))
            {
                query += $" WHERE name LIKE '%{search}%' OR article LIKE '%{search}%' OR category LIKE '%{search}%'";
            }

            return ExecuteQuery(query);
        }

        public bool AddProduct(string article, string name, decimal price, string category,
                             int stockQuantity, string manufacturer, string supplier,
                             decimal discount, string description)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO products 
                                   (article, name, price, category, stock_quantity, 
                                    manufacturer, supplier, discount, description) 
                                   VALUES (@article, @name, @price, @category, @stockQuantity, 
                                           @manufacturer, @supplier, @discount, @description)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@article", article);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@category", category);
                        command.Parameters.AddWithValue("@stockQuantity", stockQuantity);
                        command.Parameters.AddWithValue("@manufacturer", manufacturer);
                        command.Parameters.AddWithValue("@supplier", supplier);
                        command.Parameters.AddWithValue("@discount", discount);
                        command.Parameters.AddWithValue("@description", description);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления товара: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdateProduct(string article, string name, decimal price, string category,
                                int stockQuantity, string manufacturer, string supplier,
                                decimal discount, string description)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE products SET 
                                   name = @name, price = @price, category = @category, 
                                   stock_quantity = @stockQuantity, manufacturer = @manufacturer,
                                   supplier = @supplier, discount = @discount, description = @description
                                   WHERE article = @article";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@article", article);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@category", category);
                        command.Parameters.AddWithValue("@stockQuantity", stockQuantity);
                        command.Parameters.AddWithValue("@manufacturer", manufacturer);
                        command.Parameters.AddWithValue("@supplier", supplier);
                        command.Parameters.AddWithValue("@discount", discount);
                        command.Parameters.AddWithValue("@description", description);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления товара: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool DeleteProduct(string article)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM products WHERE article = @article";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@article", article);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления товара: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // === ЗАКАЗЫ ===
        public DataTable GetOrders()
        {
            string query = @"SELECT o.id as 'ID', o.order_number as 'Номер заказа', o.order_date as 'Дата заказа',
                           o.client_name as 'Клиент', o.status as 'Статус', 
                           pp.address as 'Пункт выдачи' FROM orders o 
                           JOIN pickup_points pp ON o.pickup_point_id = pp.id";

            return ExecuteQuery(query);
        }

        public bool AddOrder(int orderNumber, DateTime orderDate, DateTime deliveryDate,
                           int pickupPointId, string clientName, string receiveCode, string status)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO orders 
                                   (order_number, order_date, delivery_date, pickup_point_id, 
                                    client_name, receive_code, status) 
                                   VALUES (@orderNumber, @orderDate, @deliveryDate, @pickupPointId,
                                           @clientName, @receiveCode, @status)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@orderNumber", orderNumber);
                        command.Parameters.AddWithValue("@orderDate", orderDate);
                        command.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                        command.Parameters.AddWithValue("@pickupPointId", pickupPointId);
                        command.Parameters.AddWithValue("@clientName", clientName);
                        command.Parameters.AddWithValue("@receiveCode", receiveCode);
                        command.Parameters.AddWithValue("@status", status);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdateOrder(int id, int orderNumber, DateTime orderDate, DateTime deliveryDate,
                              int pickupPointId, string clientName, string receiveCode, string status)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE orders SET 
                                   order_number = @orderNumber, order_date = @orderDate, 
                                   delivery_date = @deliveryDate, pickup_point_id = @pickupPointId,
                                   client_name = @clientName, receive_code = @receiveCode, status = @status
                                   WHERE id = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@orderNumber", orderNumber);
                        command.Parameters.AddWithValue("@orderDate", orderDate);
                        command.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                        command.Parameters.AddWithValue("@pickupPointId", pickupPointId);
                        command.Parameters.AddWithValue("@clientName", clientName);
                        command.Parameters.AddWithValue("@receiveCode", receiveCode);
                        command.Parameters.AddWithValue("@status", status);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool DeleteOrder(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM orders WHERE id = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable GetPickupPoints()
        {
            string query = "SELECT id, address FROM pickup_points";
            return ExecuteQuery(query);
        }

        // === ПОЛЬЗОВАТЕЛИ ===
        public DataTable GetUsers()
        {
            string query = "SELECT id as 'ID', login as 'Логин', full_name as 'ФИО', role as 'Роль' FROM users";
            return ExecuteQuery(query);
        }

        public bool AddUser(string login, string password, string fullName, string role)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO users (login, password, full_name, role) VALUES (@login, @password, @fullName, @role)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@fullName", fullName);
                        command.Parameters.AddWithValue("@role", role);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdateUser(int id, string login, string password, string fullName, string role)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE users SET login = @login, password = @password, full_name = @fullName, role = @role WHERE id = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@fullName", fullName);
                        command.Parameters.AddWithValue("@role", role);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool DeleteUser(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM users WHERE id = @id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // === ОБЩИЙ МЕТОД ===
        private DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                using (var adapter = new SqlDataAdapter(command))
                {
                    connection.Open();
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dataTable;
        }
    }
}