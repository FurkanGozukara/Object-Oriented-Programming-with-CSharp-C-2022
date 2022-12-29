using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace lecture_12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            swWrite.AutoFlush= true;
            swWrite2.AutoFlush = true;
            swWrite3.AutoFlush = true;
            swWrite4.AutoFlush = true;
        }

        private void btnDataRacing_Click(object sender, RoutedEventArgs e)
        {
            dataRaceEx1();
            dataRaceEx2();
            dataRaceEx3();
        }

        void dataRaceEx1()
        {
            List<int> lstNumbers = Enumerable.Range(1, 100).ToList();
            foreach (var vrItemm in lstNumbers)
            {
                var _local = vrItemm;
                Task.Factory.StartNew(async () =>
                {
                    await printToFile(_local);
                });
            }
        }

        void dataRaceEx2()
        {
            List<int> lstNumbers = Enumerable.Range(1, 100).ToList();
            int irPassedData = 0;
            foreach (var vrItemm in lstNumbers)
            {
                var _local = vrItemm;
                irPassedData = vrItemm;
                Task.Factory.StartNew( () =>
                {
                    printToFile2(_local);
                    printToFile4(irPassedData);
                });
            }
        }

        void dataRaceEx3()
        {
            List<int> lstNumbers = Enumerable.Range(1, 100).ToList();
            List<object> lstNumbers2 = new List<object>();

            foreach (var item in lstNumbers)
            {
                lstNumbers2.Add(item);
            }

            List<Task> listTasks = new List<Task>();
             
            foreach (var vrItemm in lstNumbers2)
            {
               var vrTask= Task.Factory.StartNew(() =>
                {
                    printToFile3(vrItemm);
                });
                listTasks.Add(vrTask);
            }

            Task.WaitAll(listTasks.ToArray());
            File.WriteAllLines("test5.txt", listNumbersArrived.Select(pr => pr.ToString()).ToList());
        }

        StreamWriter swWrite = new StreamWriter("test.txt");
        private async Task printToFile(int _param)
        {
            Debug.WriteLine(_param.ToString());
            await swWrite.WriteLineAsync(_param.ToString());
        }

        StreamWriter swWrite2 = new StreamWriter("test2.txt");
        private  void printToFile2(int _param)
        {
            lock(swWrite2)
            {
                swWrite2.WriteLine(_param.ToString());
            } 
        }

        StreamWriter swWrite4 = new StreamWriter("test4.txt");
        private void printToFile4(int _param)
        {
            lock (swWrite4)
            {
                swWrite4.WriteLine(_param.ToString());
            }
        }

        StreamWriter swWrite3 = new StreamWriter("test3.txt");
        private static List<int> listNumbersArrived = new List<int>();  
        private void printToFile3(object _param)
        {
            listNumbersArrived.Add(Convert.ToInt32(_param));
            //lock (swWrite3)
            //{
            //    swWrite3.WriteLine(_param.ToString());
            //}
        }
    }
}
