using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lecture_1
{
    public class Methods
    {
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
    }

    public class DerivedMethods : Methods
    {
        DerivedMethods()
        {
             
        }
    }

}
