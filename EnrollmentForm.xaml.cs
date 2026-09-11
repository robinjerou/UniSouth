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
using System.Data;

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for EnrollmentForm.xaml
    /// </summary>
    public partial class EnrollmentForm : Window
    {
      
        DataClasses1DataContext _dbConn = null;
        public EnrollmentForm()
        {
            InitializeComponent();
            _dbConn = new DataClasses1DataContext(Properties.Settings.Default.OnlyTableConnectionString);
        }

       

        private void SubmitFormButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn =
                       new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_AddEnrollmentForm", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    // core profile fields
                    cmd.Parameters.AddWithValue("@Account_ID", UserSession.AccountId);
                    cmd.Parameters.AddWithValue("@Last_Name", LastNameTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@First_Name", FirstNameTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Middle_Name", MiddleNameTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Birthdate", BirthdatePicker.SelectedDate ?? DateTime.Now);
                    cmd.Parameters.AddWithValue("@Gender", GenderComboBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Religion", ReligionTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Nationality", NationalityTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Academic_Year", AcademicYear.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", AddressTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", PhoneTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Birth_Place", BirthPlaceTextBox.Text.Trim());

                    // nullable/date/number fields


                    if (!int.TryParse(AgeTextBox.Text.Trim(), out int ageValue))
                        ageValue = 0;
                    cmd.Parameters.AddWithValue("@Age", ageValue);

                    // only Course_Type and Course_YearLevel
                    cmd.Parameters.AddWithValue(
                        "@Course_Type",
                        CourseComboBox.Text.Trim()
                    );
                    cmd.Parameters.AddWithValue(
                        "@Course_YearLevel",
                        YearLevelComboBox.Text.Trim()
                    );

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "Enrollment saved successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                DocumentUpload file = new DocumentUpload();
                file.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error saving enrollment:\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            LastNameTextBox.Clear();
            FirstNameTextBox.Clear();
            MiddleNameTextBox.Clear();
            BirthdatePicker.SelectedDate = null;
            GenderComboBox.SelectedIndex = -1;
            ReligionTextBox.Clear();
            NationalityTextBox.Clear();
            AcademicYear.Clear();
            AddressTextBox.Clear();
            PhoneTextBox.Clear();
            AgeTextBox.Clear();
            BirthPlaceTextBox.Clear();
            CourseComboBox.SelectedIndex = -1;
            YearLevelComboBox.SelectedIndex = -1;
        }

        //private void SubmitFormButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.EnrollmentConnectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand("usp_AddEnrollmentForm", conn);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@Account_ID", _accountId);
        //            cmd.Parameters.AddWithValue("@Last_Name", LastNameTextBox.Text.Trim());
        //            cmd.Parameters.AddWithValue("@First_Name", FirstNameTextBox.Text.Trim());
        //            cmd.Parameters.AddWithValue("@Middle_Name", MiddleNameTextBox.Text.Trim());
        //            cmd.Parameters.AddWithValue("@Birthdate", BirthdatePicker.SelectedDate ?? DateTime.Now);
        //            cmd.Parameters.AddWithValue("@Gender", GenderComboBox.Text);
        //            cmd.Parameters.AddWithValue("@Religion", ReligionTextBox.Text.Trim());
        //            cmd.Parameters.AddWithValue("@Nationality", NationalityTextBox.Text.Trim());
        //            cmd.Parameters.AddWithValue("@Address", AddressTextBox.Text.Trim());
        //            cmd.Parameters.AddWithValue("@Phone", PhoneTextBox.Text.Trim());
        //            cmd.Parameters.AddWithValue("@Age", Convert.ToInt32(AgeTextBox.Text));
        //            cmd.Parameters.AddWithValue("@Birth_Place", BirthPlaceTextBox.Text.Trim());
        //            cmd.Parameters.AddWithValue("@Course", CourseComboBox.Text);
        //            cmd.Parameters.AddWithValue("@YearLevel", YearLevelComboBox.Text);
        //            //cmd.Parameters.AddWithValue("@Semester", "1st Sem"); // or bind this from UI
        //            //cmd.Parameters.AddWithValue("@SchoolYear_ID", 1);   // hardcoded or dynamically set

        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //        }

        //        MessageBox.Show("Enrollment saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        //        DocumentUpload file = new DocumentUpload(_accountId);
        //        file.Show();
        //        this.Close(); // Optional: close window or redirect
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error saving enrollment:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

    }
}

