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
    /// Interaction logic for RegistrarStudentInfoList.xaml
    /// </summary>
    public partial class RegistrarStudentInfoList : Window
    {
        public RegistrarStudentInfoList()
        {
            InitializeComponent();
            LoadStudents();
        }


        private void LoadStudents()
        {
            // 1) create a DataTable to hold the results
            DataTable dt = new DataTable();

            // 2) fill it via your proc
            using (var conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
            using (var cmd = new SqlCommand("dbo.usp_GetAllStudents", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
            }

            // 3) bind the DataTable's DefaultView to your ListView
            StudentListView.ItemsSource = dt.DefaultView;
        }

        private void StudentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Make sure an item is actually selected
            if (StudentListView.SelectedItem is DataRowView row)
            {
                int studentId = Convert.ToInt32(row["StudentID"]);
                RegistrarSchedule scheduleWindow = new RegistrarSchedule(studentId);
                scheduleWindow.Show();
                this.Close();
            }
        }

        private void btnUpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string username = txtUsername.Text.Trim();
           

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(username)) 
               
            {
                MessageBox.Show("All fields (Email, Username, Student Status) are required.", "Missing Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.usp_UpdateUsernameAndStatusByEmail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Bind the parameters
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Username", username);
                   

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Student record updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadStudents(); // Refresh the ListView
                    }
                    else
                    {
                        MessageBox.Show("No student found with that email.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            RegistrarAccountRecord registrar = new RegistrarAccountRecord();
            registrar.Show();
            this.Close();
        }
    }
}
