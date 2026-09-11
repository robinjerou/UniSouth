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
using Microsoft.Win32;
using System.IO;

namespace EventDrivenProgEnrollmentSystem
{
    /// <summary>
    /// Interaction logic for DocumentUpload.xaml
    /// </summary>
    public partial class DocumentUpload : Window
    {
      
        private readonly List<string> _selectedFiles = new List<string>();

        public DocumentUpload()
        {
            InitializeComponent();
          
        }

        private void FileUploadButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Select document(s) to upload",
                Filter = "PDF files (*.pdf)|*.pdf|Word documents (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
                Multiselect = true
            };

            if (dlg.ShowDialog() == true)
            {
                _selectedFiles.Clear();
                _selectedFiles.AddRange(dlg.FileNames);

                MessageBox.Show(
                  $"Selected {_selectedFiles.Count} file(s). Click Submit to upload.",
                  "Files Ready",
                  MessageBoxButton.OK,
                  MessageBoxImage.Information
                );
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            
            Schedule sched = new Schedule(UserSession.AccountId);
            sched.Show();
            this.Close();
            //if (_selectedFiles.Count == 0)
            //{
            //    MessageBox.Show(
            //        "Please select at least one file first.",
            //        "No Files",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Warning
            //    );
            //    return;
            //}

            //try
            //{
            //    using (var conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
            //    {
            //        conn.Open();

            //        foreach (var filePath in _selectedFiles)
            //        {
            //            byte[] fileBytes = File.ReadAllBytes(filePath);
            //            string fileName = System.IO.Path.GetFileName(filePath);
            //            DateTime now = DateTime.Now;

            //            using (var cmd = new SqlCommand(@"
            //            INSERT INTO Documents
            //               (Account_ID, FileName, [Content], UploadDate)
            //            VALUES
            //               (@Account,  @Name,     @Content,   @UploadDate)", conn))
            //            {
            //                cmd.Parameters.AddWithValue("@Account", _accountId);
            //                cmd.Parameters.AddWithValue("@Name", fileName);
            //                cmd.Parameters
            //                   .Add("@Content", SqlDbType.VarBinary, fileBytes.Length)
            //                   .Value = fileBytes;
            //                cmd.Parameters.AddWithValue("@UploadDate", now);

            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //    }

            //    MessageBox.Show(
            //        "Upload successful!",
            //        "Done",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Information
            //    );

            //    // return to main/options window
            //    var options = new MainWindow();
            //    options.Show();
            //    this.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(
            //        "Error uploading files:\n" + ex.Message,
            //        "Upload Failed",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Error
            //    );
            //}

            //private void SubmitButton_Click(object sender, RoutedEventArgs e)
            //{
            //    if (_selectedFiles.Count == 0)
            //    {
            //        MessageBox.Show("Please select at least one file first.",
            //                        "No Files",
            //                        MessageBoxButton.OK,
            //                        MessageBoxImage.Warning);
            //        return;
            //    }

            //    try
            //    {
            //        using (var conn = new SqlConnection(Properties.Settings.Default.OnlyTableConnectionString))
            //        {
            //            conn.Open();

            //            foreach (var filePath in _selectedFiles)
            //            {
            //                // Fully-qualified so there’s no ambiguity:
            //                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //                string fileName = System.IO.Path.GetFileName(filePath);

            //                using (var cmd = new SqlCommand(@"
            //                    INSERT INTO Documents (Account_ID, FileName, Content)
            //                    VALUES (@Account, @Name, @Content)", conn))
            //                {
            //                    cmd.Parameters.AddWithValue("@Account", _accountId);
            //                    cmd.Parameters.AddWithValue("@Name", fileName);
            //                    cmd.Parameters
            //                       .Add("@Content", System.Data.SqlDbType.VarBinary, fileBytes.Length)
            //                       .Value = fileBytes;

            //                    cmd.ExecuteNonQuery();
            //                }
            //            }
            //        }

            //        MessageBox.Show("Upload successful!",
            //                        "Done",
            //                        MessageBoxButton.OK,
            //                        MessageBoxImage.Information);

            //        MainWindow options = new MainWindow();
            //        options.Show();
            //        this.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error uploading files:\n" + ex.Message,
            //                        "Upload Failed",
            //                        MessageBoxButton.OK,
            //                        MessageBoxImage.Error);
            //    }
            //}



            //private void FileUploadButton_Click(object sender, RoutedEventArgs e)
            //{
            //    // configure the open‐file dialog
            //    OpenFileDialog dlg = new OpenFileDialog
            //    {
            //        Title = "Select document(s) to upload",
            //        Filter = "PDF files (*.pdf)|*.pdf|Word documents (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
            //        Multiselect = true
            //    };


            //    if (dlg.ShowDialog() == true)
            //    {
            //        _selectedFiles.Clear();
            //        _selectedFiles.AddRange(dlg.FileNames);

            //        MessageBox.Show(
            //          $"Selected {_selectedFiles.Count} file(s). Click Submit to upload.",
            //          "Files Ready",
            //          MessageBoxButton.OK,
            //          MessageBoxImage.Information
            //        );
            //    }


            //    // show it
            //    //bool? result = dlg.ShowDialog();

            //    //if (result == true)
            //    //{
            //    //    // dlg.FileNames is a string[]
            //    //    // e.g. display their names or store them somewhere
            //    //    string[] paths = dlg.FileNames;

            //    //    // For demo, just show how many files were picked:
            //    //    MessageBox.Show(
            //    //        $"You selected {paths.Length} file(s):\n{string.Join("\n", paths)}",
            //    //        "Files Selected",
            //    //        MessageBoxButton.OK,
            //    //        MessageBoxImage.Information
            //    //    );

            //    //    // TODO: upload the files, copy them, etc.
            //    //}
            //}
        }
    }
}
