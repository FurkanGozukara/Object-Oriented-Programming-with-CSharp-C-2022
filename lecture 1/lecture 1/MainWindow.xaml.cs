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
        public string srMsg = "test";

        public MainWindow()
        {
            InitializeComponent();

        }

        //public can be accessed everywhere
        //private can be accessed only within class
        //protected can be accessed from derived class
        //internal can be accessed only within same assembly

        public int irNumbergg = 10;

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

            Methods.doSpecialThing(this);


            MessageBox.Show("number gg = " + irNumbergg);
            Methods.modifyValueType(irNumbergg);
            MessageBox.Show("number gg = " + irNumbergg);

            Methods.modifyValueType2(ref irNumbergg);
            MessageBox.Show("number gg = " + irNumbergg);

            Methods.modifyValueType3(this);
            MessageBox.Show("number gg = " + irNumbergg);


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

        public class csStudent
        {
            public int irAge;
            public string srName;
        }

        public struct stStudent
        {
            public int irAge;
            public string srName;
        }

        private void btnstructvsreference_Click(object sender, RoutedEventArgs e)
        {
            csStudent student1 = new csStudent();
            student1.irAge = 10;
            student1.srName = "Furkan";

            csStudent student2 = student1; // this just generates a new address that points to the same values of student 1 in memory

            MessageBox.Show($"student 1 is {student1.srName} : {student1.irAge} ");

            MessageBox.Show($"student 2 is {student2.srName} : {student2.irAge} ");

            student2.irAge = 32;
            student2.srName = "Ali";

            MessageBox.Show($"student 1 is {student1.srName} : {student1.irAge} ");

            MessageBox.Show($"student 2 is {student2.srName} : {student2.irAge} ");

            stStudent _student1 = new stStudent { irAge = 20, srName = "Zeynep" };

            stStudent _student2 = _student1;//this copies values of studet 1 into a new address and value in memory - we have 2 values and 2 address 

            MessageBox.Show($"_student1 1 is {_student1.srName} : {_student1.irAge} ");

            MessageBox.Show($"_student2 2 is {_student2.srName} : {_student2.irAge} ");

            _student2.irAge = 25;
            _student2.srName = "Meryem";

            MessageBox.Show($"_student1 1 is {_student1.srName} : {_student1.irAge} ");

            MessageBox.Show($"_student2 2 is {_student2.srName} : {_student2.irAge} ");

            student2 = new csStudent();
            student2.srName = "New Student";
            student2.irAge = 36;

            MessageBox.Show($"student 1 is {student1.srName} : {student1.irAge} ");

            MessageBox.Show($"student 2 is {student2.srName} : {student2.irAge} ");

           // student1 = student2; <this 2 makes different> student2 = student1;
        }

        //show ref type to class type object what happens
    }
}
