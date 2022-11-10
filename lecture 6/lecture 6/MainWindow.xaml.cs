using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
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

namespace lecture_6
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

        private void btnImmutableType_Click(object sender, RoutedEventArgs e)
        {
            string first = "test";
            first = first + " test";//this will not modify the value but it will generate a new instance of value

            //first[2] = 'g';//since this is immutable you cant change. it is readonly

            var List = first.ToCharArray();
            List[1] = 'g';
            first = new string(List);

            first = "go school";

            //this is a very costly operation // stringbuilder is the best way
            first = string.Join("", first.Take(3)) + "S" + string.Join("", first.Skip(4));

            StringBuilder srTest = new StringBuilder("test");
            srTest.Append(" test");
            srTest[1] = 'g';
        }

        private void btnFieldProperty_Click(object sender, RoutedEventArgs e)
        {
            student myStudent = new();
            myStudent.StudentId = 10000;
            myStudent.StudentId2 = 10000;
            myStudent.StudentId3 = 10000;

            MessageBox.Show($"id = {myStudent.StudentId}  id 2 = {myStudent.StudentId2} id 3 = {myStudent.StudentId3}");

            myStudent.StudentId = -10000;
            myStudent.StudentId2 = -10000;
            myStudent.StudentId3 = -10000;

            MessageBox.Show($"id = {myStudent.StudentId}  id 2 = {myStudent.StudentId2} id 3 = {myStudent.StudentId3}");

            MessageBox.Show($"student name = {myStudent.StudentName} student2 name {myStudent.StudentName2}");

            myStudent.StudentName2 = "test value";

            MessageBox.Show($"student name = {myStudent.StudentName} student2 name {myStudent.StudentName2}");

            MessageBox.Show(modifyVal(null));
            MessageBox.Show(modifyVal("it has a value"));

            //?. this ensures that if it is not null execute second part

            var vrList1 = myStudent.StudentName2?.ToCharArray();

            myStudent.StudentName2 = null;

            var vrList2 = myStudent.StudentName2?.ToCharArray();


            myStudent.StudentName = "test";

           vrList1 = myStudent.StudentName?.ToCharArray();

            myStudent.StudentName = null;

             vrList2 = myStudent.StudentName?.ToCharArray();

            MessageBox.Show(runTimeValues.lstRandNumbers[1].ToString());
        }

        private string modifyVal(string srinput)
        {
            srinput ??= "my val";
            return srinput;
        }

        private void btnMethodExtension_Click(object sender, RoutedEventArgs e)
        {
          var vals=  runTimeValues.lstNumbers3.doSomething();

            string srVal = 12312.ToString();
        }

        private void btnMethodoverride_Click(object sender, RoutedEventArgs e)
        {
            Childclass child = new Childclass();
          MessageBox.Show(  child.show());
            myCustomClass child2 = new Childclass();
            MessageBox.Show(child2.show());

            MessageBox.Show("show 2 child : "+ child.show2());
            MessageBox.Show("show 2 parent generated as child : " + child2.show2());
        }
    }

    public class Childclass : myCustomClass
    {
        public string show()
        {
            return "shows child class";
        }

        new public static string  CustomFormatMyInt(int irVal)
        {
            return irVal.ToString("N0");
        }
        public override string show2()//in order to override you need to mark as virtual
        {
            return "shows child class 2";
        }
    }

    public class myCustomClass
    {
        public virtual string show()
        {
            return "shows main class";
        }

        public virtual string show2()//in order to override you need to mark as virtual
        {
            return "shows main class 2";
        }

        public static string CustomFormatMyInt(int irVal)
        {
            return irVal.ToString("N2");
        }
    }

    public static class runTimeValues
    {


        public static string ToString(this int irNumber)
        {
            return irNumber.ToString("N0");
        }


        //when you put this keyword in front of input in a method it becomes a method extension. however method extensions has to be static and inside a static class
        public static string doSomething(this List<int> lstNumbers)
        {
            return string.Join(" , ", lstNumbers.Skip(3));
        }

        public static List<int> lstRandNumbers;

        public static Lazy<List<int>> lstNumbers2=new Lazy<List<int>>(initVals());
            
        public static List<int> lstNumbers3;

        static Action generateNumbers()
        {
            return () =>
            {
                initVals();
            };
        }

        public static  List<int> initVals()
        {
            List<int> listVals = new();
       
            for (int i = 0; i < 900000; i++)
            {
                listVals.Add(i);
            }
            return listVals;
        }

        static runTimeValues()
        {
            lstRandNumbers = new List<int>();
            for (int i = 0; i < 900000; i++)
            {
                lstRandNumbers.Add(i);
            }

            lstNumbers3 = initVals();
        }
    }

    public class student
    {


        public int StudentId { get; set; }

        public string StudentName;

        private int _StudentId2;
        public int StudentId2
        {
            get { return _StudentId2; }
            set { _StudentId2 = value; }
        }

        private int _StudentId3;
        public int StudentId3
        {
            get
            {
                if (_StudentId3 > 1000) return 1000;
                return _StudentId3;
            }
            set { _StudentId3 =  value < 0 ? 0 : value; }
        }

        private string _StudentName2;

        public string StudentName2 { 
            
            get

            {
                //if the value is not null return value else return the else value
                return _StudentName2 ?? "this value is null";
            }

            set
            {
                _StudentName2 = value;
            }
        }
    }
}
