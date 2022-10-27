using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    }
}
