using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public List<Movie> GetAllMoviesByShowTime()
        {
            Debug.WriteLine("#############################################################");
            Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Debug.WriteLine("#############################################################");
            List<Movie> moviesToDisplay = DalManager.Instance.GetAllMoviesByShowTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ">");
            return moviesToDisplay;
        }

        public List<Theater> Theaters { get => theaters; set => theaters = value; }
        public List<Movie> Movies { get => movies; set => movies = value; }
    }
}