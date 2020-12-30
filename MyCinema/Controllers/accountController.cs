using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using MyCinema.Models;

namespace MyCinema.Controllers
{
    public class accountController : Controller
    {
        readonly SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
        [HttpPost]
        public ActionResult Delete(int id)
        {
            SqlCommand cmd = new SqlCommand(@"DELETE FROM [dbo].[Movies] WHERE [Id]='" + id + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return View("AdminPage");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Database1Entities dbe = new Database1Entities();
            Movy m = dbe.Movies.Find(id);
            if (m == null)
            {
                return HttpNotFound();
            }
            return View(m);
        }
        
        [HttpPost]
        public ActionResult Edit(Movy m)
        {
            if (m.movie_hall == "A1" || m.movie_hall == "A2" || m.movie_hall == "A3" || m.movie_hall == "B1" || m.movie_hall == "B2")
             {
                string dat = "update [Movies] set movie_date='" + m.movie_date+ "',movie_time='"+m.movie_time+ "',movie_hall= '" + m.movie_hall + "',price= '" + m.price + "' where Id='" + m.Id + "'";
                SqlCommand comm = new SqlCommand(dat, con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                ViewBag.SuccessMessage = "Movie Updated successfully.";
                 return View("AdminPage");

             }
             else if(m.movie_hall!=null)
             {
                 ViewBag.DuplicateMessage = "hall number Should be A1,A2,A3,B1,B2";
                 return View();
             }
            return View();

        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult UserPage()
        {
            return View();
        }
        public ActionResult AdminPage()
        {
            return View();
        }
        public ActionResult UsersMoviesList()
        {
            Database1Entities dbe = new Database1Entities();
            return View(dbe.Movies.ToList());
        }
        public ActionResult AdminMoviesList()
        {
            Database1Entities dbe = new Database1Entities();
            return View(dbe.Movies.ToList());
        }

        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            string check = " select count(*) from [User] where username ='" + acc.username + "'and password= '" + acc.password + "' ";
            SqlCommand com = new SqlCommand(check, con);
            con.Open();
            int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
            con.Close();

            if (temp == 1)
            {
               if (acc.username == "admin")
                    return View("AdminPage");
                else
                    return View("UserPage");
            }
            else
            {
                ViewBag.DuplicateMessage = "Wrong Username or password.";
                return View("Login");
            }
        }
        



    }
}