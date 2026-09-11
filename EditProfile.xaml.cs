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
using System.IO;

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Window
    {
     

        public EditProfile()
        {
            InitializeComponent();
       

            LoadStudentInfo();
        }

        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
            {
                string query = @"UPDATE OneTable
                         SET First_Name = @FirstName,
                             Last_Name = @LastName,
                             Email = @Email,
                             Birthdate = @Birthdate,
                             Phone = @Phone,
                             Gender = @Gender,
                             Address = @Address,
                             Course_Type = @Course,
                             Course_YearLevel = @Year
                         WHERE Account_ID = @AccountID";

                using (SqlCommand cmd = new SqlCommand(query, conn))    
                {
                    cmd.Parameters.AddWithValue("@AccountID", UserSession.AccountId);
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Birthdate", txtBirthdate.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@Gender", txtGender.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Course", txtCourse.Text);
                    cmd.Parameters.AddWithValue("@Year", txtYearLevel.Text);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        Settings profile = new Settings();
                        profile.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Update failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
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

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (dlg.ShowDialog() == true)
            {
                string filePath = dlg.FileName;

                // Load the image into the UI preview
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                ellipseProfile.Fill = new ImageBrush(bitmap); // show preview

                // Save the image to the database
                byte[] imageData = File.ReadAllBytes(filePath);

                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
                {
                    string query = "UPDATE OneTable SET ProfileImage = @Image WHERE Account_ID = @AccountID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountID", UserSession.AccountId);
                        cmd.Parameters.AddWithValue("@Image", imageData);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Profile Image Uploaded.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
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
            // optional
        }

        private void EditAccountButton_Click(object sender, RoutedEventArgs e)
        {
            EditProfile settings = new EditProfile();
            settings.Show();
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Settings profile = new Settings();
            profile.Show();
            this.Close();
        }
    }
}
