using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DISA.Models;
using System.Diagnostics;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DISA.Controllers
{
    public class HomeController : Controller
    {
        //public string LoggedInUser => User.Identity.Name.ToString();

        public IActionResult Index()
        {
            List<Movie> moviesToDisplay = TheaterManager.Instance.GetAllMoviesByShowTime();
            ViewData["Movies"] = moviesToDisplay;


            List<DateTime> DatesWithShowTime = new List<DateTime>();
            foreach (Movie movie in moviesToDisplay)
            {
                foreach (ShowTime showTime in movie.ShowTimes)
                {
                    DatesWithShowTime.Add(showTime.Time.Date);
                }
            }
            DatesWithShowTime = DatesWithShowTime.Distinct().ToList();
            ViewData["DatesWithShowTime"] = DatesWithShowTime;

            

            //ViewData["userId"] = LoggedInUser;

            return View();
        }

        public IActionResult Movie()
        {
            // Trying to retrive GET parameter of movieName.
            try
            {
                string movieName = Request.Query["movieName"];

                // Collecting information about the movie from the DB
                List<Movie> movieDetailsList = DalManager.Instance.GetMovieDetails(movieName);

                // Storing data from DB about the movie to be displayed on the movie page
                ViewData["movieName"] = movieName;
                ViewData["movieDetailsList"] = movieDetailsList;
                ViewData["movieDescription"] = movieDetailsList[0].Description;
                ViewData["movieCoverImage"] = movieDetailsList[0].CoverImage;
                ViewData["movieRunTime"] = movieDetailsList[0].RunTime;
                ViewData["movieType"] = movieDetailsList[0].Type;
                ViewData["movieTicketPrice"] = movieDetailsList[0].TicketPrice;
                ViewData["showTimes"] = movieDetailsList[0].ShowTimes;

                
            }
            catch(InvalidOperationException e)
            {
                Debug.WriteLine(e);
            }
            return View();
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
