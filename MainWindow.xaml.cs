using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Diagnostics;
using System.Security.Policy;
using Microsoft.Win32;
using System.Reflection;

namespace URL_CallingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

      

        private async void send_btn_Click(object sender, RoutedEventArgs e)
        {
            Int32 successcount = 0;
            Int32 failcount = 0;
            if (filepath_txt.Text == "")
            {
                MessageBox.Show("Please select a File to process");
                filepath_txt.Focus();   
            }
            else
            {
                try
                {

                    var urls = File.ReadAllLines(filepath_txt.Text);
                    foreach (string url in urls)
                    {
                        string sedURL = URL_txt.Text + url;

                        var myClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
                        var response = await myClient.GetAsync(sedURL);
                        var streamResponse = await response.Content.ReadAsStreamAsync();
                        string result = Encoding.UTF8.GetString((streamResponse as MemoryStream).ToArray());

                        if (result.Contains(result_chk_txt.Text))
                        {
                            successcount++;
                            success_txt.Text = successcount.ToString();
                        }
                        else
                        {
                            failcount++;
                            fail_txt.Text = failcount.ToString();
                            string errorpath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "errorURL.txt";
                            File.AppendAllText(errorpath, sedURL + Environment.NewLine);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CloseWindow_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void fileselect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Text File | *.txt";
            Boolean? isselected = opf.ShowDialog();
            if (isselected == true)
            {
                MessageBox.Show("File Selected.");
                string Fullfilename = opf.FileName;
                string onlyfilename = opf.SafeFileName;

                filepath_txt.Text = Fullfilename;
            }
        }
    }
}