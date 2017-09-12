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
    [Route("api/TheaterAPI")]
    public class AjaxRequestAPIController : Controller
    {
        [AllowAnonymous]
        [Route("/api/getTheater")]
        [HttpPost]
        public List<Line> GetTheaterInformation([FromBody]JObject data)
        {
            // Storing Json data in Models
            dynamic json = data;
            JObject jsonTheater = json.Theater;
            JObject jsonShowTime = json.ShowTime;
            Theater theater = jsonTheater.ToObject<Theater>();
            ShowTime showTime = jsonShowTime.ToObject<ShowTime>();

            try
            {
                // Storing data about the theater that the user has choosen, this we only want to do with AJAX when the user clicks an time with a theater attached.
                List<Line> lineList = GetTheaterLinesAndSeats(Convert.ToInt32(theater.Number), showTime.ShowTimeId ).Lines;
                return lineList;
            }
            catch (Exception e)
            {
                // IF something went wrong we return an empty list.
                List<Line> lineList = new List<Line>();
                Line line = new Line(0, new List<Seat>());
                return lineList;
               
            }
                      
        }

        [AllowAnonymous]
        [Route("/api/getMoviesByDate")]
        [HttpPost]
        public List<Movie> GetMoviesByDate([FromBody]string val)
        {
            try
            {
                List<Movie> movieList = new List<Movie>();
                // Now if the user dont have made a date choice on the frontpage we call the default, wich will give all movies with a showtime higher that right now
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
                // If there was an error getting data, we return an empty list
                List<Movie> movieList = new List<Movie>();
                Debug.WriteLine(e);
                return movieList;

            }

        }

        private Theater GetTheaterLinesAndSeats(int theaterNumber, int showTimeId)
        {
            // Creating new instance of theater manager, and using this in GetTheaterInformation() method
            Theater theater = new Theater();
            theater.Number = theaterNumber;
            theater.Lines = DalManager.Instance.GetTheaterLines(theaterNumber, showTimeId);

            return theater;
        }

        [AllowAnonymous]
        [Route("/api/insertCustomer")]
        [HttpPost]
        public void InsertCustomerFromReservation([FromBody]JObject data)
        {
            try
            {
                // Storing Json data in Customer object, and inserting the customer in the database
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

        [AllowAnonymous]
        [Route("/api/insertTicket")]
        [HttpPost]
        public int InsertTicketFromReservation([FromBody]JObject data)
        {
            int result = 0;
            try
            {
                // Storing Json data in Models for use on the DBManager to store the reservation.
                dynamic json = data;
                JObject customer = json.Customer;
                JObject showTime = json.ShowTime;
                JArray seatIds = json.SeatIds;
                
                Customer newCustomer = customer.ToObject<Customer>();
                ShowTime newShowTime = showTime.ToObject<ShowTime>();
                List<int> seatList = seatIds.ToObject<List<int>>();

                Debug.WriteLine("THIS IS the seatidlist"+seatList);

                result = DalManager.Instance.InsertTicket(Convert.ToInt32(newCustomer.PhoneNumber), newShowTime.ShowTimeId, seatList);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return result;
        }
    }
}