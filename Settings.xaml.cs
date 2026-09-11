using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        //private int _accountId;
        public Settings(/*int accountId*/)
        {
            InitializeComponent();
            //_accountId = accountId;

            //LoadStudentSettings();

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
                            ellipseSettingsProfile.Fill = new ImageBrush(bitmap);
                        }
                    }
                }
            }
        }


        //private void LoadStudentSettings()
        //{


        //    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
        //    {
        //        conn.Open();

        //        string query = "SELECT First_Name, Last_Name FROM OneTable WHERE Account_ID = @AccountID";
        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@AccountID", UserSession.AccountId);

        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string firstName = reader["First_Name"].ToString();
        //                    string lastName = reader["Last_Name"].ToString();
        //                    StudentName.Text = $"{firstName} {lastName}";
        //                }
        //            }
        //        }
        //    }
        //}

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
        }


        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            // Open your dark-mode window (implement this class yourself)
            this.Opacity = 0; // Optional: fade out

            DarkMode dark = new DarkMode();
            dark.Opacity = 0;
            dark.Show();

            var fadeIn = new System.Windows.Media.Animation.DoubleAnimation(0, 1, TimeSpan.FromSeconds(.8));
            dark.BeginAnimation(Window.OpacityProperty, fadeIn);

            this.Close();
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Optional: close it, or leave as is
        }



        private void NotifToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        private void EditAccountButton_Click(object sender, RoutedEventArgs e)
        {
            EditProfile settings = new EditProfile();
            settings.Show();    
            this.Close();   
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Login back = new Login();
            back.Show();
            this.Close();
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
