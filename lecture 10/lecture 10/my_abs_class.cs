using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lecture_10
{
    public class exampleAbstract
    {
        //An abstract class is a class in C# that cannot be instantiated on its own, but can be inherited by other classes. Abstract classes are typically used to provide a common base class for a group of related classes. Abstract classes can contain both abstract and non-abstract methods (abstract methods do not have an implementation).
        public abstract class MyShape
        {
            public int SpecialNumber=2;
            public int RegulerProperty { get; set; }

            public virtual int SpecialProp { get; set; }//this do not force you to implement but you can override

            public virtual int VirtualProp { get; set; }//this you have to override and implement it

            //public abstract double Area2() this is invalid because abstract can not have body
            //{

            //}

            public virtual double Area3()//virtual can have body and can be overriden
            {
                return 0;
            }

            public abstract double Area();

            public string DisplayArea()
            {
               return string.Format($"The area of the {this.GetType()} is: {{0}}", Area());
            }
        }

        public class Circle : MyShape
        {
            public override int SpecialProp { get; set; }

            double radius;

            public Circle(double r)
            {
                radius = r;
            }

            public override double Area()
            {
                return Math.PI * Math.Pow(radius, 2);
            }
        }

        public class Square : MyShape
        {
            public override int SpecialProp { get; set; }

            double side;

            public Square(double s)
            {
                side = s;
            }
            public override double Area()
            {
                return Math.Pow(side, 2);
            }
        }
    }
}
