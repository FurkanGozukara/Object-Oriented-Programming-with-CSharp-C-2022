using System;
using System.Collections.Generic;
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

namespace lecture_1
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

        //public can be accessed everywhere
        //private can be accessed only within class
        //protected can be accessed from derived class
        //internal can be accessed only within same assembly

        private void btnReturnResult_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Methods.func1().ToString());
            //call other methods inside method class here // func 2 3 4 5
            Methods myMethods = new Methods();
            myMethods.func2();
            myMethods.func3();
            myMethods.func5();

            newTest test2 = new newTest();
            test2.func4();

            test2.irMyNumber = 100;
            MessageBox.Show(test2.func8().ToString());

            newTest test3 = new newTest();
            MessageBox.Show(test3.func8().ToString());
        }

        private class newTest : Methods
        {
            public int func4()
            {
               //not this but base to call paren'ts func4
                return base.func4();
            }

            public int func8()
            {
                return this.irMyNumber;
            }
        }
    }
}
