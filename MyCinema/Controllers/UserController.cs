using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCinema.Models;
using System.Data.SqlClient;
namespace MyCinema.Controllers
{
    public class UserController : Controller
    {
        readonly SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");

        [HttpGet]
        public ActionResult AddOrEdit(int id=0)
        {
            User user = new User();
            return View(user);
        }
        [HttpPost]

        public ActionResult AddOrEdit(User newuser)
        {
            string check = " select count(*) from [User] where username ='" + newuser.username + "'";
            SqlCommand com = new SqlCommand(check, con);
            con.Open();
            int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
            con.Close();
            if (temp != 1)
            {
                string dat = "Insert into [User](username,password) Values('" + newuser.username + "','" + newuser.password + "')";
                SqlCommand comm = new SqlCommand(dat, con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                ViewBag.SuccessMessage = "Registeration Successful.";
                return View("AddOrEdit", new User());
            }
            else
            {
                ViewBag.DuplicateMessage = "Username already exist.";
                return View("AddOrEdit", newuser);
            }
            
        }

        

    }
}