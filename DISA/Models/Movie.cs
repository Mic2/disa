using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class Movie
    {
        string name;
        string type;
        string description;
        string runTime;
        string coverImage;
        int ticketPrice;
        List<ShowTime> showTimes = new List<ShowTime>();

        public Movie(string name,string type, string runTime, string description, string coverImage)
        {
            Name = name;
            Type = type;
            Description = description;
            RunTime = runTime;
            CoverImage = coverImage;
        }

        public string Type { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string RunTime { get => runTime; set => runTime = value; }
        public string CoverImage { get => coverImage; set => coverImage = value; }
        public List<ShowTime> ShowTimes { get => showTimes; set => showTimes = value; }
        public int TicketPrice { get => ticketPrice; set => ticketPrice = value; }
    }
}
