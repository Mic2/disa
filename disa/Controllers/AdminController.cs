using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DISA.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;

namespace DISA.Controllers
{
    public class AdminController : Controller
    {
        private IHostingEnvironment _environment;

        public AdminController(IHostingEnvironment environment)
        {
            this._environment = environment;
        }

        public IActionResult InsertMovie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertMovie(ICollection<IFormFile> files)
        {
            string fileName = "Not set";
            var uploads = Path.Combine(_environment.WebRootPath, "images");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        fileName = file.FileName;
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            // Getting post data
            string movieName = Request.Form["movieName"];
            string movieType = Request.Form["type"];
            string movieRuntime = Request.Form["runtime"];
            string movieDescription = Request.Form["description"];
            string theater = Request.Form["theater"];
            // Comma seperated list of showtimes
            string movieShowTime = Request.Form["showtime"];

            string[] showTimesSplitted = movieShowTime.Split(',');
            string[] theatersSplitted = theater.Split(',');

            Movie movieToCreate = new Movie(movieName, movieType, movieRuntime, movieDescription, "/images/" + fileName);
            DalManager.Instance.InsertMovie(movieToCreate);
            int count = 0;

            // Inserting all of the showtimes inserted by the user.
            foreach (string showTime in showTimesSplitted)
            {         
                DalManager.Instance.CheckShowTimeExistence(showTime);
                DalManager.Instance.InsertShowTime(movieToCreate.Name, showTime, Convert.ToInt32(theatersSplitted[count]));
                count = count + 1;
            }
            

            return View();
        }
    }


}