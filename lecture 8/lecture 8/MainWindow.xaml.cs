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

namespace lecture_8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _usernameMsg = "Enter Username";

        public MainWindow()
        {
            InitializeComponent();
            txtStudentName.Text = _usernameMsg;
            txtStudentName.GotFocus += TxtStudentName_GotFocus;
            txtStudentName.LostFocus += TxtStudentName_LostFocus;
        }

        private void TxtStudentName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentName.Text.Trim()))
            {
                txtStudentName.Text = _usernameMsg;
            }
        }

        //we are defining placeholder equivalament of html css
        private void TxtStudentName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtStudentName.Text == _usernameMsg)
            {
                txtStudentName.Text = "";
            }
        }
    }
}
