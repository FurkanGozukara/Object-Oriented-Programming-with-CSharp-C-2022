using System;
using System.Globalization;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("https://wwW.toros.edu.TR");
            Console.WriteLine("https://wwW.toros.edu.TR/");
            Console.WriteLine("https://wwW.toros.edu.TR/ test");
            Console.WriteLine("https://wwW.toros.edu.TR/%20test");
            Console.WriteLine("https://wwW.toros.edu.TR/%20test#top");
        }

        public static string NormalizeUrl(this string url)
        {
            Uri myUri = new Uri(url);
            return myUri.AbsoluteUri.ToString().ToLowerCaseEN();
        }

     
     
    }

    public static class ext
    {
        private readonly static CultureInfo enCulture = new CultureInfo("en-US");
        public static string ToLowerCaseEN(this string str)
        {
            return str.ToLower(enCulture);
        }
    }
}