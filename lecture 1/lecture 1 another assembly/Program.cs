using System;
using lecture_1;

internal class Program
{
    //non derived class different assembly for lecture 1
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        lecture_1.Methods myMethod = new Methods();
        Methods.func1();
    }

    public class diffAssembly : Methods
    {
        public diffAssembly()
        {
           
        }
    }
}
