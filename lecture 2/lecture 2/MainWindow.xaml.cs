using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

namespace lecture_2
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

        private void btnPrintProperty_Click(object sender, RoutedEventArgs e)
        {
            Vehicles myVehicle = new Vehicles();
            try
            {
                myVehicle.VehicleName = txtProperty.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            MessageBox.Show(myVehicle.VehicleName);
        }

        private void btnPrintNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int irNumber = Convert.ToInt32(txtNumber.Text);
                int irResult = Convert.ToInt32(Math.Pow(irNumber, 2));

                if (irResult > 1000)
                    irResult = 1000;

                irResult = (irResult > 1000 ? 1000 : irResult);

                MessageBox.Show(irResult.ToString("N0"));//n0 means no numbers after 0.x if it be n1 it will show as 0.2 // so this is for precision

                MessageBox.Show(3213.673.ToString("N1", new CultureInfo("en-US")));//displays 3,213.7

                MessageBox.Show(3213.673F.ToString("N1", new CultureInfo("tr-TR")));//displays 3.213,7
            }
            catch (OverflowException E)
            {
                MessageBox.Show("The number you have entered overflows int\n\n" + E.StackTrace);
            }
            catch (FormatException E)
            {
                MessageBox.Show("You have entered invalid number\n\n" + E.StackTrace);
            }
            finally//this is called everytime
            {
                MessageBox.Show("finally called");
            }


        }

        private void btnGenerateRandomNumbers_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw1 = new Stopwatch();

            sw1.Start();
            var t1 = Task.Factory.StartNew(() => { WriteNumbersToText_v1(); });

            t1.ContinueWith((p1) =>
            {
                sw1.Stop(); 
                MessageBox.Show("method 1 took " + sw1.ElapsedMilliseconds.ToString("N0") + " ms");
            });

            Stopwatch sw2 = new Stopwatch();

            sw2.Start();
            var t2 = Task.Factory.StartNew(() => { WriteNumbersToText_v2(); });

            t2.ContinueWith((p1) =>
            {
                sw2.Stop();
                MessageBox.Show("method 2 took " + sw2.ElapsedMilliseconds.ToString("N0") + " ms");
            });

          

        
         
        }

        static int irNumberstoBeWritten = 1000000;

        private static void WriteNumbersToText_v1()
        {
            //do not uses any ram memory but slower
            Random newRand = new Random();
            using (StreamWriter swWrite = new StreamWriter("random_numbers_v1.txt"))
            {
                //https://learn.microsoft.com/en-us/dotnet/api/system.io.streamwriter.autoflush?view=net-6.0
                swWrite.AutoFlush = true;//auto flush means it will save string in memory to file as batches continously 
                for (int i = 0; i < irNumberstoBeWritten; i++)
                {
                    swWrite.WriteLine(newRand.Next());
                }
            }//when it goes out of this scope, everything inside using is properly disposed off
        }

        private static void WriteNumbersToText_v2()
        {   // uses big amount of ram but faster
            Random newRand = new Random();

            List<string> lstNumbers = new List<string>();

            for (int i = 0; i < irNumberstoBeWritten; i++)
            {
                lstNumbers.Add(newRand.Next().ToString());
            }

            File.WriteAllLines("random_numbers_v2.txt", lstNumbers);

            // File.WriteAllLines("random_numbers_v2.txt", lstNumbers.Select(pr => pr.ToString()));
        }
    }
}
