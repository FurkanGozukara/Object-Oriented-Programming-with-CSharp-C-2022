using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace lecture_2
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

        private void btnPrintProperty_Click(object sender, RoutedEventArgs e)
        {
            Vehicles myVehicle = new Vehicles();
            try
            {
                myVehicle.VehicleName = txtProperty.Text;
            }
            catch   (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
         
            MessageBox.Show(myVehicle.VehicleName);
        }

        private void btnPrintNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int irNumber = Convert.ToInt32(txtNumber.Text);
                int irResult = Convert.ToInt32( Math.Pow(irNumber, 2));
              
                if (irResult > 1000)
                    irResult = 1000;

                irResult = (irResult > 1000 ? 1000 : irResult);

                MessageBox.Show(irResult.ToString("N0"));//n0 means no numbers after 0.x if it be n1 it will show as 0.2 // so this is for precision

                MessageBox.Show(3213.673.ToString("N1",new CultureInfo("en-US")));//displays 3,213.7

                MessageBox.Show(3213.673.ToString("N1", new CultureInfo("tr-TR")));//displays 3.213,7
            }
            catch (OverflowException E)
            {
                MessageBox.Show("The number you have entered overflows int\n\n" + E.StackTrace);
            }
            catch (FormatException E)
            {
                MessageBox.Show("You have entered invalid number\n\n" + E.StackTrace);
            }
            finally//this is called everytime
            {
                MessageBox.Show("finally called");
            }


        }
    }
}
