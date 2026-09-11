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
    /// Interaction logic for RegistrarSchedule.xaml
    /// </summary>
    public partial class RegistrarSchedule : Window
    {

        private readonly int _accountId;
        private readonly string _cs =
            Properties.Settings.Default.OnlyTableConnectionString;

        public RegistrarSchedule(int accountId)
        {
            InitializeComponent();

            _accountId = accountId;

            LoadStudentInfo();
            LoadSchedule();
            LoadAvailableSubjects();
        }

        private void LoadStudentInfo()
        {
            using (var conn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("dbo.usp_GetStudentInfo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountID", _accountId);
                conn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return;
                    txtStudentNumber.Text = rdr["Student_ID"].ToString();
                    txtName.Text = $"{rdr["First_Name"]} {rdr["Last_Name"]}";
                    txtCourseYear.Text = $"{rdr["Course_Type"]} {rdr["Course_YearLevel"]}";
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
            lstSubjects.ItemsSource = dt.DefaultView;
        }

        private void LoadAvailableSubjects()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_cs))
            using (var da = new SqlDataAdapter("dbo.usp_GetAvailableSubjects", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@AccountID", _accountId);
                da.Fill(dt);
            }

            ListSubjectsComboBox.ItemsSource = dt.DefaultView;
            ListSubjectsComboBox.DisplayMemberPath = "Subject";
            ListSubjectsComboBox.SelectedValuePath = "SubjectID";
        }

        private void ListSubjectsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (ListSubjectsComboBox.SelectedValue == null)
                return;

            int subjectId = Convert.ToInt32(ListSubjectsComboBox.SelectedValue);

            using (var conn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("dbo.usp_AddStudentSchedule", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountID", _accountId);
                cmd.Parameters.AddWithValue("@SubjectID", subjectId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Reload updated schedule and available subjects
            LoadSchedule();
            LoadAvailableSubjects();

            // Optional: clear ComboBox selection
            ListSubjectsComboBox.SelectedIndex = -1;
        }
    }
}
