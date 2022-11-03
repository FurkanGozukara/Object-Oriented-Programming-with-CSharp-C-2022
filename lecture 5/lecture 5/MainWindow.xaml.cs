using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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

namespace lecture_5
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

        static long myNumber;

        private void btnTryDataRace_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => { DoTasks_Refactored(); });
        }

        public delegate void DelDoIncrements();//this is used to assign methods into variables

        private void DoTasks_Refactored()
        {
            List<Task> lstTasks = new List<Task>();

            for (int upperLoop = 0; upperLoop < 2; upperLoop++)
            {
                DelDoIncrements myMethodCall = incrementVal_UnSafe;

                switch (upperLoop)
                {
                    case 1:
                        myMethodCall = incrementVal_Safe;
                        break;
                }

                for (int i = 0; i < 10; i++)
                {
                    var vrTask = Task.Factory.StartNew(() => { myMethodCall(); });
                    lstTasks.Add(vrTask);
                }

                Task.WaitAll(lstTasks.ToArray());

                MessageBox.Show("my number result is " + myNumber.ToString("N0"));

                myNumber = 0;
                lstTasks = new List<Task>();
            }

            //so fix code to display 10 billion
        }

        private void DoTasks()
        {
            List<Task> lstTasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                var vrTask = Task.Factory.StartNew(() => { incrementVal_UnSafe(); });
                lstTasks.Add(vrTask);
            }

            Task.WaitAll(lstTasks.ToArray());

            MessageBox.Show("my number result is " + myNumber.ToString("N0"));

            myNumber = 0;

            lstTasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                var vrTask = Task.Factory.StartNew(() => { incrementVal_Safe(); });
                lstTasks.Add(vrTask);
            }

            Task.WaitAll(lstTasks.ToArray());

            MessageBox.Show("my number result is " + myNumber.ToString("N0"));

            //so fix code to display 10 billion
        }

        const int irMaxLoopCount = 10000000;

        //if you use await, it wouldnt run async in parallely 
        //using await inside for loop is not an acceptable answer
        //all - 10  incrementVal_UnSafe has to run simultaneusly at the same time 
        //in this case data race is happening therefore incremental values are overwritten
        private void incrementVal_UnSafe()
        {
            for (int i = 0; i < irMaxLoopCount; i++)
            {
                myNumber++;
            }
        }

        private void incrementVal_Safe()
        {
            for (int i = 0; i < irMaxLoopCount; i++)
            {
                Interlocked.Increment(ref myNumber);
            }
        }

        public class csStudent
        {
            public int irStudentId { get; set; }
            public string srStudentName;
            public double dblAvgScore { get; set; }
            public int irCurrentSemester;
            public string srPhoneNumber;
            public string srEmail { get; set; }

            public csStudent doCloneCopy()
            {
                return new csStudent { dblAvgScore = this.dblAvgScore, irCurrentSemester = this.irCurrentSemester, irStudentId = this.irStudentId, srEmail = this.srEmail, srPhoneNumber = this.srPhoneNumber, srStudentName = this.srStudentName };
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            csStudent FirstStudent = new csStudent
            {
                irStudentId = 1,
                dblAvgScore = 3.53,
                irCurrentSemester = 3,
                srEmail = "firstStud@gmail.com",
                srPhoneNumber = "+90 555 55 55",
                srStudentName = "First Student"
            };

            //generate a duplicate of this student
            //however, when i change value of firststudent, it musn't affect the clone student object
            //this is deep cloning

            csStudent student2 = FirstStudent;//this will compose only a shallow clone

            student2.srStudentName = "student 2";

            MessageBox.Show($"first: {FirstStudent.srStudentName} , second: {student2.srStudentName}"  );

            csStudent student3 = FirstStudent.doCloneCopy();

            FirstStudent.srPhoneNumber = "055555555";
            student3.srPhoneNumber = "06666666";

            MessageBox.Show($"1: {FirstStudent.srPhoneNumber} , 3: {student3.srPhoneNumber}");

            csStudent student4 = Clone<csStudent>(FirstStudent);

            student4.srStudentName = "student 4";

            MessageBox.Show($"first: {FirstStudent.srStudentName} , second: {student4.srStudentName}");

            //how to save an instance of object into a text file and then load back later

            //you can save and load objects with serializing object into a json string
            //you can use Newtonsoft.Json.Bson from nuget

        }

        public static T Clone<T>(T source)//this is generic type method therefore you can use this method on any of your custom class etc
        {

            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
