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

namespace lecture_9
{
    public class customStudent : ListBoxItem, lecture_9_dll.customStudent
    {
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

        public customStudent()
        {
            this.Foreground = Brushes.Red;
            this.Background = Brushes.Green;
            this.FontStyle = FontStyles.Italic;
            this.FontSize = 22.1;
            

        }

        public override string ToString()
        {
            return DisplayObject;
        }

    }
}
