using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for Schedule.xaml
    /// </summary>
    public partial class Schedule : Window
    {
        private readonly int _accountId;
        private readonly string _cs = Properties.Settings.Default.OnlyTableConnectionString;

        public Schedule(int accountId)
        {
            InitializeComponent();
            _accountId = accountId;

            LoadStudentInfo();
            LoadSchedule();
        }

        private void LoadStudentInfo()
        {
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_GetStudentInfo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountID", _accountId);
                    conn.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            string fullName = $"{rdr["First_Name"]} {rdr["Last_Name"]}";
                            string course = $"{rdr["Course_Type"]} {rdr["Course_YearLevel"]}";
                            txtStudentHeader.Text = $"{fullName} – {course}";
                        }
                    }
                }
            }
        }

        private void LoadSchedule()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_cs))
            using (var da = new SqlDataAdapter("dbo.usp_GetStudentSchedule", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@AccountID", _accountId);
                da.Fill(dt);
            }
            lbSchedule.ItemsSource = dt.DefaultView;
        }


 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Transaction tuition = new Transaction();
            tuition.Show();
            this.Close();
        }
    }

}
