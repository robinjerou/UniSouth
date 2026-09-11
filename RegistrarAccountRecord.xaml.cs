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
    /// Interaction logic for RegistrarAccountRecord.xaml
    /// </summary>
    public partial class RegistrarAccountRecord : Window
    {
        private int _selectedRecordId = 0;
        private int _selectedAccountId = 0;
        private string _cs = Properties.Settings.Default.OnlyTableConnectionString;
        private DataTable _studentsTable;
        public RegistrarAccountRecord()
        {
            InitializeComponent();
            lstRecords.ItemsSource = null;
            LoadStudentNames();
        }

        private void LoadRecords()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_cs))
            using (var da = new SqlDataAdapter("dbo.usp_GetAccountRecords", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
            }
            lstRecords.ItemsSource = dt.DefaultView;
        }

        private void LoadAccountRecordByStudent(int accountId)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("usp_GetAccountRecordsByStudent", conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", accountId);
                adapter.Fill(dt);
            }

            lstRecords.ItemsSource = dt.DefaultView;
          
        }


        private void btnAddRecord_Click(object sender, RoutedEventArgs e)
        {
           if (_selectedAccountId == 0)
            {
                MessageBox.Show("Please search and select a student first.", "No Student", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSchoolYear.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("School Year and Description are required.");
                return;
            }

            if (!int.TryParse(txtAmount.Text, out var amount))
            {
                MessageBox.Show("Amount must be a number.");
                return;
            }

            if (!int.TryParse(txtBalance.Text, out var balance))
            {
                MessageBox.Show("Balance must be a number.");
                return;
            }


            using (var conn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("dbo.usp_AddAccountRecord", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Account_ID", _selectedAccountId);
                cmd.Parameters.AddWithValue("@AcademicYear", txtSchoolYear.Text.Trim());
                cmd.Parameters.AddWithValue("@PaymentMethod", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Balance", balance);
                cmd.Parameters.AddWithValue("@Status", txtStatus.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Record added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearForm();
            LoadAccountRecordByStudent(_selectedAccountId);
        }
      

        private void btnUpdateRecord_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRecordId == 0) return;

            if (string.IsNullOrWhiteSpace(txtSchoolYear.Text)
                || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("School Year and Description are required.");
                return;
            }
            if (!int.TryParse(txtAmount.Text, out var amount))
            {
                MessageBox.Show("Amount must be a number.");
                return;
            }
            if (!int.TryParse(txtBalance.Text, out var balance))
            {
                MessageBox.Show("Balance must be a number.");
                return;
            }

            using (var conn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("dbo.usp_UpdateAccountRecord", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RecordID", _selectedRecordId);
                cmd.Parameters.AddWithValue("@AcademicYear", txtSchoolYear.Text.Trim());
                cmd.Parameters.AddWithValue("@PaymentMethod", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Balance", balance);
                cmd.Parameters.AddWithValue("@Status", txtStatus.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Record updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearForm();
            LoadRecords();
        }

        private void ClearForm()
        {
            txtSchoolYear.Text = "";
            txtDescription.Text = "";
            txtAmount.Text = "";
            txtBalance.Text = "";
            txtStatus.Text = "";
            _selectedRecordId = 0;
            btnUpdateRecord.IsEnabled = false;
            lstRecords.SelectedIndex = -1;
        }

        private void lstRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRecords.SelectedItem is DataRowView row)
            {
                _selectedRecordId = Convert.ToInt32(row["RecordID"]);
                txtSchoolYear.Text = row["SchoolYear"].ToString();
                txtDescription.Text = row["Description"].ToString();
                txtAmount.Text = row["Amount"].ToString();
                txtBalance.Text = row["Balance"].ToString();
                txtStatus.Text = row["Status"].ToString();
                btnUpdateRecord.IsEnabled = true;
            }
            else
            {
                ClearForm();
            }
        }

        private void LoadStudentNames()
        {
            using (var conn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("SELECT Account_ID, First_Name + ' ' + Last_Name AS FullName FROM OneTable WHERE First_Name IS NOT NULL", conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                _studentsTable = new DataTable();
                adapter.Fill(_studentsTable);
            }

            lstSuggestions.ItemsSource = _studentsTable.DefaultView;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_studentsTable == null) return;

            string query = txtSearch.Text.ToLower();

            var rows = _studentsTable.AsEnumerable()
                .Where(row => row.Field<string>("FullName").ToLower().Contains(query));

            if (rows.Any() && !string.IsNullOrWhiteSpace(query))
            {
                lstSuggestions.ItemsSource = rows.CopyToDataTable().DefaultView;
                lstSuggestions.Visibility = Visibility.Visible;
            }
            else
            {
                lstSuggestions.Visibility = Visibility.Collapsed;
            }
        }

       private void lstSuggestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSuggestions.SelectedItem is DataRowView selected)
            {
                _selectedAccountId = Convert.ToInt32(selected["Account_ID"]); 
                LoadAccountRecordByStudent(_selectedAccountId);
                lstSuggestions.Visibility = Visibility.Collapsed;
                txtSearch.Text = selected["FullName"].ToString();
            }
        }

      




        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            RegistrarAccountRecord account = new RegistrarAccountRecord();
            account.Show();
            this.Close();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            RegistrarStudentInfoList students = new RegistrarStudentInfoList();
            students.Show();
            this.Close();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            RegistrarAddSubject subject = new RegistrarAddSubject();
            subject.Show();
            this.Close();
        }
    }
}
