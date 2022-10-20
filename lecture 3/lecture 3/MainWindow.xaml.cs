using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace lecture_3
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

        List<Tuple<int, string>> ListTuples = new List<Tuple<int, string>>();
        Dictionary<int, string> DictTuples = new Dictionary<int, string>();

        private void speedTest_Click(object sender, RoutedEventArgs e)
        {
            //generate 1 random number
            //add i and that random number to the both list and dictionary
            //i is key in dictionary and first element in tuples

            Random rand = new Random();

            for (int i = 0; i < 1000000; i++)
            {
                string srRandNumber = rand.Next().ToString("N0");
                ListTuples.Add(new Tuple<int, string>(i, srRandNumber));

                //you cant add 2 same key to the dictionaries it would throw an error
                if (DictTuples.ContainsKey(i) == false)
                    DictTuples.Add(i, srRandNumber);
                else
                    DictTuples[i] = srRandNumber;//this means that update the value that key i holds
            }

            ListTuples.Add(new Tuple<int, string>(321, "334,345,123"));
            ListTuples.Add(Tuple.Create(321, "334,345,123"));//same as new tuple
            //this would throw an error DictTuples.Add(321, "334,345,123");

            var vrPrevValue = DictTuples[321];//how we read value
            DictTuples[321] = "334,345,123";//how we assign value
            var vrNewValue = DictTuples[321];

            Debug.WriteLine($"vrPrevValue = {vrPrevValue}");
            Debug.WriteLine($"vrNewValue = {vrNewValue}");

            //random sort list and dictionary to compare speed
           
            ListTuples = ListTuples.OrderBy(anyThing => Guid.NewGuid()).ToList();

        }

        //complete this method
        private void writeListToTextFile(string FileName, List<Tuple<int, string>> ListTuples)
        {
            //if you dont provide a full path it will save file into the same folder as exe - where the exe is running 
            File.WriteAllLines(FileName, ListTuples.Select(perPairs => perPairs.Item1 + "\t" + perPairs.Item2));
            //" t is tab character"

            using (StreamWriter swWrite =new StreamWriter(FileName))
            {
                foreach (var vrPerTuple in ListTuples)
                {
                    swWrite.WriteLine(vrPerTuple.Item1 + "\t" + vrPerTuple.Item2);
                }

                for (int i = 0; i < ListTuples.Count; i++)
                {
                    swWrite.WriteLine(ListTuples[i].Item1 + "\t" + ListTuples[i].Item2);
                }
            }
        }
    }
}