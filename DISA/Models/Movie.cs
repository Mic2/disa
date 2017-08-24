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
        int runTime;
        int price;
        string coverImage;
        List<ShowTime> showTimes = new List<ShowTime>();

        public Movie(string name,string type, int runTime, string description, int price, string coverImage)
        {
            Name = name;
            Type = type;
            Description = description;
            RunTime = runTime;
            Price = price;
            CoverImage = coverImage;
        }

        public string Name { get => Name1; set => Name1 = value; }
        public string Type { get => type; set => type = value; }
        public string Name1 { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int RunTime { get => runTime; set => runTime = value; }
        public int Price { get => price; set => price = value; }
        public string CoverImage { get => coverImage; set => coverImage = value; }
        public List<ShowTime> ShowTimes { get => showTimes; set => showTimes = value; }
        
    }
}
