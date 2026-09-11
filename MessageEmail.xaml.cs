using System;
using System.Collections.Generic;
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
using System.Net;
using System.Net.Mail;
using System.Data;
using System.Data.SqlClient;


namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for MessageEmail.xaml
    /// </summary>
    public partial class MessageEmail : Window
    {
      

        DataClasses1DataContext _dbConn = null;
        public MessageEmail()
        {
            InitializeComponent();
            _dbConn = new DataClasses1DataContext(Properties.Settings.Default.OnlyTableConnectionString);
       
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string usernameInput = txtStudentID.Text.Trim();

            if (string.IsNullOrWhiteSpace(usernameInput))
            {
                MessageBox.Show("Please enter your Username.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
                {
                    conn.Open();

                    string query = "SELECT Student_ID, Email FROM dbo.Onetable WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", usernameInput);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // You may store session details here using static class or pass to next window
                                CongratulationsMessage congrats = new CongratulationsMessage();
                                congrats.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("No student found with that username. Please check and try again.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while accessing the database:\n" + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Transaction back = new Transaction();
            back.Show();
            this.Close();
        }


        //private void SendEmailConfirmation(string toEmail, string username)
        //{
        //    try
        //    {
        //        var mail = new MailMessage();
        //        mail.From = new MailAddress("youradmin@email.com");
        //        mail.To.Add(toEmail);
        //        mail.Subject = "Your Login Username";
        //        mail.Body = $"Hello!\n\nYour student portal username is: {username}\n\nThank you.";

        //        var smtp = new SmtpClient("smtp.yourserver.com"); // e.g., smtp.gmail.com
        //        smtp.Port = 587;
        //        smtp.Credentials = new NetworkCredential("youradmin@email.com", "yourpassword");
        //        smtp.EnableSsl = true;

        //        smtp.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Failed to send email: " + ex.Message);
        //    }
        //}
    }
}
