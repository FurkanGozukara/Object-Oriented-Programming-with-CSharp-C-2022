using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            MessageBox.Show(sr + sr);
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
            object test2 = new List<Int16> { 231, 321, 12, 12 };

            List<object> myList = new List<object>();

            myList.Add(test);
            myList.Add(test2);

            MessageBox.Show(myList[1].ToString());
            //unboxing (List<Int16>) this is expensive operation
            MessageBox.Show(((List<Int16>)myList[1])[1].ToString());

            MessageBox.Show(HelperMethods.irNum.ToString());//this one wont cause init of static class
            MessageBox.Show(HelperMethods.irNum2.ToString());//however when this one is used the constructor of the static class will be called

            // MessageBox.Show(myNumbersList.Value[2].ToString());
        }

        Lazy<List<int>> myNumbersList = new Lazy<List<int>>(Enumerable.Range(1, 99999999)
           .Select(x => x).ToList());

        List<int> myNumbersList2 = new List<int>(Enumerable.Range(1, 99999999)
         .Select(x => x).ToList());

        private async void btnInsertElemts_Click(object sender, RoutedEventArgs e)
        {
          await  runForever();
        }

        private async Task runForever()
        {
            await Task.Factory.StartNew(async () => {await addElement(); });
            await Task.Delay(5555);
            await runForever();
        }

        private async Task addElement()
        {
         
     //this below one will wait until listbox is updated    
            lstBox1.Dispatcher.Invoke(() => { lstBox1.Items.Add(DateTime.Now.ToString()); });
      //this will modify listbox whenever possible and this line will be immediately executed      lstBox1.Dispatcher.BeginInvoke(() => { lstBox1.Items.Add(DateTime.Now.ToString()); });

            // lstBox1.Items.Add(DateTime.Now.ToString()); this will cause error
            // The calling thread cannot access this object because a different thread owns it.
        }
    }

    public static class HelperMethods
    {
        public static Dictionary<int, int> dicNumbers;

        public const int irNum = 312;//this is compile time assigned

        public static int irNum2 = 312;//this is runtime assigned
        public static readonly int irNum3;

        static HelperMethods()
        {
            // irNum = 321;//this gives error because it is compile time assigned
            irNum3 = 123;//this works because we are inside the constructor of the class

            dicNumbers = new Dictionary<int, int>();
            for (int i = 1; i < 8888888; i++)
            {
                dicNumbers.Add(i, i);
            }
        }
    }
}
