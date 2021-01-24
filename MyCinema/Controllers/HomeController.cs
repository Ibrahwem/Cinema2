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
            using(Database1Entities db=new Database1Entities())
            {
                foreach(Movy mv in db.Movies.ToList<Movy>())
                {
                    /*System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString());
                    System.Diagnostics.Debug.WriteLine(mv.movie_date.ToString() + " " + mv.movie_time.ToString());*/
                    DateTime d = mv.movie_date.Date+TimeSpan.Parse(mv.movie_time);
                    
                    int res = DateTime.Compare(d,DateTime.Now);
                    if(res<0)
                    {
                        db.Movies.Remove(mv);
                        db.SaveChanges();
                        Database1Entities1 dbe1 = new Database1Entities1();
                        /*BookSeat b = (BookSeat)dbe1.BookSeats.Where(x => x.Id == mv.Id);
                        dbe1.BookSeats.Remove(b);
                        dbe1.SaveChanges();*/
                    }
                }
            }
            string cat = form["Categories"];
            ViewBag.Cat = cat;
            Database1Entities dbe = new Database1Entities();
            

            //form["moviedaytime"]= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T');
            if (Session["DT"]!=null)
            {
                form["moviedaytime"] = ((DateTime)Session["DT"]).ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T');
            }
            string dt = form["moviedaytime"];
            DateTime datetime ;
            ViewBag.Date = null;
            ViewBag.Time = null;
            if (dt != "" && dt!=null)
            {
                dt=dt.Replace('T', ' ');
                datetime = Convert.ToDateTime(dt);
                Session["DT"] = datetime;
                ViewBag.Date = datetime.Date.ToString();
                ViewBag.Time = datetime.TimeOfDay.ToString();
            }
            string ord= form["order"];
            var emps = dbe.Movies;
            if(ord=="1")
            {
                var emps1 = from e in dbe.Movies
                           orderby e.price * (1 - (e.Discount / 100))
                            select e;
                return View(emps1.ToList());
            }
            if(ord=="2")
            {
                var emps1 = from e in dbe.Movies
                           orderby e.price*(1-(e.Discount/100)) descending
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