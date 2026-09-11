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
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for Transaction.xaml
    /// </summary>
    public partial class Transaction : Window
    {
        
        public Transaction()
        {
            InitializeComponent();
            
        }

        //private void PaymentOptionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (PaymentOptionsComboBox.SelectedItem is ComboBoxItem selectedItem)
        //    {
        //        string choice = selectedItem.Content as string;
        //        Window nextWindow = null;

        //        switch (choice)
        //        {
        //            case "Card":
        //                nextWindow = new Card();    // your window for card payments
        //                break;
        //            case "Mobile":
        //                nextWindow = new EWallet();  // your window for mobile payments
        //                break;
        //        }

        //        if (nextWindow != null)
        //        {
        //            nextWindow.Show();
        //            this.Close();  // optional: close the Transaction window
        //        }
        //    }
        //}

        private void Visa_Click(object sender, RoutedEventArgs e)
        {
            EWallet payment = new EWallet();
            payment.Show();
            this.Close();
        }

        private void Paypal_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Amex_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Confirmation_Click(object sender, RoutedEventArgs e)
        {
            //string email = txtEmail.Text.Trim();
            //ComboBoxItem selectedPlan = PaymentOptionsComboBox.SelectedItem as ComboBoxItem;

            //if (string.IsNullOrWhiteSpace(email) || selectedPlan == null)
            //{
            //    MessageBox.Show("Please enter a valid email and select a payment plan.");
            //    return;
            //}

            //string academicYear = "2021";
            //string studentId = "0156";
            //string username = $"{academicYear.Substring(2)}-{studentId}c";
            //string selectedPlanText = selectedPlan.Content.ToString();

            //string subject = "UniSouth Payment Plan Confirmation";
            //string body = $"Dear Student,\n\nThank you for choosing a payment plan.\n\nUsername: {username}\nPlan: {selectedPlanText}\n\n- UniSouth Enrollment Team";

            //try
            //{
            //    MailMessage mail = new MailMessage();
            //    mail.From = new MailAddress("youradminemail@gmail.com");
            //    mail.To.Add(email);  // Only sends to this email
            //    mail.Subject = subject;
            //    mail.Body = body;

            //    SmtpClient smtp = new SmtpClient("smtp.gmail.com")
            //    {
            //        Port = 587,
            //        Credentials = new NetworkCredential("youradminemail@gmail.com", "your_app_password"),
            //        EnableSsl = true
            //    };

            //    smtp.Send(mail);
            //    MessageBox.Show("Email sent successfully!");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);
            //}

            MessageBox.Show("Email sent successfully!", "Proceed", MessageBoxButton.OK, MessageBoxImage.Information);
            MessageEmail next = new MessageEmail(); 
            next.Show();
            this.Close();
        }

    }
}
