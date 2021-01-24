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
          //  return View("AdminPage");
            return RedirectToAction("AdminMoviesList", "Account");

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

        public ActionResult DeleteSeat(int id,int mid ,int sno,string uid)
        {
            BookSeat vmm = new BookSeat();
            vmm.Full_Name = uid;
            SqlCommand cmd = new SqlCommand(@"DELETE FROM [dbo].[cart] WHERE [Id]='" + id + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            cmd = new SqlCommand(@"DELETE FROM [dbo].[BookSeat] WHERE [seatno]='" + sno + "' and [movieId]='" + mid + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("CheckMyCart",vmm);
        }
        //[HttpPost]
        //public ActionResult EditSeat(cart c)
        //{
        //    string dat = "update [cart] set seatno='" + c.seatno + "'where Id='" + c.Id + "'";
        //    SqlCommand comm = new SqlCommand(dat, con);
        //    con.Open();
        //    comm.ExecuteNonQuery();
        //    con.Close();
        //  /*  ViewBag.SuccessMessage = "Movie Updated successfully.";
        //    dat = "update [BookSeat] set seatno='" + c.seatno + "'where Id='" + c.Id + "'";
        //    comm = new SqlCommand(dat, con);
        //    con.Open();
        //    comm.ExecuteNonQuery();
        //    con.Close();
        //    ViewBag.SuccessMessage = "Movie Updated successfully.";*/
        //    return View();

        //}
        [HttpPost]
        public ActionResult Edit(Movy m)
        {
            
            if (m.movie_hall == "A1" || m.movie_hall == "A2" || m.movie_hall == "A3" || m.movie_hall == "B1" || m.movie_hall == "B2")
             {
                string dat = "update [Movies] set movie_date='" + m.movie_date+ "',movie_time='"+m.movie_time+ "',movie_hall= '" + m.movie_hall + "',price= '" + m.price + "',Discount= '" + m.Discount + "' where Id='" + m.Id + "'";
                SqlCommand comm = new SqlCommand(dat, con);
                con.Open();
                comm.ExecuteNonQuery();
                con.Close();
                ViewBag.SuccessMessage = "Movie Updated successfully.";
                // return View("AdminPage");
                return RedirectToAction("AdminMoviesList", "Account");


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
        public ActionResult UsersMoviesList(FormCollection form)
        {
            using (Database1Entities db = new Database1Entities())
            {
                foreach (Movy mv in db.Movies.ToList<Movy>())
                {
                    /*System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString());
                    System.Diagnostics.Debug.WriteLine(mv.movie_date.ToString() + " " + mv.movie_time.ToString());*/
                    DateTime d = mv.movie_date.Date + TimeSpan.Parse(mv.movie_time);

                    int res = DateTime.Compare(d, DateTime.Now);
                    if (res < 0)
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
            if (Session["DT"] != null)
            {
                form["moviedaytime"] = ((DateTime)Session["DT"]).ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T');
            }
            string dt = form["moviedaytime"];
            DateTime datetime;
            ViewBag.Date = null;
            ViewBag.Time = null;
            if (dt != "" && dt != null)
            {
                dt = dt.Replace('T', ' ');
                datetime = Convert.ToDateTime(dt);
                Session["DT"] = datetime;
                ViewBag.Date = datetime.Date.ToString();
                ViewBag.Time = datetime.TimeOfDay.ToString();
            }
            string ord = form["order"];
            var emps = dbe.Movies;
            if (ord == "1")
            {
                var emps1 = from e in dbe.Movies
                            orderby e.price
                            select e;
                return View(emps1.ToList());
            }
            if (ord == "2")
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
                    return RedirectToAction("AdminMoviesList", "Account");
                else
                    return RedirectToAction("UsersMoviesList", "Account");
            }
            else
            {
                ViewBag.DuplicateMessage = "Wrong Username or password.";
                return View("Login");
            }
        }
        
        public ActionResult Booking(int id)
        {
            Database1Entities1 db = new Database1Entities1();
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

            Session["Hall"] = "The hall that the movie will played in is [ " + vm.MyHall + " ] witch contains seats from 1 to " + seats_num ;
            //  TempData["Amount"] =vm.Amount+ "  shekels will be deducted from your card";
            // TempData["Price"] = "         Movie price = " + vm.Amount;
            Session["choosen"] = " Enter [ " + id + " ] to see the choosen seaets";
            Session["Seatno"] = "1 to " + seats_num;
            var tuple1 =new Tuple<BookSeat,List<BookSeat>> (vm, db.BookSeats.Where(x => x.movieId.Contains(id.ToString())).ToList());
            return View(tuple1);
        }
        
        [HttpPost]
        public ActionResult AddBooking(BookSeat vm)
        {

            int seatno = vm.seatno;
            string moviename = vm.movieName;
            DateTime moviedate = vm.moviedate;
            string movietime = vm.movietime;
            string movieId = vm.movieId;
            int seats_num = -1;
            string seats_num2 = "0";
            Database1Entities1 db = new Database1Entities1();
            var tuple1 = new Tuple<BookSeat, List<BookSeat>>(vm, db.BookSeats.Where(x => vm.movieId.Contains(movieId.ToString())).ToList());

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
                    ViewBag.SuccessMessage = "Successful Book! Check your Cart";
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
        [HttpPost]
        public ActionResult PayNow(BookSeat vm)
        {

            int seatno = vm.seatno;
            string moviename = vm.movieName;
            DateTime moviedate = vm.moviedate;
            string movietime = vm.movietime;
            string movieId = vm.movieId;
            int seats_num = -1;
            string seats_num2 = "0";
            Database1Entities1 db = new Database1Entities1();
            var tuple1 = new Tuple<BookSeat, List<BookSeat>>(vm, db.BookSeats.Where(x => vm.movieId.Contains(movieId.ToString())).ToList());

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
                    string dat = "Insert into [BookSeat](movieName,moviedate,movietime,seatno,Full_Name,movieId,MyHall) Values('" + moviename + "','" + moviedate + "','" + movietime + "','" + seatno + "','" + vm.Full_Name + "','" + movieId + "','" + vm.MyHall + "')";
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
            return View(db.BookSeats.Where(x => x.Full_Name.Contains(searching)).ToList());


        }

        public ActionResult Payment(string id)
        {
            TempData["Sucess"] = "Please enter payment details ";
            if (id != null)
            {
                SqlCommand cmd = new SqlCommand(@"DELETE FROM [dbo].[cart] WHERE [UserId]='" + id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return View();
            }
            else
                return View();
        }
        [HttpPost]
        public ActionResult PaymentDone()
        {
            return RedirectToAction("Index", "Home");
        }
        public ActionResult CheckMyCart(BookSeat vm)
        {
            Database1Entities dbe = new Database1Entities();
            return View(dbe.carts.Where(x => x.UserId.Contains(vm.Full_Name)).ToList());
        }
    }
}