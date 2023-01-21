using lecture_14_crawler_ex.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.Pkcs;
using System.Security.Policy;
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

namespace lecture_14_crawler_ex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow _thisWindow;

        public MainWindow()
        {
            InitializeComponent();
            _thisWindow = this;
        }

        private async void btnAddUrl_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddNewUrl.Text.IsValidUrl() == false)
            {
                MessageBox.Show("please enter a valid url");
                return;
            }

            var result = await txtAddNewUrl.Text.addUrl().ConfigureAwait(false);
            if (result == false)
            {
                MessageBox.Show("this url exists");
                return;
            }

            MessageBox.Show("url added");
        }

        private readonly Timer CrawlerTimer = new Timer(CrawlTimerTick, null, Timeout.Infinite, Timeout.Infinite);

        private static int threadCount = 20;
        private static List<Task> lstActiveCrawlingTasks = new List<Task>();
        private static object _CrawlTimerTick = new object();
        public static HashSet<string> hsActiveCrawling = new HashSet<string>();

        private static readonly ReaderWriterLockSlim crawlTimerLock = new ReaderWriterLockSlim();
        private static readonly SemaphoreSlim semph_CrawlTimerTick = new SemaphoreSlim(1, 1);
        private static bool blLocked_CrawlTimerTick = false;

        private static async void CrawlTimerTick(object? state)
        {
            //if (crawlTimerLock.IsWriteLockHeld)
            //    return;

            //crawlTimerLock.EnterWriteLock();
            //semph_CrawlTimerTick.Wait();
            if (blLocked_CrawlTimerTick)
                return;

            Debug.WriteLine("EnterWriteLock");
            try
            {
                lstActiveCrawlingTasks = lstActiveCrawlingTasks.Where(pr => pr.IsCompleted == false).ToList();
                var newCrawlCout = (threadCount * 2) - lstActiveCrawlingTasks.Count;
                List<Tuple<string, string>> vrUrls = new List<Tuple<string, string>>();
                if (newCrawlCout > 0)
                    vrUrls = await fetchRemainingUrls(newCrawlCout, 1).ConfigureAwait(true);
                await _thisWindow.lblUrlControl.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _thisWindow.lblUrlControl.Content = DateTime.Now.ToString("H:mm:ss") + $" current active crawling: {lstActiveCrawlingTasks.Count.ToString("N0")} number of url waiting to be crawled : {vrUrls.Count}";
                }));

                foreach (var vrUrl in vrUrls)
                {
                    lock (hsActiveCrawling)
                    {
                        if (hsActiveCrawling.Contains(vrUrl.Item1))
                            continue;

                        var startedTask = Task.Run(new Action(async () =>
                        {
                            await CrawlUrl(vrUrl.Item1, vrUrl.Item2).ConfigureAwait(false);
                        }));

                        lstActiveCrawlingTasks.Add(startedTask);
                    }
                }
            }
            finally
            {
                //if (crawlTimerLock.IsWriteLockHeld)
                //    crawlTimerLock.ExitWriteLock();

                //semph_CrawlTimerTick.Release();

                blLocked_CrawlTimerTick = false;
            }
        }

        private static async Task CrawlUrl(string srUrl, string Hash)
        {
            var vrUrls = await Scripts.extractLinks(srUrl, Hash).ConfigureAwait(false);

            if (vrUrls is not null)
            {
                foreach (var vrUrl in vrUrls)
                {
                    await HelperExtensions.addUrl(vrUrl).ConfigureAwait(false);
                }
            }
        }

        private void btnStartCrawling_Click(object sender, RoutedEventArgs e)
        {
            CrawlerTimer.Change(0, 1*1000);

        }

        private static async Task<List<Tuple<string, string>>> fetchRemainingUrls(int fetchCount, int rootUrlId)
        {
            using ExampleCrawlerContext _context = new ExampleCrawlerContext();
            var gg = await _context.Urls.ToListAsync().ConfigureAwait(false);
            return await _context.Urls.Where(pr => pr.Crawled == false && pr.RootDomainId == rootUrlId && pr.CrawlTryCount < 4).OrderBy(pr => pr.DiscoveryDate).Select(pr => Tuple.Create(pr.Url, pr.UrlHash)).ToListAsync().ConfigureAwait(false);
        }
    }
}
