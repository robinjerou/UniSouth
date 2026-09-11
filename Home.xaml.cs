using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        //private int _accountId;
        //private string _studentName;
        private readonly string _cs =
            Properties.Settings.Default.EnrollmentConnectionString;

        public Home()
        {
            InitializeComponent();
            //_accountId = accountId;
            LoadStudentName();

            LoadStudentImage();
        }

        private void LoadStudentName()
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
            {
                conn.Open();

                string query = "SELECT First_Name, Last_Name FROM OneTable WHERE Account_ID = @AccountID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AccountID", UserSession.AccountId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader["First_Name"].ToString();
                            string lastName = reader["Last_Name"].ToString();
                            txtStudentName.Text = $"{firstName} {lastName}";

                        }
                    }
                }
            }
        }

        //private void LoadStudentName()
        //{
        //    using (SqlConnection conn = new SqlConnection(_cs))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("dbo.usp_GetStudentName", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;


        //            conn.Open();
        //            using (SqlDataReader rdr = cmd.ExecuteReader())
        //            {
        //                if (rdr.Read())
        //                {
        //                    string first = rdr.GetString(0);
        //                    string last = rdr.GetString(1);
        //                    txtStudentName.Text = $"{first} {last}";
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

        private void Star_Click(object sender, RoutedEventArgs e)
        {
            var clicked = (ToggleButton)sender;
            int rating = int.Parse(clicked.Tag.ToString());

            foreach (ToggleButton star in StarsPanel.Children.OfType<ToggleButton>())
            {
                int value = int.Parse(star.Tag.ToString());
                star.IsChecked = value <= rating;
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            FeedbackMessage survey = new FeedbackMessage(); 
            survey.Show();  
        }

        private void SocialMediaButton_Click(object sender, RoutedEventArgs e)
        {
            // this will open the default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.facebook.com/sisc.monarchs",
                UseShellExecute = true
            });
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
                            ellipseHomeProfile.Fill = new ImageBrush(bitmap);
                        }
                    }
                }
            }
        }





    }
}
