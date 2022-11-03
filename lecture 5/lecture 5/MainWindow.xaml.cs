using System;
using System.Collections.Generic;
using System.Linq;
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

namespace lecture_5
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

        static long myNumber;

        private void btnTryDataRace_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => { DoTasks(); });
        }

        private void DoTasks()
        {
            List<Task> lstTasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                var vrTask = Task.Factory.StartNew(() => { incrementVal_UnSafe(); });
                lstTasks.Add(vrTask);
            }

            Task.WaitAll(lstTasks.ToArray());

            MessageBox.Show("my number result is " + myNumber.ToString("N0"));

            myNumber = 0;

            lstTasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                var vrTask = Task.Factory.StartNew(() => { incrementVal_Safe(); });
                lstTasks.Add(vrTask);
            }

            Task.WaitAll(lstTasks.ToArray());

            MessageBox.Show("my number result is " + myNumber.ToString("N0"));

            //so fix code to display 10 billion
        }

        const int irMaxLoopCount = 10000000;

        //if you use await, it wouldnt run async in parallely 
        //using await inside for loop is not an acceptable answer
        //all - 10  incrementVal_UnSafe has to run simultaneusly at the same time 
        //in this case data race is happening therefore incremental values are overwritten
        private void incrementVal_UnSafe()
        {
            for (int i = 0; i < irMaxLoopCount; i++)
            {
                myNumber++;
            }
        }

        private void incrementVal_Safe()
        {
            for (int i = 0; i < irMaxLoopCount; i++)
            {
                Interlocked.Increment(ref myNumber);
            }
        }
    }
}
