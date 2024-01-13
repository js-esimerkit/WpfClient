using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string token = null;
        public MainWindow()
        {
            InitializeComponent();
        }
        static async Task<string> LoginHttp(string u, string p)
        {

            var response = string.Empty;

            User objectUser = new User(u, p);

            var json = JsonConvert.SerializeObject(objectUser);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://localhost:5028/api/login/";

            var client = new HttpClient();
            try
            {
                HttpResponseMessage result = await client.PostAsync(url, postData);
                response = await result.Content.ReadAsStringAsync();
                return response;
            }
            catch (Exception)
            {
                return "-1";
            }

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string u = textUsername.Text;
            string p = textPassword.Text;

            var data = Task.Run(() => LoginHttp(u, p));
            data.Wait();

            string response = data.Result;
            if (string.Compare(response, "-1") == 0)
            {
                MessageBox.Show("No connection to server");
            }
            else
            {
                if (string.Compare(response, "0") == 0)
                {
                    MessageBox.Show("No connection to Database");
                }
                else
                {
                    if (string.Compare(response, "false") == 0)
                    {
                        labelResult.Content = "Wrong username/password";
                    }
                    else
                    {
                        labelResult.Content = "Login OK";
                        token = "Bearer " + response;
                    }
                }
            }
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            textUsername.Text = string.Empty;
            textPassword.Text = string.Empty;
            gridBooks.Columns.Clear();
            token = null;
        }
        static async Task<string> GetBookFromApi(string token)
        {
            var response = string.Empty;
            var url = "http://localhost:5028/api/book/";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            HttpResponseMessage result = await client.GetAsync(url);
            response = await result.Content.ReadAsStringAsync();
            return response;
        }

        private void btnShowBooks_Click(object sender, RoutedEventArgs e)
        {
            if (token == null)
            {
                MessageBox.Show("You have to login first");
            }
            else
            {
                var data = Task.Run(() => GetBookFromApi(token));
                data.Wait();
                Console.WriteLine(data.Result);
                if (data.Result.Length > 3) //Result is not []
                {
                    dynamic books = JsonConvert.DeserializeObject(data.Result);

                    gridBooks.ItemsSource = books;//writes the data to DataGrid
                }
                else
                {
                    MessageBox.Show("There is no books");
                }
            }
        }
    }
}
