using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCinema.Models;
using System.Data.SqlClient;
using System.Data;

namespace MyCinema.Controllers
{
    public class AdminController : Controller
    {
        readonly SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
        // GET: Admin
        public ActionResult AddMovie()
        {
            
            return View();
        }
        
        [HttpPost]
        public ActionResult AddMovie(HttpPostedFileBase file,Movie m)
        {
            m.Discount = 0;
            if (DateTime.Compare(m.Date, DateTime.Today) > 0)
            {
                if (m.hall == "A1" || m.hall == "A2" || m.hall == "A3" || m.hall == "B1" || m.hall == "B2")
                {
                    string check = " select count(*) from [Movies] where movie_date ='" + m.Date + "'and movie_time='" + m.Time + "' and movie_hall='" + m.hall + "'";
                    SqlCommand com = new SqlCommand(check, con);
                    con.Open();
                    int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                    con.Close();
                    if (temp != 1)
                    {
                        var path = "";
                        if (file != null)
                        {
                            if (file.ContentLength > 0)
                            {
                                if (Path.GetExtension(file.FileName).ToLower() == ".jpg" || Path.GetExtension(file.FileName).ToLower() == ".png" || Path.GetExtension(file.FileName).ToLower() == ".jpeg")
                                {
                                    path = Path.Combine(Server.MapPath("~/Images"), file.FileName);
                                    file.SaveAs(path);
                                    string dat = "Insert into [Movies](movie_name,movie_dis,movie_date,movie_time,movie_hall,price,movie_pic,Category,Age_limit,Discount,last) Values('" + m.Name + "','" + m.Description + "','" + m.Date + "','" + m.Time + "','" + m.hall + "','" + m.price + "','" + file.FileName + "','" + m.Category + "','" + m.age + "','" + m.Discount + "','" + (m.price-m.price) + "')";
                                    SqlCommand comm = new SqlCommand(dat, con);
                                    con.Open();
                                    comm.ExecuteNonQuery();
                                    con.Close();
                                    ViewBag.SuccessMessage = "Movie added successfully.";

                                }
                            }
                        }
                        return RedirectToAction("AdminMoviesList", "Account");
                        // return View();
                    }
                    else
                    {
                        ViewBag.DuplicateMessage = "Change date Please , Another Movie in that date";
                        return View();
                    }
                }
                else
                {
                    ViewBag.DuplicateMessage = "hall number invalid please choose one of these halls: A1,A2,A3,B1,B2";
                    return View();
                }
            }
            else
            {
                ViewBag.DuplicateMessage = "Change date please ";
                return View();
            }
        }
    }
}
