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

        public Movie GetMovie(Movie movie)
        {
            string query = "SELECT * FROM Movie INNER JOIN MovieType ON Movie.FK_type = MovieType.PK_type WHERE Movie.PK_movieName = 'Awsome movie .dk'";
            Movie newMovie = null;
            
            if (ConnectToDB() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    newMovie = new Movie(dataReader["PK_movieName"].ToString(), dataReader["FK_type"].ToString(), Convert.ToInt32(dataReader["runTime"]), dataReader["description"].ToString(), Convert.ToInt32(dataReader["price"]), dataReader["coverImage"].ToString());               
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
