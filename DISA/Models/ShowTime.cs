using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class ShowTime
    {
        string time;
        Theater theater;

        public ShowTime() { }

        public string Time { get => time; set => time = value; }
        public Theater Theater { get => theater; set => theater = value; }
    }
}
