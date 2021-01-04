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
            //vm.Amount = item.price;
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
            TempData["Amount"] =vm.Amount+ "  shekels will be deducted from your card";
            TempData["Price"] = "         Movie price = " + vm.Amount;
            TempData["choosen"] = " Enter [ " + id + " ] to see the choosen seaets";
            TempData["Seatno"] = "1 to " + seats_num;
            return View(vm);
        }
        
        [HttpPost]
        public ActionResult AddBooking(BookSeat vm)
        {

            string seatno = vm.seatno;
            string moviename = vm.movieName;
            string moviedate = vm.moviedate;
            string movietime = vm.movietime;
            string movieId = vm.movieId;
            int seats_num = -1;
            string seats_num2 = "0";
            int booked = -1;
            if (vm.MyHall == "A2" || vm.MyHall == "A1")
            { seats_num = 50; seats_num2 = "50"; }
            else
                if (vm.MyHall == "A3" || vm.MyHall == "B1")
            { seats_num = 40; seats_num2 = "40"; }
            else
                if (vm.MyHall == "B2")
            { seats_num = 30; seats_num2 = "30"; }
            if (seatno == "1") booked = 1; else if(seatno == "9") booked =9; else if (seatno == "17") booked = 17; else if (seatno == "25") booked = 25; else if(seatno == "40") booked = 40; else if (seatno == "48") booked = 48;
            if (seatno == "2") booked = 2; else if (seatno == "10") booked = 10; else if(seatno == "18") booked = 18; else if (seatno == "26") booked = 26; else if(seatno == "39") booked = 39; else if (seatno == "47") booked = 47;
            if (seatno == "3") booked = 3; else if (seatno == "11") booked = 11; else if(seatno == "19") booked = 19; else if (seatno == "27") booked = 27; else if(seatno == "38") booked = 38; else if (seatno == "46") booked = 46;
            if (seatno == "4") booked = 4; else if (seatno == "12") booked = 12; else  if (seatno == "20") booked = 20; else if (seatno == "28") booked = 28; else if(seatno == "37") booked = 37; else if (seatno == "45") booked = 45;
            if (seatno == "5") booked = 5; else if (seatno == "13") booked = 13; else if(seatno == "21") booked = 21; else if (seatno == "29") booked = 29; else if(seatno == "36") booked = 36; else if (seatno == "44") booked = 44;
            if (seatno == "6") booked = 6; else if (seatno == "14") booked = 14; else if(seatno == "22") booked = 22; else if (seatno == "30") booked = 30; else if(seatno == "35") booked = 35; else if (seatno == "43") booked = 43;
            if (seatno == "7") booked = 7; else if (seatno == "15") booked = 15; else if(seatno == "23") booked = 23; else if (seatno == "31") booked = 31; else if(seatno == "34") booked = 34; else if (seatno == "42") booked = 42;
            if (seatno == "8") booked = 8; else if (seatno == "16") booked = 16; else if(seatno == "24") booked = 24; else if (seatno == "32") booked = 32; else if(seatno == "33") booked = 33; else if (seatno == "41") booked = 41;
            if (seatno == "49") booked = 49; else if (seatno == "50") booked = 50;
            if (booked >= 1 && booked <= seats_num)
            {
                if (checkSeat(movieId, seatno))
                {
                    string dat = "Insert into [BookSeat](movieName,moviedate,movietime,seatno,Full_Name,movieId) Values('" + moviename + "','" + moviedate + "','" + movietime + "','" + seatno + "','" + vm.Full_Name + "','" + movieId + "')";
                    SqlCommand comm = new SqlCommand(dat, con);
                    con.Open();
                    comm.ExecuteNonQuery();
                    con.Close();
                    return View("Payment");
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
        private bool checkSeat(string movieId ,string seatno)
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
            TempData["Sucess"] = "You booked a seat , Please enter payment details ";
            
            return View();
        }
        [HttpPost]
        public ActionResult PaymentDone()
        {
            return View();
        }
    }
}