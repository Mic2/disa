using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class Theater
    {
            int number;
            List<Line> lines;

            public Theater(int number, List<Line> lines)
            {
                this.Number = number;
                this.Lines = lines;
            }

            public int Number { get => number; set => number = value; }
            public List<Line> Lines { get => lines; set => lines = value; }
    }
}
