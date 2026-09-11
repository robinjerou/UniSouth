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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for RegistrarAddSubject.xaml
    /// </summary>
    public partial class RegistrarAddSubject : Window
    {
        private int _selectedSubjectId = 0;
        private readonly string _cs =
            Properties.Settings.Default.OnlyTableConnectionString;
        public RegistrarAddSubject()
        {
            InitializeComponent();
            LoadSubjects();
        }

        private void LoadSubjects()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(_cs))
            using (var da = new SqlDataAdapter("dbo.usp_GetSubjects", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
            }

            // Hide any rows where the Subject column is null or empty
            dt.DefaultView.RowFilter = "Subject IS NOT NULL AND Subject <> ''";

            lstSubjects.ItemsSource = dt.DefaultView;
        }

        private void btnAddSubject_Click(object sender, RoutedEventArgs e)
        {
            //string name = txtSubjectName.Text.Trim();
            //if (!int.TryParse(txtUnits.Text.Trim(), out int units))
            //{
            //    MessageBox.Show("Units must be a number.");
            //    return;
            //}
            //string code = txtLab.Text.Trim();
            //string time = txtTime.Text.Trim();

            try
            {
                using (var conn = new SqlConnection(_cs))
                using (var cmd = new SqlCommand("dbo.usp_AddSubject", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectName", txtSubjectName.Text);
                    cmd.Parameters.AddWithValue("@Units", txtUnits.Text);
                    cmd.Parameters.AddWithValue("@Lab", txtLab.Text);
                    cmd.Parameters.AddWithValue("@Time", txtTime.Text);
                    cmd.Parameters.AddWithValue("@Day", txtDay.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Subject added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();
                LoadSubjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding subject:\n" + ex.Message);
            }
        }

        private void btnUpdateSubject_Click(object sender, RoutedEventArgs e)
        {
            //if (_selectedSubjectId == 0) return;

            //string name = txtSubjectName.Text.Trim();
            //if (!int.TryParse(txtUnits.Text.Trim(), out int units))
            //{
            //    MessageBox.Show("Units must be a number.");
            //    return;
            //}
            //string code = txtLab.Text.Trim();
            //string time = txtTime.Text.Trim();

            if (_selectedSubjectId == 0) return;

            using (var conn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("dbo.usp_UpdateSubject", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubjectID", _selectedSubjectId);
                cmd.Parameters.AddWithValue("@SubjectName", txtSubjectName.Text.Trim());
                cmd.Parameters.AddWithValue("@Units", int.Parse(txtUnits.Text.Trim()));
                cmd.Parameters.AddWithValue("@Lab", txtLab.Text.Trim());
                cmd.Parameters.AddWithValue("@Time", txtTime.Text.Trim());
                cmd.Parameters.AddWithValue("@Day", txtDay.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Subject updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearForm();
            LoadSubjects();
        }

        private void lstSubjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = lstSubjects.SelectedItem as DataRowView;
            if (row == null)
            {
                ClearForm();
                return;
            }

            // grab the hidden PK
            _selectedSubjectId = Convert.ToInt32(row["SubjectID"]);

            // populate textboxes
            txtSubjectName.Text = row["Subject"].ToString();
            txtUnits.Text = row["Unit"].ToString();
            txtLab.Text = row["Lab"].ToString();
            txtTime.Text = row["Time"].ToString();
            txtDay.Text = row["Day"].ToString();

            btnUpdateSubject.IsEnabled = true;
        }

        //private void lstSubjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (lstSubjects.SelectedItem is DataRowView row)
        //    {
        //        // The Account_ID used as SubjectID

        //        txtSubjectName.Text = row["SubjectName"].ToString();
        //        txtUnits.Text = row["Units"].ToString();
        //        txtLab.Text = row["Lab"].ToString();
        //        txtTime.Text = row["Time"].ToString();

        //        btnUpdateSubject.IsEnabled = true;
        //    }
        //    else
        //    {
        //        ClearForm();
        //    }
        //}

        private void ClearForm()
        {
            txtSubjectName.Clear();
            txtUnits.Clear();
            txtLab.Clear();
            txtTime.Clear();
            btnUpdateSubject.IsEnabled = false;
            txtDay.Clear();
        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            RegistrarAccountRecord registrar = new RegistrarAccountRecord();    
            registrar.Show();
            this.Close();
        }
    }
}
