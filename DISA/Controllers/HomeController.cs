using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DISA.Models;

namespace DISA.Controllers
{
    public class HomeController : Controller
    {

        
        public IActionResult Index()
        {
            /*Movie movie = new Movie("Awsome movie .dk", "premiere", 90, "awsome", 90, " ");

            Movie movie2 = DalManager.Instance.GetMovie(movie);*/

            List<Movie> moviesToDisplay = TheaterManager.Instance.GetAllMoviesByShowTime();
            ViewData["Movies"] = moviesToDisplay;
            return View();
        }

        public IActionResult Movie()
        {
            // Code for testing retrieval of form post data.
            // ViewData["FormData"]= Request.Form["formdata"];


            ViewData["Message"] = "This is the movie reservation page!";

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
