using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test2;

namespace lecture_2
{
    internal class Vehicles
    {

        public string VehicleType;//some applications behaviour changes according to being field or property. so even if you do not change get and set and use property as field, the application / framework behaviour could change

        private string _VehicleName;

        public string VehicleName
        {
            get
            {
                return "Vehicle Name: " + _VehicleName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    _VehicleName = "Not Set";
                else
                    _VehicleName = value;
            }
        }

        private void test()
        {
            lecture_2.Vehicles.Test gg = new Test();
            test2.Test gg2 = new test2.Test();
        }

        internal class Test
        {
            public int MyProperty2 { get; set; }
        }
    }


}

namespace test2
{
    internal class Test
    {
        public int MyProperty { get; set; }
    }
}