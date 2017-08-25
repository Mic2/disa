﻿using MySql.Data.MySqlClient;
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

        public Movie GetMovie(string movieName)
        {
            string query = "SELECT * FROM Movie INNER JOIN MovieType ON Movie.FK_type = MovieType.PK_type WHERE Movie.PK_movieName = '"+ movieName + "'";
            Movie newMovie = null;
            
            if (ConnectToDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    newMovie = new Movie(dataReader["PK_movieName"].ToString(), dataReader["FK_type"].ToString(), dataReader["runTime"].ToString(), dataReader["description"].ToString(), dataReader["coverImage"].ToString());
                }
                Debug.WriteLine(newMovie.Name);


                //close Data Reader
                dataReader.Close();

                //close Connection
                con.Close();

                return newMovie;
                }
            else
            {
                return newMovie;
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

        public List<Movie> GetAllMoviesByShowTime()
        {
            string query = "SELECT * FROM Movie INNER JOIN ShowTime ON Movie.PK_movieName = ShowTime.FK_movieName WHERE ShowTime.FK_time > NOW()";
            Movie newMovie = null;
            List<Movie> movieList = new List<Movie>();

            if (ConnectToDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                string movieName = null;
                ShowTime showTime;
                Theater theater;

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    // Making sure we only use unique movieNames displayed on the frontpage
                    // And inserting times to the list on the movie if we are getting more than 1 showtime
                    if(dataReader["PK_movieName"].ToString() != movieName) {
                        movieName = dataReader["PK_movieName"].ToString();

                        newMovie = new Movie(dataReader["PK_movieName"].ToString(), dataReader["FK_type"].ToString(), dataReader["runTime"].ToString(), dataReader["description"].ToString(), dataReader["coverImage"].ToString());
                        showTime = new ShowTime();
                        theater = new Theater();
                        showTime.Time = dataReader["FK_time"].ToString();
                        theater.Number = Convert.ToInt32(dataReader["FK_theaterNumber"]);
                        showTime.Theater = theater;
                        newMovie.ShowTimes.Add(showTime);
                        movieList.Add(newMovie);
                    }
                    else
                    {
                        showTime = new ShowTime();
                        theater = new Theater();
                        theater.Number = Convert.ToInt32(dataReader["FK_theaterNumber"]);
                        showTime.Theater = theater;
                        showTime.Time = dataReader["FK_time"].ToString();
                        newMovie.ShowTimes.Add(showTime);
                    }
                }
                Debug.WriteLine(newMovie.Name);


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
    }
}
