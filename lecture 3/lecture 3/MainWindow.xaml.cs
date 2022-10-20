using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        private void btnnInitTestdata_Click(object sender, RoutedEventArgs e)
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

            //lambda operators : https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
            //inline function
            Func<(int, int, int), (int, int, int)> doubleThem = ns => (5 * ns.Item1, 2 * ns.Item2, 2 * ns.Item3);

            var numbers = (2, 3, 4);
            var doubledNumbers = doubleThem(numbers);
            Console.WriteLine($"The set {numbers} doubled: {doubledNumbers}");
            // Output:
            // The set (2, 3, 4) doubled: (4, 6, 8)



            //random sort list and dictionary to compare speed

            string srPath = "default_sort_list.txt";
            int irNewNumber = 55;
            int irSecondNumber = 100;

            //   writeListToTextFile_3(srPath, ListTuples, irNewNumber, ref irSecondNumber);

            writeListToTextFile(srPath, ListTuples, irNewNumber, ref irSecondNumber);
            //string doesnt behave like a class type because it is immutable

            //       writeListToTextFile("default_sort_list.txt", ListTuples, irNewNumber, ref irSecondNumber);

            //  writeListToTextFile_v2("default_sort_list.txt", ref ListTuples, irNewNumber, ref irSecondNumber);

            ListTuples = ListTuples.OrderBy(anyThing => Guid.NewGuid()).ToList();

            writeListToTextFile("random_sorted_list.txt", ListTuples, irNewNumber, ref irSecondNumber);
            //string doesnt behave like a class type because it is immutable

            writeDictioaryToFile("default_sorted_dic.txt", DictTuples);

            DictTuples = DictTuples.OrderBy(anyThing => Guid.NewGuid()).ToDictionary(anyThing => anyThing.Key, anyThing => anyThing.Value);

            writeDictioaryToFile("random_sorted_dic.txt", DictTuples);

            int irTestval = 100;
            outExample(299, out irTestval);
        }

        private void outExample(int irNumber1, out int irNumber2)
        {
            irNumber2 = irNumber1 * 100;
        }

        Tuple<int, int, int> doubleThem(Tuple<int, int, int> vrTuple)
        {
            return Tuple.Create(vrTuple.Item1 * 5, vrTuple.Item2 * 2, vrTuple.Item3 * 3);
        }

        //complete this method
        private void writeListToTextFile(string FileName, List<Tuple<int, string>> ListTuples, int irNewNumber, ref int irSecondNumber)
        {
            //if you dont provide a full path it will save file into the same folder as exe - where the exe is running 
            File.WriteAllLines(FileName, ListTuples.Select(perPairs => perPairs.Item1 + "\t" + perPairs.Item2));
            //" t is tab character"

            ListTuples[0] = new Tuple<int, string>(0, irNewNumber.ToString("N0"));//would this affect the list

            FileName = "new path";
            irNewNumber = 1000;
            irSecondNumber = 500;
            //this is easier but slower method
            //using (StreamWriter swWrite =new StreamWriter(FileName))
            //{
            //    foreach (var vrPerTuple in ListTuples)
            //    {
            //        swWrite.WriteLine(vrPerTuple.Item1 + "\t" + vrPerTuple.Item2);
            //    }

            //    for (int i = 0; i < ListTuples.Count; i++)
            //    {
            //        swWrite.WriteLine(ListTuples[i].Item1 + "\t" + ListTuples[i].Item2);
            //    }
            //}

            //this does not copy new reference into the original reference
            ListTuples = new List<Tuple<int, string>>();
        }

        private void writeDictioaryToFile(string FileName, Dictionary<int, string> DictTuples)
        {
            File.WriteAllLines(FileName, DictTuples.Select(pr => pr.Key + "\t" + pr.Value));
        }

        private void writeListToTextFile_v2(string FileName, ref List<Tuple<int, string>> ListTuples, int irNewNumber, ref int irSecondNumber)
        {
            File.WriteAllLines(FileName, ListTuples.Select(perPairs => perPairs.Item1 + "\t" + perPairs.Item2));

            //it overwrites the original reference
            ListTuples = new List<Tuple<int, string>>();
        }

        private void writeListToTextFile_3(string FileName, List<Tuple<int, string>> ListTuples, int irNewNumber, ref int irSecondNumber)
        {
            //how can i use ListTuples in a way that the changes i made here will not affect the original ListTuples

            ListTuples = ListTuples.ToList();//this will generate a new list with a new reference therefore it will not affect original list

            ListTuples[0] = new Tuple<int, string>(0, "999");//would 

            ListTuples.Sort();
        }

        private void speedTest_Click(object sender, RoutedEventArgs e)
        {
            Random randTest = new Random();

            List<int> listTestKeys = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                listTestKeys.Add(randTest.Next(0, 100000));
            }

            Stopwatch swTimer = new Stopwatch();
            swTimer.Start();
            List<string> lstListFoundItems = new List<string>();
            foreach (var vrKey in listTestKeys)
            {
                var vrFoundTuple = ListTuples.Where(pr => pr.Item1 == vrKey).FirstOrDefault();

                foreach (var vrEachTuple in ListTuples)
                {
                    if(vrEachTuple.Item1==vrKey)
                    {
                        vrFoundTuple = vrEachTuple;
                        break;
                    }
                }

                var vrFoundTuples = ListTuples.Where(pr => pr.Item1 == vrKey);

                List<Tuple<int, string>> lstFoundTuples = new List<Tuple<int, string>>();

                foreach (var vrEachTuple in ListTuples)
                {
                    if (vrEachTuple.Item1 == vrKey)
                    {
                        lstFoundTuples.Add(vrEachTuple);
                    }
                }


                if (vrFoundTuple != null)
                    lstListFoundItems.Add(vrFoundTuple.Item1 + "\n" + vrFoundTuple.Item2);
            }
            swTimer.Stop();
            MessageBox.Show("list finding took: " + swTimer.ElapsedMilliseconds.ToString("N0")+" ms");
            File.WriteAllLines("list found items.txt", lstListFoundItems);

            swTimer.Reset();
            swTimer.Start();
            List<string> lstDictFoundItems = new List<string>();
            foreach (var vrKey in listTestKeys)
            {
                var vrFoundValue = DictTuples.ContainsKey(vrKey) ? DictTuples[vrKey] : null;
                lstDictFoundItems.Add(vrKey + "\n" + vrFoundValue);
            }
            swTimer.Stop();
            MessageBox.Show("dictionary finding took: " + swTimer.ElapsedMilliseconds.ToString("N0") + " ms");
            File.WriteAllLines("dict found items.txt", lstDictFoundItems);
        }
    }
}