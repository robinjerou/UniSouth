using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


 
        public MainWindow(int accountId)
        {
            InitializeComponent();
          
        }

        private void Button_1(object sender, RoutedEventArgs e)
        {
            EnrollmentForm enroll = new EnrollmentForm();
            enroll.Show();
            this.Close();   
        }

        private void Button_2(object sender, RoutedEventArgs e)
        {
            RegistrarAccountRecord select = new RegistrarAccountRecord();
            select.Show();
            this.Close();
        }
    }
}
