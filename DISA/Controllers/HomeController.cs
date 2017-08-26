using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DISA.Models;
using System.Diagnostics;

namespace DISA.Controllers
{
    public class HomeController : Controller
    {

        
        public IActionResult Index()
        {
            /*Movie movie = new Movie("Awsome movie .dk", "premiere", 90, "awsome", 90, " ");

            Movie movie2 = DalManager.Instance.GetMovie(movie);*/

            List<ShowTime> DatesWithShowTime = new List<ShowTime>();
            ShowTime showTime = new ShowTime();
            showTime.Time = "8/8-2017";
            DatesWithShowTime.Add(showTime);
            ViewData["DatesWithShowTime"] = DatesWithShowTime;

            List<Movie> moviesToDisplay = TheaterManager.Instance.GetAllMoviesByShowTime();
            ViewData["Movies"] = moviesToDisplay;
            return View();
        }

        public IActionResult Movie()
        {
            // Code for testing retrieval of form post data.
            // ViewData["FormData"]= Request.Form["formdata"];
            try
            {
                string movieName = Request.Form["movieName"];
                List<Movie> movieDetailsList = DalManager.Instance.GetMovieDetails(movieName);
                ViewData["movieName"] = movieName;
                ViewData["movieDetailsList"] = movieDetailsList;

                ViewData["movieDescription"] = movieDetailsList[0].Description;
                ViewData["movieCoverImage"] = movieDetailsList[0].CoverImage;
                ViewData["movieRunTime"] = movieDetailsList[0].RunTime;
                ViewData["movieType"] = movieDetailsList[0].Type;
                ViewData["movieTicketPrice"] = movieDetailsList[0].TicketPrice;
                ViewData["showTimes"] = movieDetailsList[0].ShowTimes;

                ViewData["theaterLines"] = GetTheaterLinesAndSeats(1).Lines;
            }
            catch(InvalidOperationException e)
            {
                Debug.WriteLine(e);
            }
            return View();
        }

        private Theater GetTheaterLinesAndSeats(int theaterNumber)
        {
            Theater theater = new Theater();
            theater.Number = Convert.ToInt32(theaterNumber);
            theater.Lines = DalManager.Instance.GetTheaterLines(theaterNumber);

            return theater;
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "This is the Contact page";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
