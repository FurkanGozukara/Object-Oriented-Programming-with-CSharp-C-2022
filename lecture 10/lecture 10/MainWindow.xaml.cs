﻿using lecture_10.Models;
using Microsoft.EntityFrameworkCore;
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
            Students _Student = new Students();
            _Student.FullName = "Furkan Gözükara";
            _Student.Gpa = 3.77M;


            await myRepo.Create(_Student);
        }
    }
}
