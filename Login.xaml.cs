using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewAccount account = new NewAccount();
            account.Show();
            this.Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim(); // Use PasswordBox for security

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Missing Info",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
                {
                    conn.Open();
                    string query = "SELECT Account_ID, Password FROM OneTable WHERE Username = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int accountId = Convert.ToInt32(reader["Account_ID"]);
                                string storedPassword = reader["Password"].ToString();

                                if (storedPassword == password)
                                {
                                    UserSession.AccountId = accountId;

                                    MessageBox.Show("Login successful!", "Welcome",
                                        MessageBoxButton.OK, MessageBoxImage.Information);

                                    Home home = new Home();
                                    home.Show();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Incorrect password. Please try again.",
                                        "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Username not found. Please check or register.",
                                    "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
