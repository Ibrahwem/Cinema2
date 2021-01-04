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
        public ActionResult Index(FormCollection form)
        {
            string cat = form["Categories"];
            ViewBag.Cat = cat;
            Database1Entities dbe = new Database1Entities();
            //System.Diagnostics.Debug.WriteLine("---|");
            //System.Diagnostics.Debug.WriteLine(dbe.Movies.ToList().OrderByDescending(p => p.price));
            //System.Diagnostics.Debug.WriteLine("---|");
            return View(dbe.Movies.ToList());
        }
       

        
    }
}