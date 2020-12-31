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
            if (checkSeat(movieId, seatno))
            {
                string dat = "Insert into [BookSeat](movieName,moviedate,movietime,seatno,Full_Name,movieId) Values('" + moviename+ "','" + moviedate+ "','" + movietime + "','" + seatno + "','" + vm.Full_Name + "','" + movieId + "')";
                SqlCommand comm = new SqlCommand(dat, con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                TempData["Sucess"] = "Seat booked ,check card";


            }
            else
            {
                ViewBag.DuplicateMessage = "Seat already taken please choose another one ";
            }
            return View("Booking");
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
        /*check*/
        /*
        public ActionResult SBooking(int id=0)
        {
            Database1Entities dbe = new Database1Entities();
            var item = dbe.Movies.Where(a => a.Id == id).FirstOrDefault();
            BookNowViewModel vm = new BookNowViewModel();
            vm.Movie_Name = item.movie_name;
            vm.hall = item.movie_hall;
            vm.date = item.movie_date;
            vm.time = item.movie_time;
            vm.Amount = item.price;
            vm.MovieId = id;
            return View(vm);
        }
        [HttpPost]
        public ActionResult AddSBooking(BookNowViewModel vm)
        {
            Database1Entities dbe = new Database1Entities();
            List<BookingTable> booking = new List<BookingTable>();
            List<Cart> carts = new List<Cart>();
            string seatno = vm.SeatNo;
            int movieId = vm.MovieId;
            string[] seatnoarray = seatno.Split(',');
            count = seatnoarray.Length;
            if (checkSeat2(seatno, movieId) == false)
            {
                foreach (var item in seatnoarray)
                {
                    carts.Add(new Cart { seatno = vm.SeatNo, UserId = vm.User_Name, date = vm.date, time = vm.time, Amount = vm.Amount, MovieId = vm.MovieId });
                }
                foreach (var item in carts)
                {
                    dbe.Cart.Add(item);
                    dbe.SaveChanges();
                }
                TempData["Sucess"] = "Seat booked ,check card";


            }
            else
                TempData["seatnomsg"] = "seat already booked please change your seat number ";

            return View("SBooking");

        }
        private bool checkSeat2(string seatno,int movieId)
        {
            Database1Entities dbe = new Database1Entities();
            string seats = seatno;
            string[] seatreserve = seats.Split(',');
            var seatlist = dbe.BookingTable.Where(a => a.MovieDetailsId == movieId).ToList();
            
            foreach (var item in seatlist)
            {
                string alreadybooked = item.seatno;
                foreach (var item1 in seatreserve)
                {
                    if (item1 == alreadybooked)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag == false)
            {
                return true;
            }
            else
                return false;
        }*/
    }
}