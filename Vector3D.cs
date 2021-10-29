using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Vector3D
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        // форматированный вывод координат точки с проверкой NaN
        public void Print()
        {
            if (!double.IsNaN(this.X) && !double.IsNaN(this.Y) && !double.IsNaN(this.Z))
            {
                Console.WriteLine(
                    "Координаты точки:\n" +
                    $"X: {this.X}\n" +
                    $"Y: {this.Y}\n" +
                    $"Z: {this.Z}"
                    );
            }
        }
    }
}
