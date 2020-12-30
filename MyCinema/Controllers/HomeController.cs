using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyCinema.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Database1Entities dbe = new Database1Entities();
            return View(dbe.Movies.ToList());
        }
       

        
    }
}