using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
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

namespace lecture_7
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

        private void btnUnhanlederror_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToInt16("asd212");
            }
            catch (Exception)
            {

                
            }

            //Convert.ToInt16("asd212");

            //myAwesomeMethod();

            myDel _del = myAwesomeMethod;

            _del += myAwesomeMethod2;

            _del();

            myDel _del2 = myAwesomeMethod3;

            testMethod(_del2);

            _del2 = writeToFile;

            testMethod(_del2);

            //myDel2 _del3 = myAwesomeMethod3;//signature not match

            //myDel3 _del4 = myAwesomeMethod3;//return type not match

            myDel2 _del5 = testMethod2;
            _del5 += testMethod3;
            _del5("fds");
        }



        private void testMethod3(string sr)
        {
            MessageBox.Show(sr+ sr);
        }

        private void testMethod2(string sr)
        {
            MessageBox.Show(sr);
        }

        private void testMethod(myDel methods)
        {
            methods();
        }

        private delegate void myDel();

        private delegate void myDel2(string srMsg);

        private delegate int myDel3();

        private void writeToFile()
        {
            File.WriteAllText("gg.txt", "gg");
        }

        private void myAwesomeMethod()
        {
            MessageBox.Show("awesome method call");
        }

        private void myAwesomeMethod2()
        {
            MessageBox.Show("awesome method call 2");
        }

        private void myAwesomeMethod3()
        {
            MessageBox.Show("awesome method call 3");
        }

        class DataStore<T>
        {
            public T[] data = new T[10];
        }

        class DataStore
        {
            public int[] data = new int[10];
        }

        private void btnSimpleGenericExample_Click(object sender, RoutedEventArgs e)
        {
            DataStore myData = new DataStore();
            myData.data[3] = 32;

            DataStore<long> myDataF = new DataStore<long>();
            myDataF.data[3] = 31243894789112;

            DataStore<string> myDataStr = new DataStore<string>();
            myDataStr.data[2] = "gg wp";
            
            //this is boxing
            object test = myDataStr;
            object test2 = new List<Int16> { 231, 321, 12, 12 } ;

            List<object> myList = new List<object>();

            myList.Add(test);
            myList.Add(test2);

            MessageBox.Show(myList[1].ToString());
            //unboxing
            MessageBox.Show(((List<Int16>)myList[1])[1].ToString());
        }
    }

 
}
