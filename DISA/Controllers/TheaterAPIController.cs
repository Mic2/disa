using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DISA.Models;
using System.Diagnostics;

namespace DISA.Controllers
{
    [Produces("application/json")]
    [Route("api/TheaterAPI")]
    public class TheaterAPIController : Controller
    {
        [Route("/api/getTheater")]
        [HttpPost]
        public List<Line> GetTheaterInformation([FromBody]string val)
        {
            
            try
            {
                Debug.WriteLine(val);
                // Storing data about the theater that the user has choosen, this we only want to do with AJAX when the user clicks an time with a theater attached.
                List<Line> lineList = GetTheaterLinesAndSeats(Convert.ToInt32(val)).Lines;
                return lineList;
            }
            catch (Exception e)
            {
                List<Line> lineList = new List<Line>();
                Debug.WriteLine(e);
                return lineList;
               
            }
                      
        }

        private Theater GetTheaterLinesAndSeats(int theaterNumber)
        {
            Theater theater = new Theater();
            theater.Number = Convert.ToInt32(theaterNumber);
            theater.Lines = DalManager.Instance.GetTheaterLines(theaterNumber);

            return theater;
        }
    }
}