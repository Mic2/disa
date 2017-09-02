using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DISA.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace DISA.Controllers
{
    [Produces("application/json")]
    [Route("api/AjaxRequestAPI")]
    public class AjaxRequestAPIController : Controller
    {
        //[AllowAnonymous]
        //[ActionName("GetTheaterInformation")]
        [Route("/api/getTheater")]
        [HttpPost]
        public ActionResult GetTheaterInformation([FromBody]JObject data)
        {
            dynamic json = data;
            JObject jsonTheater = json.Theater;
            JObject jsonShowTime = json.ShowTime;
            Theater theater = jsonTheater.ToObject<Theater>();
            ShowTime showTime = jsonShowTime.ToObject<ShowTime>();
            Debug.WriteLine("###########################################"+showTime.ShowTimeId);

            try
            {
                // Storing data about the theater that the user has choosen, this we only want to do with AJAX when the user clicks an time with a theater attached.
                List<Line> lineList = GetTheaterLinesAndSeats(Convert.ToInt32(theater.Number), showTime.ShowTimeId ).Lines;
                return Json(lineList);
            }
            catch (Exception e)
            {
                List<Line> lineList = new List<Line>();
                Debug.WriteLine("THIS IS AN IMPORTANT ERROR  "+e);
                return Json(lineList);
               
            }
                      
        }

        //[AllowAnonymous]
        [Route("/api/getMoviesByDate")]
        //[ActionName("GetMoviesByDate")]
        [HttpPost]
        public List<Movie> GetMoviesByDate([FromBody]string val)
        {
            try
            {
                List<Movie> movieList = new List<Movie>();
                if (val == "Default")
                {
                    movieList = DalManager.Instance.GetAllMoviesByShowTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ">");
                }
                else
                {
                    // Storing daa about the theater that the user has choosen, this we only want to do with AJAX when the user clicks an time with a theater attached.
                    movieList = DalManager.Instance.GetAllMoviesByShowTime(val + '%', "like");
                }
                return movieList;
            }
            catch (Exception e)
            {
                List<Movie> movieList = new List<Movie>();
                Debug.WriteLine(e);
                return movieList;

            }

        }

        private Theater GetTheaterLinesAndSeats(int theaterNumber, int showTimeId)
        {
            Theater theater = new Theater();
            theater.Number = theaterNumber;
            theater.Lines = DalManager.Instance.GetTheaterLines(theaterNumber, showTimeId);

            return theater;
        }

        //[AllowAnonymous]
        [Route("/api/insertCustomer")]
        //[ActionName("InsertCustomerFromReservation")]
        [HttpPost]
        public void InsertCustomerFromReservation([FromBody]JObject data)
        {
            try
            {

                dynamic json = data;
                JObject customer = json.Customer;
  
                Customer newCustomer = customer.ToObject<Customer>();

                DalManager.Instance.InsertCustomer(newCustomer.FullName, Convert.ToInt32(newCustomer.PhoneNumber));

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        //[AllowAnonymous]
        [Route("/api/insertTicket")]
        //[ActionName("InsertTicketFromReservation")]
        [HttpPost]
        public void InsertTicketFromReservation([FromBody]JObject data)
        {
            try
            {

                dynamic json = data;
                JObject customer = json.Customer;
                JObject showTime = json.ShowTime;
                JObject seatId = json.SeatId;

                Customer newCustomer = customer.ToObject<Customer>();
                ShowTime newShowTime = showTime.ToObject<ShowTime>();
                Seat seat = seatId.ToObject<Seat>();

                DalManager.Instance.InsertTicket(Convert.ToInt32(newCustomer.PhoneNumber), newShowTime.ShowTimeId, seat.SeatId);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }
    }
}