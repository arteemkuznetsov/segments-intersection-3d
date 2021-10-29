using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        // перечисление типов расположений отрезков в трехмерном пространстве
        enum SegmentStatus
        {
            DifferentPlane = 1,
            SamePlaneParallel = 2,
            SamePlaneIntersected = 3
        }

        // инициализация точек, отрезков, предварительная оценка расположения отрезков в пространстве, нахождение точки пересечения
        static void Main(string[] args)
        {
            Vector3D a1 = new Vector3D(4, -1, 2);
            Vector3D a2 = new Vector3D(-4, 8, 5);

            Vector3D b1 = new Vector3D(4, -4, 2);
            Vector3D b2 = new Vector3D(-28, 38, 14);

            Segment3D segment1 = new Segment3D(a1, a2);
            Segment3D segment2 = new Segment3D(b1, b2);

            SegmentStatus segmentStatus = getSegmentStatus(segment1, segment2);
            PrintSegmentStatus(segmentStatus);

            if (segmentStatus == SegmentStatus.SamePlaneIntersected)
            {
                Vector3D point = Intersect(segment1, segment2);
                point.Print();
            }

            Console.ReadKey();
        }

        // нахождение точки пересечения через канонические уравнения прямых
        static Vector3D Intersect(Segment3D segment1, Segment3D segment2)
        {
            // каноническое уравнение первой прямой: (x-xo)/p=(y-yo)/q=(z-zo)/r
            // каноническое уравнение второй прямой: (x-x1)/p1=(y-y1)/q1=(z-z1)/r1

            double x0 = segment1.Start.X; 
            double y0 = segment1.Start.Y; 
            double z0 = segment1.Start.Z;

            double p = segment1.End.X - x0; 
            double q = segment1.End.Y - y0; 
            double r = segment1.End.Z - z0;

            double x1 = segment2.Start.X; 
            double y1 = segment2.Start.Y; 
            double z1 = segment2.Start.Z;

            double p1 = segment2.End.X - x1; 
            double q1 = segment2.End.Y - y1; 
            double r1 = segment2.End.Z - z1;

            double x = (x0*q*p1 - x1*q1*p - y0*p*p1 + y1*p*p1) / (q*p1 - q1*p);
            double y = (y0*p*q1 - y1*p1*q - x0*q*q1 + x1*q*q1) / (p*q1 - p1*q);
            double z = (z0*q*r1 - z1*q1*r - y0*r*r1 + y1*r*r1) / (q*r1 - q1*r);


            return new Vector3D(x, y, z); 
        }

        // оценка расположения отрезков в трехмерном пространстве через расчет определителя матрицы 3x3 и равенство
        static SegmentStatus getSegmentStatus(Segment3D segment1, Segment3D segment2)
        {
            /* обозначим начальные и конечные точки как (x1;y1;z1) и (x2;y2;z2) для первого отрезка, (x3;y3;z3) и (x4;y4;z4) для второго отрезка
             * если представленный ниже детерминант равен 0, то отрезки находятся в одной плоскости, иначе - в разных
            
            |x1-x2 y1-y2 z1-z2|
            |x3-x4 y3-y4 z3-z4|
            |x1-x3 y1-y3 z1-z3|=(x1-x2)(y3-y4)(z1-z3)-(x1-x2)(z3-z4)(y1-y3)-(y1-y2)(x3-x4)(z1-z3)+(y1-y2)(z3-z4)(x1-x3)+(z1-z2)(x3-x4)(y1-y3)-(z1-z2)(y3-y4)(x1-x3)
            
            если отрезки находятся в одной плоскости, и при этом соблюдается описанное ниже равенство, то прямые параллельны, иначе - пересекаются

            (x1-x2)/(x3-x4)=(y1-y2)(y3-y4)=(z1-z2)(z3-z4)         
             */

            SegmentStatus segmentStatus;

            double determinant = (segment1.Start.X - segment1.End.X) * (segment2.Start.Y - segment2.End.Y) * (segment1.Start.Z - segment2.Start.Z) - 
                (segment1.Start.X - segment1.End.X) * (segment2.Start.Z - segment2.End.Z) * (segment1.Start.Y - segment2.Start.Y) - 
                (segment1.Start.Y - segment1.End.Y) * (segment2.Start.X - segment2.End.X) * (segment1.Start.Z - segment2.Start.Z) + 
                (segment1.Start.Y - segment1.End.Y) * (segment2.Start.Z - segment2.End.Z) * (segment1.Start.X - segment2.Start.X) + 
                (segment1.Start.Z - segment1.End.Z) * (segment2.Start.X - segment2.End.X) * (segment1.Start.Y - segment2.Start.Y) - 
                (segment1.Start.Z - segment1.End.Z) * (segment2.Start.Y - segment2.End.Y) * (segment1.Start.X - segment2.Start.X);

            if (determinant != 0)
            {
                segmentStatus = SegmentStatus.DifferentPlane;
            } 
            else
            {
                double expressionX = (segment1.Start.X - segment1.End.X) / (segment2.Start.X - segment2.End.X);
                double expressionY = (segment1.Start.Y - segment1.End.Y) / (segment2.Start.Y - segment2.End.Y);
                double expressionZ = (segment1.Start.Z - segment1.End.Z) / (segment2.Start.Z - segment2.End.Z);

                if (expressionX == expressionY && expressionY == expressionZ)
                {
                    segmentStatus = SegmentStatus.SamePlaneParallel;
                }
                else
                {
                    segmentStatus = SegmentStatus.SamePlaneIntersected;
                }
            }

            return segmentStatus;
        }

        // вывод результата оценки расположения отрезков в трехмерном пространстве
        static void PrintSegmentStatus(SegmentStatus segmentStatus)
        {
            switch(segmentStatus)
            {
                case SegmentStatus.DifferentPlane:
                    Console.WriteLine("Отрезки лежат в разных плоскостях. Они могут скрещиваться, но не могут иметь точки пересечения");
                    break;
                case SegmentStatus.SamePlaneParallel:
                    Console.WriteLine("Отрезки лежат в одной плоскости. Они параллельны и не могут иметь точки пересечения");
                    break;
                case SegmentStatus.SamePlaneIntersected:
                    Console.WriteLine("Отрезки лежат в одной плоскости. Они имеют точку пересечения");
                    break;
                default:
                    Console.WriteLine("Статус расположения отрезков не определен");
                    break;
            }
        }

    }
}
