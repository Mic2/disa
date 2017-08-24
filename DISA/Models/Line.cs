using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class Line
    {
        int number;
        List<Seat> seats;

        public Line(int number,List<Seat> seats)
        {
            this.Number = number;
            this.Seats = seats;
        }

        public int Number { get => number; set => number = value; }
        public List<Seat> Seats { get => seats; set => seats = value; }
    }
}
