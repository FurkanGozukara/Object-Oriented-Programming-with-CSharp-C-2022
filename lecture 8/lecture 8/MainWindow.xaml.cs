using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string _usernameMsg = "Enter Username";
        private const string _userIdMsg = "Enter UserId";

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            txtStudentName.Text = _usernameMsg;
            txtStudentName.GotFocus += TxtStudentName_GotFocus;
            txtStudentName.LostFocus += TxtStudentName_LostFocus;

            txtUserId.Text = _userIdMsg;
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

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _numberOfListBoxElements;
        public string numberOfListBoxElements
        {
            get { return _numberOfListBoxElements; }
            set { _numberOfListBoxElements = value;
                NotifyPropertyChanged();
            }
        }

        private int _irElementCount;
        public int irElementCount
        {
            get { return _irElementCount; }
            set
            {
                _irElementCount = value;
                NotifyPropertyChanged();
            }
        }

        private void addStudent_Click(object sender, RoutedEventArgs e)
        {
            lstBoxItems.Items.Add(txtUserId.Text + " : " + txtStudentName.Text);
            numberOfListBoxElements = "Number of elements in the list box: "+ lstBoxItems.Items.Count.ToString("N0");
            irElementCount++;
        }
    }
}
