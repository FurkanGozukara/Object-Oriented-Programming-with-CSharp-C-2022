using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            for (int i = 0; i < 10000000; i++)
            {
                Task.Factory.StartNew(() => { tastGG(); });
                System.Threading.Thread.Sleep(1);
                if (GCList.Count > 1000)
                {
                    GCList.Clear();
                    GC.Collect();
                }                   
            }
        }

        List<exampleGC> GCList = new List<exampleGC>();

        private void tastGG()
        {
            //make this application crash due to memory error
            exampleGC exampleGC = new exampleGC();
            exampleGC.MyProperty = new Random().Next();
            for (int i = 0; i < 10; i++)
            {
                exampleGC.srProp += exampleGC.srProp;
            }
            GCList.Add(exampleGC);
        }
    }
}
