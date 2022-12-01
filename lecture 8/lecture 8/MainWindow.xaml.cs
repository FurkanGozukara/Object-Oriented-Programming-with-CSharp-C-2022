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
        private const string _userIdMsg = "Enter UserId";

        public MainWindow()
        {
            InitializeComponent();
            txtStudentName.Text = _usernameMsg;
            txtStudentName.GotFocus += TxtStudentName_GotFocus;
            txtStudentName.LostFocus += TxtStudentName_LostFocus;

            txtUserId.Text = _usernameMsg;
            txtUserId.GotFocus += TxtStudentName_GotFocus;
            txtUserId.LostFocus += TxtStudentName_LostFocus;

        }

        private string returnMsg(object sender)
        {
            string srmsg = _usernameMsg;

            switch (((TextBox)sender).Name)//this is unboxing
            {
                case "txtUserId":
                    srmsg = _userIdMsg;
                    break;
            }
            return srmsg;
        }

        private void TxtStudentName_LostFocus(object sender, RoutedEventArgs e)
        {
            var srmsg = returnMsg(sender);

            if (string.IsNullOrEmpty(((TextBox)sender).Text.Trim()))
            {
                ((TextBox)sender).Text = srmsg;
            }
        }

        //we are defining placeholder equivalament of html css
        private void TxtStudentName_GotFocus(object sender, RoutedEventArgs e)
        {
            var srmsg = returnMsg(sender);

            if (((TextBox)sender).Text == srmsg)
            {
                ((TextBox)sender).Text = "";
            }
        }
    }
}
