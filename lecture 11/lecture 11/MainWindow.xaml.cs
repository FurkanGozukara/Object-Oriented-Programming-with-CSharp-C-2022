using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace lecture_11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public enum AppStatus
        {
            Start,
            Pause,
            Continue,
            Stop
        };

        public static AppStatus status = AppStatus.Continue;

        private AppStatus _status2;

        public AppStatus status2
        {
            get { return _status2; }
            set
            {
                _status2 = value;
                //simply call what you want done
                StatusPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void StatusPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        static readonly HttpClient client = new HttpClient();
        static readonly List<int> list = new List<int> { 32, 12 };
        static readonly int max = 300;

        private static readonly DispatcherTimer mainTimer = new DispatcherTimer();

        private static Timer accurateTimer;

        public MainWindow()
        {
            InitializeComponent();
            client.Timeout = new TimeSpan(0, 2, 0);//2 min timeout

            mainTimer.Tick += MainTimer_Tick;
            mainTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);


        }

        //if unhandled exception occurs inside here, it wont cause your application to crash because it is running in a sub thread
        //therefore from this method you cant directly access ui elements
        public void accurateTimerClick(Object stateInfo)
        {
            if (checkPause())
                return;

            writeRunningThread("threading timer");
            Debug.WriteLine(DateTime.Now.ToString("ss:fff"));

            //  lblThreadingtimer.Content = _counter_threading++.ToString("N0"); this will throw error

            lblThreadingtimer.Dispatcher.BeginInvoke(() =>
            {
                lblThreadingtimer.Content = _counter_threading++.ToString("N0");
            });
        }

        private static int _counter_wpf = 0, _counter_threading = 0;

        //this runs inside main method so any unhandled exception will cause application termination and you can directly access elements in the main thread
        private static bool checkPause()
        {
            if (status is AppStatus.Pause)
                return true;
            return false;
        }

        private void MainTimer_Tick(object? sender, EventArgs e)
        {
            if (checkPause())
                return;

            writeRunningThread("wpf timer");
            Debug.WriteLine(DateTime.Now.ToString("ss:fff"));

            //lblNumber.Content = _counter_wpf++.ToString("N0");

            lblNumber.Dispatcher.BeginInvoke(() =>
            {
                lblNumber.Content = _counter_wpf++.ToString("N0");
            });

        }

        private void writeRunningThread(string msg = "")
        {
            Debug.WriteLine($"running thread: {msg} " + Thread.CurrentThread.ManagedThreadId);
        }

        private async void btntimeout_Click(object sender, RoutedEventArgs e)
        {
            list.Add(3); // this works because readonly protects only the reference holding pointer object
                         //  max = 334; you cant do this because it is already a value type
                         //   list = null; this wont work because readonly protecting the pointer object
                         //  list = new List<int>();this wont work because readonly protecting the pointer object


            writeRunningThread();
            var source = await returnSourceCode("https://www.toros.edu.tr").ConfigureAwait(false);



            source = await Task.Factory.StartNew(() =>
            {
                return returnSourceCode("https://www.monstermmorpg.com");

            }, fetchToken).Result;

            MessageBox.Show(source?.Substring(0, 100));
        }

        CancellationToken fetchToken = new CancellationToken();

        //why message box didnt show anything
        //make this code in a way that it wont freeze the ui

        static async Task<List<string>> batchCrawl(string Url, CancellationToken token = default, int gg = default, string aa = null)
        {
            List<string> sourcCodes = new List<string>();

            for (int i = 0; i < 20; i++)
            {
                if (status.Equals(AppStatus.Stop))
                    break;

                var source = await (returnSourceCode("https://www.pokemonpets.com"));

                if (token.IsCancellationRequested) { break; }
            }

            return sourcCodes;
        }

        static async Task<string> returnSourceCode(string Url)
        {
            Debug.WriteLine("running thread: " + Thread.CurrentThread.ManagedThreadId);

            await Task.Delay(5555);
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
                MessageBox.Show("\nException Caught!");
                MessageBox.Show("Message :{0} ", e.Message);
            }
            return null;
        }



        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            mainTimer.Start();

            //accurateTimer?.Dispose();

            //if (accurateTimer is not null)
            //    accurateTimer.Dispose();

            if (accurateTimer is null)
                accurateTimer = new Timer(accurateTimerClick, null, 0, 100);
            else
                accurateTimer.Change(0, 100);
        }

        private void btnPauseWithoutModify_Click(object sender, RoutedEventArgs e)
        {
            status = (status == AppStatus.Pause) ? AppStatus.Continue : AppStatus.Pause;
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == nameof(btnStop))
            {
                _counter_wpf = 0;
                _counter_threading = 0;
            }
            accurateTimer.Change(Timeout.Infinite, Timeout.Infinite);
            mainTimer.Stop();
            //accurateTimer.Dispose();
        }

        private void btnSortCustomClass_Click(object sender, RoutedEventArgs e)
        {
            List<csStudents> studentList = new List<csStudents>
            {
                 new csStudents (11,"test"),
                 new csStudents (2,"ahmet"),
                 new csStudents (343, "hakan")
            };

            studentList.Sort(new testCompare<csStudents>());
        }

        public class testCompare<T> : IComparer<csStudents>
        { 
            public int Compare(csStudents x , csStudents y)
            {
                return x.studentId.CompareTo(y.studentId);
            }
        }

        public class csStudents : IComparer<(int,string)>
        {
            public csStudents()
            {

            }
            public csStudents(int _studentId, string _studentName)
            {
                studentId = _studentId;
                studentName = _studentName;
            }
            public int studentId { get; set; }
            public string studentName { get; set; }

            public int Compare((int, string) x, (int, string) y)
            {
                return x.Item1.CompareTo(y.Item1);
            }
        }
    }
}
