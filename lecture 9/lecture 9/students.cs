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
using System.Reflection;

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

        private static Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }


        public customStudent(string _name, int _id, int _age)
        {
            this.Foreground = PickBrush();
            this.Background = PickBrush();
            this.FontStyle = FontStyles.Italic;
            this.FontSize = 22.1;
            this.name = _name;  
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

