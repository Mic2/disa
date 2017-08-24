using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DISA.Models
{
    public class TheaterManager
    {
        private List<Theater> theaters = new List<Theater>();
        private List<Movie> movies = new List<Movie>();

        private static TheaterManager _instance = null;
        private TheaterManager()
        {
        }
        public static TheaterManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TheaterManager();
                }
                return _instance;
            }
        }

        public List<Theater> Theaters { get => theaters; set => theaters = value; }
        internal List<Movie> Movies { get => movies; set => movies = value; }
    }
}
