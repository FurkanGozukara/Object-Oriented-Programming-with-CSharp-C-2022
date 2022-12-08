using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using lecture_9_dll;
using System.IO;

namespace lecture_9
{


    public class customStudent : ListBoxItem, lecture_9_dll.customStudent
    {
        public static string returnCustomName()
        {
            return ListOfNames[Random.Shared.Next(0, ListOfNames.Count)];
        }

        public static readonly List<string> ListOfNames = File.ReadAllLines("names.txt").ToList();

        public string name { get; set; }
        public int id { get; set; }
        public int age { get; set; }

        public string DisplayObject
        {
            get
            {
                return $"Id: {id}, Name: {name}, Age: {age}";
            }
        }

         

        public customStudent(string _name, int _id, int _age)
        {
            this.Foreground = Brushes.Red;
            this.Background = Brushes.Green;
            this.FontStyle = FontStyles.Italic;
            this.FontSize = 22.1;
            this.Name = _name;  
            this.id = _id;
            this.age = _age;
            this.Content = DisplayObject;

        }

        public override string ToString()
        {
            return DisplayObject;
        }

   
        }
         
    }

