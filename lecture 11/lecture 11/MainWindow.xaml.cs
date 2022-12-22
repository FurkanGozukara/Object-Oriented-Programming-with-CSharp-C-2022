using HtmlAgilityPack;
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

namespace lecture_11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();
        static readonly List<int> list = new List<int> { 32, 12 };
        static readonly int max = 300;
        public MainWindow()
        {
            InitializeComponent();
            client.Timeout = new TimeSpan(0, 2, 0);
        }

        private async void btntimeout_Click(object sender, RoutedEventArgs e)
        {
            list.Add(3); // this works because readonly protects only the reference holding pointer object
                         //  max = 334; you cant do this because it is already a value type
                         //   list = null; this wont work because readonly protecting the pointer object
                         //  list = new List<int>();this wont work because readonly protecting the pointer object

            var source = await returnSourceCode("https://www.toros.edu.tr");
            MessageBox.Show(source);
        }

        //why message box didnt show anything
        //make this code in a way that it wont freeze the ui

        static async Task<string> returnSourceCode(string Url)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                using HttpResponseMessage response = await client.GetAsync(Url);              
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                return responseBody;
            }
            catch (Exception e)
            {                
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }
    }
}
