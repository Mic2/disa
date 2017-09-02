using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace DISA.Models
{
    public class DalManager
    {

        //singleton
        private static DalManager _instance = null;
        private MySqlConnection con;
        private DalManager() { }
        public static DalManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DalManager();
                }
                return _instance;
            }
        }

        private bool ConnectToDB() {

            List<string> dbSettings = ReadDbConnectionSettings();
            con = new MySqlConnection("Server="+ dbSettings[0] + ";port="+ dbSettings[1] + ";database="+ dbSettings[2] + ";uid="+ dbSettings[3] + ";pwd="+ dbSettings[4] +";SslMode=none;");
            try
            {
                con.Open();           
                Console.WriteLine("Success on opening connection to the database!");
                return true;

            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error on opening connection to the database: Stacktrace = " + ex);
                return false;
            }
            
        }

        public string GetMovieRelatedTheater(string showTimeId) {
            string query = "SELECT FK_theaterNumber FROM ShowTime WHERE FK_showTimeId = '"+ showTimeId + "'";
            string theaterNumber = "";
            if (ConnectToDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    theaterNumber = dataReader["FK_theaterNumber"].ToString();
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                con.Close();

            }
            return theaterNumber;
        }

        public List<Movie> GetMovieDetails(string movieName)
        {
            string query = "SELECT * FROM ((Movie INNER JOIN MovieType ON Movie.FK_type = MovieType.PK_type) INNER JOIN ShowTime ON Movie.PK_movieName = ShowTime.FK_movieName) WHERE Movie.PK_movieName = '" + movieName + "'";
            Movie newMovie = null;
            List<Movie> movieDetailsList = new List<Movie>();

            if (ConnectToDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                ShowTime showTime;
                Theater theater;

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    newMovie = new Movie(dataReader["PK_movieName"].ToString(), dataReader["FK_type"].ToString(), dataReader["runTime"].ToString(), dataReader["description"].ToString(), dataReader["coverImage"].ToString());
                    newMovie.TicketPrice = Convert.ToInt32(dataReader["price"]);
                    showTime = new ShowTime();
                    theater = new Theater();
                    showTime.ShowTimeId = Convert.ToInt32(dataReader["PK_showTimeId"]);
                    showTime.Time = Convert.ToDateTime(dataReader["FK_time"]);
                    theater.Number = Convert.ToInt32(dataReader["FK_theaterNumber"]);
                    showTime.Theater = theater;
                    newMovie.ShowTimes.Add(showTime);

                    movieDetailsList.Add(newMovie);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                con.Close();

                return movieDetailsList;
                }
            else
            {
                //close Connection
                con.Close();

                return movieDetailsList;
            }
        }

        // Returns true if there was an record
        public void CheckShowTimeExistence(string dateTime)
        {
            string returnValue = null;
            // Lets check if the date is already in the db.
            // If not then we insert it to time.
            string query = "SELECT time FROM Time WHERE time = '" + dateTime + "'";

            if(ConnectToDB() == true) {

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                        returnValue = dataReader["time"].ToString();
                }
                //close Data Reader
                dataReader.Close();

                //close Connection
                con.Close();
            }

            // Now if there is no time in db, then we create one.
            if (returnValue == null) {
                if (ConnectToDB() == true)
                {
                    MySqlCommand comm = con.CreateCommand();
                    comm.CommandText = "INSERT INTO Time (time) VALUES(@time)";
                    comm.Parameters.AddWithValue("@time", dateTime);
                    comm.ExecuteNonQuery();

                    //close Connection
                    con.Close();
                }
                
            }

        }

        public void InsertShowTime(string movieName, string dateTime, int theaterNumber)
        {
            if (ConnectToDB() == true)
            {
                
                MySqlCommand comm = con.CreateCommand();
                comm.CommandText = "INSERT INTO ShowTime(FK_movieName, FK_theaterNumber, FK_time) VALUES(@movieName, @theaterNumber, @showTime)";
                comm.Parameters.AddWithValue("@movieName", movieName);
                comm.Parameters.AddWithValue("@showTime", dateTime);
                comm.Parameters.AddWithValue("@theaterNumber", theaterNumber);
                comm.ExecuteNonQuery();
            }

            con.Close();
        }

        public List<Movie> GetAllMoviesByShowTime(string date, string operater)
        {
            string query = "SELECT * FROM Movie INNER JOIN ShowTime ON Movie.PK_movieName = ShowTime.FK_movieName WHERE ShowTime.FK_time " + operater + "  '" + date + "'";
            Movie newMovie = null;
            List<Movie> movieList = new List<Movie>();

            if (ConnectToDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                List<string> movieNames = new List<string>();
                ShowTime showTime;
                Theater theater;
                bool listCheck = false;  

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    // Making sure we only use unique movieNames displayed on the frontpage
                    // And inserting times to the list on the movie if we are getting more than 1 showtime

                    if (movieNames.Contains(dataReader["PK_movieName"].ToString()))
                    {
                        listCheck = true;
                    }

                    if(listCheck == false)
                    {

                        newMovie = new Movie(dataReader["PK_movieName"].ToString(), dataReader["FK_type"].ToString(), dataReader["runTime"].ToString(), dataReader["description"].ToString(), dataReader["coverImage"].ToString());
                        showTime = new ShowTime();
                        theater = new Theater();
                        showTime.ShowTimeId = Convert.ToInt32(dataReader["PK_showTimeId"]);
                        showTime.Time = Convert.ToDateTime(dataReader["FK_time"]);
                        theater.Number = Convert.ToInt32(dataReader["FK_theaterNumber"]);
                        showTime.Theater = theater;
                        newMovie.ShowTimes.Add(showTime);
                        movieNames.Add(dataReader["PK_movieName"].ToString());
                        movieList.Add(newMovie);
                    }
                    else
                    {
                        showTime = new ShowTime();
                        theater = new Theater();
                        theater.Number = Convert.ToInt32(dataReader["FK_theaterNumber"]);
                        showTime.Theater = theater;
                        showTime.Time = Convert.ToDateTime(dataReader["FK_time"]);
                        newMovie.ShowTimes.Add(showTime);
                        
                    }

                    listCheck = false;

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                con.Close();

                return movieList;
            }
            else
            {
                return movieList;
            }
        }

        public void InsertMovie(Movie movie)
        {
            if (ConnectToDB() == true)
            {
                MySqlCommand comm = con.CreateCommand();
                
                comm.CommandText = "INSERT INTO Movie(PK_movieName, FK_type, runTime, description, coverImage) VALUES(@movieName, @movieType, @runTime, @description, @coverImage)";
                comm.Parameters.AddWithValue("@movieName", movie.Name);
                comm.Parameters.AddWithValue("@movieType", movie.Type);
                comm.Parameters.AddWithValue("@runTime", movie.RunTime);
                comm.Parameters.AddWithValue("@description", movie.Description);
                comm.Parameters.AddWithValue("@coverImage", movie.CoverImage);
                comm.ExecuteNonQuery();
            }

            con.Close();
        }
                
        public List<Line> GetTheaterLines(int theaterNumber, int showTimeId)
        {
            string query = "SELECT PK_lineId, lineNumber FROM ShowTime INNER JOIN Theater ON ShowTime.FK_theaterNumber = Theater.PK_theaterNumber INNER JOIN Line ON Line.FK_theaterNumber = ShowTime.FK_theaterNumber WHERE ShowTime.PK_showTimeId = '"+ showTimeId + "'";
            Debug.WriteLine(query);
            List<Line> lines = new List<Line>();
            Line line = null;

            List<Seat> seats = new List<Seat>();

            if (ConnectToDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    // Now getting all of the seats based on the line id
                    seats = GetTheaterLineSeats(dataReader["PK_lineId"].ToString(), showTimeId);

                    // creating new line with all the seats it has and placing it in the line List
                    line = new Line(Convert.ToInt32(dataReader["lineNumber"]), seats);
                    lines.Add(line);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                con.Close();

                return lines;
            }
            else
            {
                return lines;
            }
        }

        private List<int> GetReservedSeats(string lineId, int showTimeId)
        {
            string query = "SELECT FK_seatId FROM Ticket INNER JOIN Seat ON Seat.PK_seatId = Ticket.FK_seatId WHERE FK_lineId = "+lineId+" AND Ticket.FK_showTimeId = "+showTimeId+"";
            List<int> reservedSeats = new List<int>();

            if (ConnectToDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    reservedSeats.Add(Convert.ToInt32(dataReader["FK_seatId"]));
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                con.Close();

                return reservedSeats;
            }
            else
            {
                return reservedSeats;
            }
        }

        private List<Seat> GetTheaterLineSeats(string lineId, int showTimeId)
        {
            string query = "SELECT PK_seatId, seatNumber FROM Seat WHERE FK_lineId = '" + lineId + "'";
            List<Seat> seats = new List<Seat>();
            Seat seat = null;
            List<int> reservedSeatsList = GetReservedSeats(lineId, showTimeId);

            if (ConnectToDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    seat = new Seat(Convert.ToInt32(dataReader["seatNumber"]));
                    seat.Reserved = "free";
                    foreach (int seatId in reservedSeatsList) {
                        if (Convert.ToInt32(dataReader["PK_seatId"]) == seatId) {
                            seat.Reserved = "reserved";
                            
                        }
                        
                    }                  
                    seat.SeatId = Convert.ToInt32(dataReader["PK_seatId"]);
                    seats.Add(seat);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                con.Close();

                return seats;
            }
            else
            {
                return seats;
            }
        }

        private List<string> ReadDbConnectionSettings()
        {
            List<string> dbSettings = new List<string>();

            // Create an XML reader for this file.
            using (XmlReader reader = XmlReader.Create("db.xml"))
            {
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "server":
                                if (reader.Read())
                                {
                                    string value = reader.Value.Trim();
                                    dbSettings.Add(value);
                                }
                                
                                break;
                            case "port":
                                if (reader.Read())
                                {
                                    string value = reader.Value.Trim();
                                    dbSettings.Add(value);
                                }
                                break;
                            case "database":
                                if (reader.Read())
                                {
                                    string value = reader.Value.Trim();
                                    dbSettings.Add(value);
                                }
                                break;
                            case "user":
                                if (reader.Read())
                                {
                                    string value = reader.Value.Trim();
                                    dbSettings.Add(value);
                                }
                                break;
                            case "password":
                                if (reader.Read())
                                {
                                    string value = reader.Value.Trim();
                                    dbSettings.Add(value);
                                }
                                break;
                        }
                    }
                }
            }
            return dbSettings;
        }

        public void InsertTicket(int phoneNumber, int showTimeId, int seatId)
        {
            if (ConnectToDB() == true)
            {
                MySqlCommand comm = con.CreateCommand();

                comm.CommandText = "INSERT INTO Ticket(FK_phoneNumber, FK_showTimeId, FK_seatId) VALUES(@phoneNumber, @showTimeId, @seatId)";
                comm.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                comm.Parameters.AddWithValue("@showTimeId", showTimeId);
                comm.Parameters.AddWithValue("@seatId", seatId);
                comm.ExecuteNonQuery();
            }

            con.Close();
        }

        public void InsertCustomer(string fullName, int phoneNumber)
        {
            if (ConnectToDB() == true)
            {
                MySqlCommand comm = con.CreateCommand();

                Debug.WriteLine("WE ARE IN HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                comm.CommandText = "INSERT INTO Customer(PK_phoneNumber, fullName) VALUES(@phoneNumber, @fullName)";
                comm.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                comm.Parameters.AddWithValue("@fullName", fullName);
                comm.ExecuteNonQuery();
            }

            con.Close();
        }
    }
}
