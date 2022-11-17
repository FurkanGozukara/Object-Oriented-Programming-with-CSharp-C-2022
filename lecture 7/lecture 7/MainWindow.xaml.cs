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
        }

        private void testMethod(myDel methods)
        {
            methods();
        }

        private delegate void myDel();

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
    }
}
