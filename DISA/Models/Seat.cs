using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class Seat
    {
        int number;

        public Seat(int number)
        {
            this.Number = number;
        }

        public int Number { get => number; set => number = value; }
    }
}
