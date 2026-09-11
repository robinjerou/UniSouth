using System;
using System.Collections.Generic;
using System.Data;
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

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for AcademicRecord.xaml
    /// </summary>
    public partial class AcademicRecord : Window
    {
        //private int _accountId;

        public AcademicRecord()//int accountId)
        {
            InitializeComponent();
            //_accountId = accountId;
            LoadAcademicInfo();
        }

        private void LoadAcademicInfo()
        {
            var dt = new DataTable();

            using (var conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
            using (var da = new SqlDataAdapter("dbo.usp_GetStudentSchedule", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@AccountID", UserSession.AccountId);
                da.Fill(dt);
            }

            lbAcademicRecord.ItemsSource = dt.DefaultView;
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
    }
}
