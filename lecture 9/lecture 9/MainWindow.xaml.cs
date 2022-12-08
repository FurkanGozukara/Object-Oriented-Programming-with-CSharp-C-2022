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
using lecture_9_part2;
using System.Drawing;
using System.Collections.ObjectModel;

namespace lecture_9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         ObservableCollection<customStudent> ListBoxElements  = new ObservableCollection<customStudent>();


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
           // lstBoxStudents.ItemsSource = ListBoxElements;
            
            //lstBoxStudents.DisplayMemberPath = "DisplayObject";
        }

        private void btnUseAnotherNameSpace_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(lecture_9_part2.HelperMethods.returnMsg());
            testConfliction();
        }

        public static void testConfliction()
        {

        }

        //I want these student objects to be displayed as "Id: 1, Name: Test, Age: 25";
        private void btnAddSomeCustomStudents_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                lstBoxStudents.Items.Add(returnRandomStudent());
            }
        }

        private static int _irId = 1;

        private static customStudent returnRandomStudent()
        {
            customStudent _customStudent = new customStudent(customStudent.returnCustomName(),
                _irId++, Random.Shared.Next(20, 60));

            return _customStudent;
        }
    }



    public static class HelperMethods
    {
        public static void testConfliction()
        {

        }
    }


}
