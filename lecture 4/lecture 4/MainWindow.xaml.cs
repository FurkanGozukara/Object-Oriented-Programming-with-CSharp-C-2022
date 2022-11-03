using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace lecture_4
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

        public class exampleGC
        {
            public int MyProperty { get; set; }
            public string srProp { get; set; } = "this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a test this is a testthis is a test this is a test this is a test";
        }

        //make this application responsive while running
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Button_Click " + Thread.CurrentThread.ManagedThreadId);

            Task.Factory.StartNew(async () =>
            {
                for (int i = 0; i < 10000000; i++)
                {
                    Debug.WriteLine("inside for loop " + Thread.CurrentThread.ManagedThreadId);


                    //start task whenever possible so this line will be immediately executed and it will continue with executing of next line
                    Task.Factory.StartNew(() => { testGG(); });

                    //however in this case, it will wait until task is completed 
                    await Task.Factory.StartNew(() => { testGG(); });

                    System.Threading.Thread.Sleep(1);
                    if (GCList.Count > 1000)
                    {
                        GCList.Clear();
                        GC.Collect();
                    }
                }
            });
        }

        List<exampleGC> GCList = new List<exampleGC>();

        private void testGG()
        {
            Debug.WriteLine("testGG " + Thread.CurrentThread.ManagedThreadId);

            //make this application crash due to memory error
            exampleGC exampleGC = new exampleGC();
            exampleGC.MyProperty = new Random().Next();
            for (int i = 0; i < 10; i++)
            {
                exampleGC.srProp += exampleGC.srProp;
            }
            System.Threading.Thread.Sleep(1111);
            GCList.Add(exampleGC);
        }

        async private Task<string> downloadSite(string srUrl)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(new Uri(srUrl));
            }
        }

        async private void btnDownloadAsyncString_Click(object sender, RoutedEventArgs e)
        {
            string srUrl = "https://www.monstermmorpg.com";
            string code = downloadSite(srUrl).GetAwaiter().GetResult();

            var vrResult2 = Task.Run(async () =>
            {
                string msg = await downloadSite(srUrl);
                return msg;
            }).Result;

            var vrResult = await downloadSite(srUrl);

            string srResult2;

            downloadSite(srUrl)
                .ContinueWith(t =>
                {
                    srResult2 = t.Result;
                })
                .Wait();

            MessageBox.Show(code);
        }
        List<int> lstInts;
        long irRunningTaskCount = 0;
        private async void btnParallelForEach_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            lstInts = new List<int>(Enumerable.Range(1, 1000).Select(pr => rnd.Next()));

            lstInts = new List<int>(Enumerable.Range(1, 1000));

            ParallelOptions PO = new ParallelOptions();
            PO.MaxDegreeOfParallelism = 4;

            //Parallel.ForEach(lstInts, PO, async number =>
            //{
            //    await debugFile(number);
            //});

            long lrCounter = 0;

            //our aim is that running 10 tasks simultanously and with the order they should start
            //so whenever a task is completed, start another task and keep running task count at 10

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += taskScheduler;
            timer.Start();
        }

        DispatcherTimer timer;
        private static List<Task> lstOurRunningTasks = new List<Task>();
        int irMaxSimultanousTaskCount = 20;

        //this method will be called every 1 miliseconds
        private void taskScheduler(object? sender, EventArgs e)
        {
            if (Interlocked.Read(ref irRunningTaskCount) < irMaxSimultanousTaskCount)
            {
                lock (lstInts)//this will ensure thread synchronization between different calls of taskScheduler
                {
                    if (lstInts.Count == 0)
                    {
                        timer.Stop();
                        return;
                    }

                    int irLocal = lstInts.FirstOrDefault();//this way with locatization we are preventing data in task starting
                    lstInts.RemoveAt(0);
                    Interlocked.Add(ref irRunningTaskCount, +1);
                    var vrTask = Task.Factory.StartNew(() => debugFile(irLocal)).ContinueWith(task =>
                      {

                          Interlocked.Add(ref irRunningTaskCount, -1);

                      });

                    lstOurRunningTasks.Add(vrTask);
                }
            }
        }

        async private Task<int> debugFile(int irVal)
        {
            Debug.WriteLine(irVal);


            int irRunningCount = lstOurRunningTasks.Where(pr => pr.Status == TaskStatus.Running).Count<Task>();
            int irCompletedCount = lstOurRunningTasks.Where(pr => pr.Status == TaskStatus.RanToCompletion).Count<Task>();
            int irWaiting = lstOurRunningTasks.Where(pr => pr.Status == TaskStatus.WaitingForActivation).Count<Task>();
            Debug.WriteLine($"running: {irRunningCount} , completed : {irCompletedCount} , WaitingForActivation : {irWaiting}");

            await downloadSite("https://www.toros.edu.tr");

            await justWait(irVal);
            return irVal;
        }

        async private Task<bool> justWait(int irVal)
        {
            System.Threading.Thread.Sleep(irVal * 100);
            return true;
        }
    }

    public static class ScheduledTaskAccess
    {
        public static Task[] GetScheduledTasksForDebugger(TaskScheduler ts)
        {
            var mi = ts.GetType().GetMethod("GetScheduledTasksForDebugger");
            if (mi == null)
                return null;
            return (Task[])mi.Invoke(ts, new object[0]);
        }
    }
}
