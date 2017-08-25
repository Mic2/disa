using DISA.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace DISA.tmpfiles
{
    public class CreateTheaters
    {
        private MySqlConnection con;
        private int lineIdCount = 1;

        public CreateTheaters()
        {
            CreateDBConnection();
            InsertTheathers();
        }

        private void CreateDBConnection()
        {
            List<string> dbSettings = ReadDbConnectionSettings();
            con = new MySqlConnection("Server="+ dbSettings[0] + ";port="+ dbSettings[1] + ";database="+ dbSettings[2] + ";uid="+ dbSettings[3] + ";pwd="+ dbSettings[4] +";SslMode=none;");
            try
            {
                con.Open();           
                Console.WriteLine("Success on opening connection to the database!");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error on opening connection to the database: Stacktrace = " + ex);          
            }
        }

        private void InsertTheathers()
        {
            // Looping throug theathers - We will need 10 with different seatnumbers
            for(var i = 1; i <= 10; i++)
            {
                
                // Now insert the number to db
                InsertTheaterToDb(i);

                // Check on the theaterNumber (i), how many lines it needs.
                if (i <= 2) {

                    // Now insert all the lines in big theaters
                    for(var line = 1; line <= 13; line++)
                    {
                        
                        // Insert the lines in db
                        InsertLine(line, i);

                        // Insert seat to db referenced to the line
                        for (var seat = 1; seat <= 24; seat++)
                        {
                            InsertSeats(lineIdCount, seat);
                        }
                        lineIdCount = lineIdCount + 1;
                    }

                }
                else if(i <= 4 && i > 2)
                {
                    // Now insert all the lines in medium theaters
                    for (var line = 1; line <= 9; line++)
                    {
                        // Insert the lines in db
                        InsertLine(line, i);
                        
                        // Insert seat to db referenced to the line
                        for(var seat = 1; seat <= 17; seat++)
                        {
                            InsertSeats(lineIdCount, seat);
                        }
                        lineIdCount = lineIdCount + 1;
                    }
                }
                else
                {
                    // Now insert all the lines in small theaters
                    for (var line = 1; line <= 7; line++)
                    {
                        // Insert the lines in db
                        InsertLine(line, i);

                        // Small theater have special seat numbers on first line
                        if(line <= 1)
                        {
                            // Insert seat to db referenced to the line
                            for (var seat = 1; seat <= 9; seat++)
                            {
                                InsertSeats(lineIdCount, seat);
                            }
                            lineIdCount = lineIdCount + 1;
                        }
                        else
                        {
                            // Insert seat to db referenced to the line
                            for (var seat = 1; seat <= 12; seat++)
                            {
                                InsertSeats(lineIdCount, seat);
                            }
                            lineIdCount = lineIdCount + 1;
                        }
                        
                    }
                }

                

            }
            con.Close();
        }

        private void InsertTheaterToDb(int theatherNumber)
        {
            MySqlCommand comm = con.CreateCommand();
            comm.CommandText = "INSERT INTO Theater(PK_theaterNumber) VALUES(@theaterNumber)";
            comm.Parameters.AddWithValue("@theaterNumber", theatherNumber);
            comm.ExecuteNonQuery();
        }

        private void InsertLine(int lineNumber, int theaterNumber)
        {
            MySqlCommand comm = con.CreateCommand();
            comm.CommandText = "INSERT INTO Line(FK_theaterNumber, lineNumber) VALUES(@theaterNumber, @lineNumber)";
            comm.Parameters.AddWithValue("@theaterNumber", theaterNumber);
            comm.Parameters.AddWithValue("@lineNumber", lineNumber);
            comm.ExecuteNonQuery();
            
        }

        private void InsertSeats(int lineId, int seatNumber)
        {
            MySqlCommand comm = con.CreateCommand();
            comm.CommandText = "INSERT INTO Seat(FK_lineId, seatNumber) VALUES(@lineId, @seatNumber)";
            comm.Parameters.AddWithValue("@lineId", lineId);
            comm.Parameters.AddWithValue("@seatNumber", seatNumber);
            comm.ExecuteNonQuery();
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
