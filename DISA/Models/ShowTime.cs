using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class ShowTime
    {
        int showTimeId;
        DateTime time;
        Theater theater;

        public ShowTime() { }

        public DateTime Time { get => time; set => time = value; }
        public Theater Theater { get => theater; set => theater = value; }
        public int ShowTimeId { get => showTimeId; set => showTimeId = value; }
    }
}
