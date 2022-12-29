using System;
using System.Collections;
using System.Collections.Generic;
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
        }

        private void btnDataRacing_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstNumbers = Enumerable.Range(1, 100).ToList();
            foreach (var vrItemm in lstNumbers)
            {
                Task.Factory.StartNew(async () =>
                {
                    await printToFile(vrItemm);
                });
            }
        }

        StreamWriter swWrite = new StreamWriter("test.txt");
        private async Task printToFile(int irNumber)
        {
            await swWrite.WriteLineAsync(irNumber.ToString());
        }
    }
}
