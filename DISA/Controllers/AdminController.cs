using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DISA.Models;
using System.Diagnostics;

namespace DISA.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult InsertMovie()
        {
            try
            {
                string submit = Request.Form["movieName"];
                if (String.IsNullOrEmpty(submit))
                {
                    // Getting post data
                    string movieName = Request.Form["movieName"];
                    string movieType = Request.Form["type"];
                    int movieRuntime = Convert.ToInt32(Request.Form["runtime"]);
                    string movieDescription = Request.Form["description"];
                    string movieCoverImage = Request.Form["converImage"];

                    if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
                    {
                        string fn = System.IO.Path.GetFileName(File1.PostedFile.FileName);
                        string SaveLocation = Server.MapPath("Data") + "\\" + fn;
                        try
                        {
                            File1.PostedFile.SaveAs(SaveLocation);
                            Response.Write("The file has been uploaded.");
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Error: " + ex.Message);
                            //Note: Exception.Message returns detailed message that describes the current exception. 
                            //For security reasons, we do not recommend you return Exception.Message to end users in 
                            //production environments. It would be better just to put a generic error message. 
                        }
                    }
                    else
                    {
                        Response.Write("Please select a file to upload.");
                    }

                    Movie movieToCreate = new Movie(movieName, movieType, movieRuntime, movieDescription, movieCoverImage);
                    DalManager.Instance.InsertMovie(movieToCreate);
                    return View();
                }
            } catch (InvalidOperationException e)
            {
                Debug.WriteLine(e);
            }
            return View();
        }


    }
}