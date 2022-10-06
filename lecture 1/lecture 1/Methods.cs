using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lecture_1
{
    public class Methods
    {
        public int irMyNumber;

        public static int func1()
        {
            return 1;
        }

        public int func2()
        {
            return 2;
        }

        protected internal int func3()
        {
            return 3;
        }

        protected int func4()
        {
            return 4;
        }

        internal int func5()
        {
            return 5;
        }

        private protected int func6()
        {
            return 6;
        }

        private int func7()
        {
            return 7;
        }

        public static string printMSG()
        {
            return "working";
        }

        public static void doSpecialThing(MainWindow _main)
        {
            _main.srMsg = "ok it works";
            _main.lblmsg.Content = "working";
        }

        public static void modifyValueType(int irNumber)
        {
            irNumber = irNumber * 3;
        }

        public static void modifyValueType2(ref int irNumber)
        {
            irNumber = irNumber * 3;
        }

        public static void modifyValueType3(MainWindow _main)
        {
            _main.irNumbergg = _main.irNumbergg * 3;
        }

    }

    public class DerivedMethods : Methods
    {
       

        DerivedMethods()
        {
            irMyNumber = 32;
            this.irMyNumber = 32;
            func1();
        }
    }

 
}
