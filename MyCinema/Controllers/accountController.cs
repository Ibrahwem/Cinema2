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
                string dat = "update [Movies] set movie_date='" + m.movie_date+ "',movie_time='"+m.movie_time+ "',movie_hall= '" + m.movie_hall + "',price= '" + m.price*((100-m.Discount)/100) + "',Discount= '" + m.Discount + "' where Id='" + m.Id + "'";
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
        
        public ActionResult Booking(int id)
        {
            Database1Entities dbe = new Database1Entities();
            var item = dbe.Movies.Where(a => a.Id == id).FirstOrDefault();
            BookSeat vm = new BookSeat();
            vm.movieId = id.ToString();
            vm.movieName = item.movie_name;
            vm.moviedate = item.movie_date;
            vm.movietime = item.movie_time;
            vm.MyHall = item.movie_hall;
            vm.Amount = item.price;
            string seats_num="0";
            if (vm.MyHall == "A2" || vm.MyHall == "A1")
                seats_num = "50";
            else
                if (vm.MyHall == "A3" || vm.MyHall == "B1")
                seats_num = "40";
            else
                if (vm.MyHall == "B2")
                seats_num = "30";

            TempData["Hall"] = "The hall that the movie will played in is [ " + vm.MyHall + " ] witch contains seats from 1 to " + seats_num ;
          //  TempData["Amount"] =vm.Amount+ "  shekels will be deducted from your card";
           // TempData["Price"] = "         Movie price = " + vm.Amount;
            TempData["choosen"] = " Enter [ " + id + " ] to see the choosen seaets";
            TempData["Seatno"] = "1 to " + seats_num;
            return View(vm);
        }
        
        [HttpPost]
        public ActionResult AddBooking(BookSeat vm)
        {

            int seatno = vm.seatno;
            string moviename = vm.movieName;
            string moviedate = vm.moviedate;
            string movietime = vm.movietime;
            string movieId = vm.movieId;
            int seats_num = -1;
            string seats_num2 = "0";
            
            if (vm.MyHall == "A2" || vm.MyHall == "A1")
            { seats_num = 50; seats_num2 = "50"; }
            else
                if (vm.MyHall == "A3" || vm.MyHall == "B1")
            { seats_num = 40; seats_num2 = "40"; }
            else
                if (vm.MyHall == "B2")
            { seats_num = 30; seats_num2 = "30"; }
            if (seatno >= 1 && seatno <= seats_num)
            {
                if (checkSeat(movieId, seatno))
                {
                    string dat = "Insert into [BookSeat](movieName,moviedate,movietime,seatno,Full_Name,movieId,MyHall) Values('" + moviename + "','" + moviedate + "','" + movietime + "','" + seatno + "','" + vm.Full_Name + "','" + movieId + "','"+vm.MyHall+"')";
                    SqlCommand comm = new SqlCommand(dat, con);
                    con.Open();
                    comm.ExecuteNonQuery();
                    con.Close();
                    string datt = "Insert into [cart](Amount,date,time,seatno,UserId,movieId) Values('" + vm.Amount + "','" + moviedate + "','" + movietime + "','" + seatno + "','" + vm.Full_Name + "','" + movieId + "')";
                    SqlCommand commm = new SqlCommand(datt, con);
                    con.Open();
                    commm.ExecuteNonQuery();
                    con.Close();
                    TempData["GoodSeatBook"] = "Successful Book! Check your Cart";
                    return View("Booking");
                }
                else
                {
                    ViewBag.DuplicateMessage = "Seat already taken please choose another one ";
                    return View("Booking");
                }
            }
            else
            {
                ViewBag.DuplicateMessage = "Seats Number from 1 to " + seats_num2;
                return View("Booking");
            }
            
        }
        private bool checkSeat(string movieId ,int  seatno)
        {
            string check = " select count(*) from [BookSeat] where movieId ='" + movieId+ "' and seatno='" + seatno+"'";
            SqlCommand com = new SqlCommand(check, con);
            con.Open();
            int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
            con.Close();
            if (temp != 1)
            {
                return true;
            }
            else 
                return false;
        }

        public ActionResult ChoosenSeats(string searching)
        {

            Database1Entities1 db = new Database1Entities1();
            return View(db.BookSeats.Where(x =>x.movieId.Contains(searching)||searching==null).ToList());
                
        }

        public ActionResult Payment()
        {
            TempData["Sucess"] = "Please enter payment details ";
            
            return View();
        }
        [HttpPost]
        public ActionResult PaymentDone()
        {
            return View();
        }
        public ActionResult CheckMyCart(string searching)
        {
            Database1Entities dbe = new Database1Entities();
            return View(dbe.carts.Where(x => x.UserId.Contains(searching)).ToList());
        }
    }
}