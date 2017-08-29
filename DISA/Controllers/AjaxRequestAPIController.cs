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

namespace DISA.Controllers
{
    [Produces("application/json")]
    [Route("api/TheaterAPI")]
    public class AjaxRequestAPIController : Controller
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

        [Route("/api/getMoviesByDate")]
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

        private Theater GetTheaterLinesAndSeats(int theaterNumber)
        {
            Theater theater = new Theater();
            theater.Number = Convert.ToInt32(theaterNumber);
            theater.Lines = DalManager.Instance.GetTheaterLines(theaterNumber);

            return theater;
        }

        [Route("/api/insertCustomer")]
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

        [Route("/api/insertTicket")]
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