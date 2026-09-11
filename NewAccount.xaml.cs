using System;
using System.Collections.Generic;
using System.Data;
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
using static System.Net.Mime.MediaTypeNames;

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for NewAccount.xaml
    /// </summary>
    public partial class NewAccount : Window
    {
        DataClasses1DataContext _dbConn = null;

        public NewAccount()
        {
            InitializeComponent();
            _dbConn = new DataClasses1DataContext(Properties.Settings.Default.OnlyTableConnectionString);
        }

        private void ClickHere_Button(object sender, RoutedEventArgs e)
        {
            Login proc = new Login();
            proc.Show();
            this.Close();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            string email = UserEmail.Text.Trim();
            string password = UserPassword.Password.Trim(); // In production, hash this!

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Please enter both email and password.",
                    "Missing Info",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            using (var conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
            {
                try
                {
                    conn.Open();

                    // 1) Existing-account login path
                    using (var checkCmd = new SqlCommand(
                               "SELECT Account_ID, Password FROM OneTable WHERE Email = @Email", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int existingId = reader.GetInt32(0);
                                string existingPw = reader.GetString(1);

                                if (existingPw == password)
                                {
                                    // <-- Show a message before navigating in
                                    MessageBox.Show(
                                        "Welcome back! You have successfully logged in.",
                                        "Login Successful",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information
                                    );

                                    UserSession.AccountId = existingId;
                                    new MainWindow(UserSession.AccountId).Show();
                                    this.Close();
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show(
                                        "That email is already registered,\n" +
                                        "but the password you entered is incorrect.",
                                        "Login Failed",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error
                                    );
                                    return;
                                }
                            }
                        }
                    }

                    // 2) New-account creation path
                    using (var insertCmd = new SqlCommand("dbo.uspAddAccount", conn))
                    {
                        insertCmd.CommandType = CommandType.StoredProcedure;
                        insertCmd.Parameters.AddWithValue("@Email", email);
                        insertCmd.Parameters.AddWithValue("@Password", password);

                        int newAccountId = Convert.ToInt32(insertCmd.ExecuteScalar());
                        UserSession.AccountId = newAccountId;

                        // <-- Confirmation for account creation
                        MessageBox.Show(
                            "Account successfully created!",
                            "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );

                        MainWindow enroll = new MainWindow(UserSession.AccountId);
                        enroll.Show();  
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "An error occurred:\n" + ex.Message,
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }



    }


}
