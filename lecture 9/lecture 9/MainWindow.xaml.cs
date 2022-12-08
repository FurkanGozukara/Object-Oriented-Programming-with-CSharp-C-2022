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

namespace lecture_9
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
            lstBoxStudents.Items.Add(new customStudent { age = 25, name = "test", id = 1 }.ToString());
        }
    }

    public static class HelperMethods
    {
        public static void testConfliction()
        {

        }
    }


}
