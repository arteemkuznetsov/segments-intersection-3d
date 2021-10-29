using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Segment3D
    {
        public Vector3D Start;
        public Vector3D End;

        public Segment3D(Vector3D start, Vector3D end)
        {
            this.Start = start;
            this.End = end;
        }
    }
}
