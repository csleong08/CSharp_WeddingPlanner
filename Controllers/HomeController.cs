using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WeddingPlannerContext _context;
    
        public HomeController(WeddingPlannerContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index(Users myUser)
        {
            return View("Index");
        }
        [HttpPost("AddUser")]
        public IActionResult AddUser(UsersValidate uservalidator)
        {
            if(ModelState.IsValid)
            {

                var emailvalidation = _context.users.SingleOrDefault(p => p.email == uservalidator.email);
                if(emailvalidation == null)
                {
                    PasswordHasher<UsersValidate> Hasher = new PasswordHasher<UsersValidate>();
                    uservalidator.password = Hasher.HashPassword(uservalidator, uservalidator.password);
                    Users myUser = new Users();
                    myUser.first_name = uservalidator.first_name;
                    myUser.last_name = uservalidator.last_name;
                    myUser.email = uservalidator.email;
                    myUser.password = uservalidator.password;
                    myUser.created_at = DateTime.Now;
                    myUser.updated_at = DateTime.Now;
                    _context.Add(myUser);
                    _context.SaveChanges();

                    HttpContext.Session.SetInt32("UserID", myUser.id);
                    int? UserID = HttpContext.Session.GetInt32("UserID");
                    ViewBag.UserID = UserID;
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    TempData["uniqueemail"] = "This email belongs to a registered user. Please use another email address";
                    return View("Index");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("LoginProcess")]
        public IActionResult LoginProcess(LoginValidate myLogin)
        {
        if(ModelState.IsValid)
            {
                Users loginData = _context.users.SingleOrDefault(p => p.email == myLogin.login_email);
                if(loginData == null)
                {
                    ModelState.AddModelError("login_email", "Email Address is not registered");
                }
                else if(loginData != null && myLogin.login_password != null)
                {
                    var Hasher = new PasswordHasher<Users>();
                    // Pass the user object, the hashed password, and the PasswordToCheck
                    if(0 != Hasher.VerifyHashedPassword(loginData, loginData.password, myLogin.login_password))
                    {
                        HttpContext.Session.SetInt32("UserID", loginData.id);
                        int? UserID = HttpContext.Session.GetInt32("UserID");
                        ViewBag.UserID = UserID;
                        return RedirectToAction("Dashboard");
                    }
                }
                return View("Index");
            }
            // ViewBag.error = "LOL, Nice try!";
            // TempData["error"] = "LOL, try again!";
            return View("Index");
        }
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            ViewBag.userInfo = _context.users.Where(p => p.id == UserID).SingleOrDefault();
            var allWeddings = _context.weddings.Include(p => p.Reservation).ToList();
            ViewBag.allWeddings = allWeddings;
            //  Bonus: Weddings expire when the scheduled date passes, and are removed from the database.
            foreach (var item in allWeddings)
            {
                if(item.date < DateTime.Today)
                {
                    foreach (var rsvp in item.Reservation)
                    {
                        _context.reservations.Remove(rsvp);
                    }
                    _context.weddings.Remove(item);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
            }
            return View("Dashboard");
        }
        [HttpPost("AddWedding")]
        public IActionResult AddWedding(WeddingsValidate weddingvalidator)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            if(ModelState.IsValid)
            {
                Weddings myWedding = new Weddings();
                myWedding.wedder_one = weddingvalidator.wedder_one;
                myWedding.wedder_two = weddingvalidator.wedder_two;
                myWedding.date = weddingvalidator.date;
                myWedding.created_at = DateTime.Now;
                myWedding.updated_at = DateTime.Now;
                myWedding.usersid = (int)UserID;
                myWedding.address = weddingvalidator.address;
                if(myWedding.date < DateTime.Today)
                {
                    ModelState.AddModelError("date", "Wedding creation has to be in the future, nice try!");
                    // ViewBag.error = "LOL, Nice try!";
                    return View("PlanWedding");
                };
                _context.Add(myWedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("PlanWedding");
            }
        }
        [HttpGet("PlanWedding")]
        public IActionResult PlanWedding()
        {
            return View("PlanWedding");
        }
        [HttpGet("/Wedding/{weddingid}")]
        public IActionResult Wedding(int weddingid)
        {
            ViewBag.weddingInfo = _context.weddings.Where(p => p.id == weddingid).Include(p => p.Reservation)
            .ThenInclude(p => p.Users).FirstOrDefault();
            return View("Wedding");
        }
        [HttpPost("DeleteWedding")]
        public IActionResult DeleteWedding(int weddingID)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            var DelReservation = _context.reservations.Where(p => p.weddingsid == weddingID).ToList();
            var DelWedding = _context.weddings.Where(p => p.id == weddingID).SingleOrDefault();
            foreach (var rsvp in DelReservation)
            {
                _context.reservations.Remove(rsvp);
            }
            _context.weddings.Remove(DelWedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpPost("DeleteRSVP")]
        public IActionResult DeleteRSVP(int weddingID)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            var DelReservation = _context.reservations.Where(p => p.weddingsid == weddingID).Where(p => p.usersid == UserID).SingleOrDefault();
            _context.reservations.Remove(DelReservation);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpPost("AddRSVP")]
        public IActionResult AddRSVP(int weddingID)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            // var DelReservation = _context.reservations.Where(p => p.weddingsid == weddingID).ToList();
            // var DelReservation = _context.reservations.Where(p => p.id == reservationID).SingleOrDefault();
            Reservations myReservation = new Reservations();
            myReservation.created_at = DateTime.Now;
            myReservation.updated_at = DateTime.Now;
            myReservation.usersid = (int)UserID;
            myReservation.weddingsid = weddingID;
            _context.reservations.Add(myReservation);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
