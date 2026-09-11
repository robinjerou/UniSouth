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
using System.Data.SqlClient;
using System.IO;
using System.Windows.Media.Animation;

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for DarkMode.xaml
    /// </summary>
    public partial class DarkMode : Window
    {
        
        public DarkMode()
        {
            InitializeComponent();
            LoadStudentInfo();
            LoadStudentImage();
        }
        private void LoadStudentInfo()
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
            {
                string query = "SELECT First_Name, Last_Name, Email, Birthdate, Phone, Gender, Address, Course_Type, Course_YearLevel FROM OneTable WHERE Account_ID = @AccountID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AccountID", UserSession.AccountId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtFirstName.Text = reader["First_Name"].ToString();
                            txtLastName.Text = reader["Last_Name"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtBirthdate.Text = Convert.ToDateTime(reader["Birthdate"]).ToString("yyyy-MM-dd");
                            txtPhone.Text = reader["Phone"].ToString();
                            txtGender.Text = reader["Gender"].ToString();
                            txtAddress.Text = reader["Address"].ToString();
                            txtCourse.Text = reader["Course_Type"].ToString();
                            txtYearLevel.Text = reader["Course_YearLevel"].ToString();
                        }
                    }
                }
            }
        }

        private void LoadStudentImage()
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
            {
                string query = "SELECT ProfileImage FROM OneTable WHERE Account_ID = @AccountID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AccountID", UserSession.AccountId);
                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        byte[] imageBytes = (byte[])result;
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = ms;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            ellipseSettingsProfile.Fill = new ImageBrush(bitmap); // Make sure this control exists in DarkMode.xaml
                        }
                    }
                }
            }
        }

        private void Nav_Home_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }

        private void Nav_AcademicRecord_Click(object sender, RoutedEventArgs e)
        {
            AcademicRecord acadademic = new AcademicRecord();
            acadademic.Show();
            this.Close();
        }


        private void Nav_AccountRecord_Click(object sender, RoutedEventArgs e)
        {
            AccountRecord account = new AccountRecord();
            account.Show();
            this.Close();
        }
        private void Nav_Concerns_Click(object sender, RoutedEventArgs e)
        {
            Ticket concern = new Ticket();
            concern.Show();
            this.Close();
        }
        private void Nav_Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings setting = new Settings();
            setting.Show();
            this.Close();
        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            UserNeed userhelp = new UserNeed();
            userhelp.Show();
            this.Close();
        }

        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            Settings profile = new Settings();
            profile.Show();
            this.Close();
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Optional: close it, or leave as is
        }


        private void NotifToggle_Unchecked(object sender, RoutedEventArgs e)
        {

        }
        private void ShowPopup(string message)
        {
            NotificationPopup popup = new NotificationPopup(message);

            Canvas.SetLeft(popup, 0);
            Canvas.SetBottom(popup, 0);
            PopupContainer.Children.Add(popup);

            var fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(3)))
            {
                BeginTime = TimeSpan.FromSeconds(2)
            };

            fadeOut.Completed += (s, a) => PopupContainer.Children.Remove(popup);
            popup.BeginAnimation(UserControl.OpacityProperty, fadeOut);
        }

        private void NotifToggle_Checked(object sender, RoutedEventArgs e)
        {
            ShowPopup("Notifications enabled!");
        }
    }
}
