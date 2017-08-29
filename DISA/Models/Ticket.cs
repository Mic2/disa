using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class Ticket
    {
        string movieName;
        int showtime;
        int line;
        int seat;
        int price;
        Customer customer;
        public Ticket()
        {
            this.MovieName = movieName;
            this.Showtime = showtime;
            this.Line = line;
            this.Seat = seat;
            this.Price = price;
            this.Customer = customer;
        }

        public string MovieName { get => movieName; set => movieName = value; }
        public int Showtime { get => showtime; set => showtime = value; }
        public int Line { get => line; set => line = value; }
        public int Seat { get => seat; set => seat = value; }
        public int Price { get => price; set => price = value; }
        public Customer Customer { get => customer; set => customer = value; }
    }
}
