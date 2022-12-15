using lecture_10.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static lecture_10.exampleAbstract;

namespace lecture_10
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

        private void btnAbstractExample_Click(object sender, RoutedEventArgs e)
        {
            //exampleAbstract.Shape ex_shape = new();  this wont compile because you cant have instance of abstract class

            Circle c = new Circle(5);
            MessageBox.Show(c.DisplayArea());

            Square s = new Square(6);
            MessageBox.Show(s.DisplayArea());


        }

        private async void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            using OOP2022Context _context = new OOP2022Context();
            Repository myRepo = new Repository(_context);
            Students _Student = new Students
            {
                Email = txtEmail.Text,
                FullName = txtStudentName.Text,
                Gpa = txtGPA.Text.toDecimal()
            };

            await myRepo.Create(_Student).ConfigureAwait(false);//this wont block main thread
        }

        private async void btnGetStudent_Click(object sender, RoutedEventArgs e)
        {
            var vrStudentd = await ReturnStudent(txtStudentId.Text);

            txtEmail.Text = vrStudentd?.Email;
            txtGPA.Text = vrStudentd?.Gpa.ToString();
            txtStudentName.Text = vrStudentd?.FullName;
        }

        private  async static Task<Students> ReturnStudent(string StudentId)
        {
            using OOP2022Context _context = new OOP2022Context();
            Repository myRepo = new Repository(_context);
            return await myRepo.GetById<Students>(StudentId.toInt());
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            using OOP2022Context _context = new OOP2022Context();
            Repository myRepo = new Repository(_context);
            var vrStudentd = await myRepo.GetById<Students>(txtStudentId.Text.toInt());


            if(vrStudentd is not null)
            {
                vrStudentd.Email = txtEmail.Text;
                vrStudentd.Gpa = txtGPA.Text.toDecimal();
                vrStudentd.FullName = txtStudentName.Text;
            }
        

            var vrResult = await myRepo.Update(vrStudentd);
            MessageBox.Show("affected rows count: " + vrResult);
        }

        private async void btnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            using OOP2022Context _context = new OOP2022Context();
            Repository myRepo = new Repository(_context);
            var vrStudentd = await myRepo.GetById<Students>(txtStudentId.Text.toInt());
            var vrResult = await myRepo.Delete(vrStudentd);
            MessageBox.Show("affected rows count: " + vrResult);
        }

        private async void btnGetAll_Click(object sender, RoutedEventArgs e)
        {
            lstbox.Items.Clear();
            using OOP2022Context _context = new OOP2022Context();
            Repository myRepo = new Repository(_context);
// var vrStudentds = await myRepo.GetAll<Students>().ToListAsync();
            var vrStudentds = await myRepo.GetAll<Students>().Where(s => s.FullName.Contains("ahmet")).ToListAsync();
            if (vrStudentds is not null)
            {
                foreach (var vrStudent in vrStudentds)
                {
                    lstbox.Items.Add($"{vrStudent.Id} \t {vrStudent.FullName}");
                }
            }
        }
    }
}
