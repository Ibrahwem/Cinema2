using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCinema.Models;
using System.Data.Entity;

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
            string ord= form["order"];
            var emps = dbe.Movies;
            if(ord=="1")
            {
                var emps1 = from e in dbe.Movies
                           orderby e.price 
                           select e;
                return View(emps1.ToList());
            }
            if(ord=="2")
            {
                var emps1 = from e in dbe.Movies
                           orderby e.price descending
                           select e;
                return View(emps1.ToList());
            }
            if (ord == "3")
            {
                var emps1 = from e in dbe.Movies
                            orderby e.Category
                            select e;
                return View(emps1.ToList());
            }

            return View(emps.ToList());
        }

        
       

        
    }
}