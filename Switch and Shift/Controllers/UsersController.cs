using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Switch_and_Shift.Models;
using Microsoft.AspNetCore.Identity;
using Switch_and_Shift.Interface;
using Switch_and_Shift.Repository;

namespace Switch_and_Shift.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IUserRepository userRepository;
        

        public object Server { get; private set; }

        public UsersController(ApplicationDbContext _db,IUserRepository userRepository)
        {
            db = _db;
            this.userRepository = userRepository;
        }

        int user_id;
        string password;
        

       

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(Users user)
        {
            
            var existingUser = await userRepository.GetUserByEmailAsync(user.Email);

            if (existingUser != null)
            {
                ModelState.Clear();
                ViewBag.Notification = "An account with this email already exists";
                return View();  
            }
            else
            {
               
                user.report_show = 1;

                await userRepository.AddUserAsync(user);

                ModelState.Clear();
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Email", user.Email.ToString());

                
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpPost]
        public ActionResult SignUpToRedirect()
        {            
            return RedirectToAction("SignUp", "Users");
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            string email = HttpContext.Session.GetString("Email");
            if (email!=null)
            {
                return RedirectToAction("Welcome", "Home");
            }
            return View();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> LogIn(Users user)
        {
            var checklogin =  await userRepository.GetUserByEmailAsync(user.Email);
            // Assuming both are strings
            if (checklogin != null && checklogin.Password.Equals(user.Password) && checklogin.report_show == 1)
            {
                HttpContext.Session.SetString("Email", checklogin.Email);
                HttpContext.Session.SetString("FirstName", checklogin.FirstName);
                HttpContext.Session.SetString("UserId", checklogin.UserId.ToString());
                user_id = checklogin.UserId;
                password = checklogin.Password;
                ModelState.Clear();
                return RedirectToAction("Welcome", "Home");
            }
            else if (checklogin == null)
            {
                ModelState.Clear();
                ViewBag.Notification = "Wrong email or password";
            }
            else
            {
                ModelState.Clear();
                ViewBag.Notification = "You have been BANNED!!!";
            }
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("Email", null);
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("FirstName");
            HttpContext.Session.Remove("UserId");
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> ViewProfile()
        {
            if(HttpContext.Session.GetString("Email")!=null)
            {
                string email = HttpContext.Session.GetString("Email");       
                var userdetails = await userRepository.GetUserByEmailAsync(email);
                return View(userdetails);
            }
            else
            {
                return View();
            }         
        }

        [HttpGet]
        public async Task<ActionResult> UserReview(int? id)
        {
            string email = HttpContext.Session.GetString("Email");
            var reviewee = await userRepository.GetUserByIdAsync(id.Value);
            var reviewer =await userRepository.GetUserByEmailAsync(email);
            ViewBag.Name = reviewee.FirstName + " " + reviewee.LastName;
            ViewBag.Email = reviewee.Email;
            ViewBag.ReviewerName = reviewer.FirstName + " " + reviewer.LastName;
            ViewBag.ReviewerEmail = reviewer.Email;
            ViewBag.Post_date = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            return View();
        }

        [HttpPost]
        public ActionResult UserReview(UserReview userReview)
        {
            db.UserReview.Add(userReview);
            db.SaveChanges();
            return RedirectToAction("Welcome", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> ShowReview(int? id)
        {
           
            var reviewee =await  userRepository.GetUserByIdAsync(id.Value);
            var review = await userRepository.GetUserByEmailAsync(reviewee.Email);
            return View(db.UserReview.Where(x => x.reviewee_email == reviewee.Email).ToList());            
        }

        public ActionResult Noreview()
        {
            return View();
        }




        // GET: USERS
        public async Task<IActionResult> Index()
        {
            return View(await userRepository.GetAllUsersAsync());
        }

        // GET: USERS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var User = await userRepository.GetUserByIdAsync(id.Value);
            if (User == null)
            {
                return NotFound();
            }

            return View(User);
        }

        // GET: USERS/Create
        /*public IActionResult Create()
        {
            return View();
        }

        // POST: USERS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,GamingName,Location,Email,Phone,Password")] USERS uSERS)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uSERS);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uSERS);
        }*/
    
       
        //GET: USERS/Edit/5
        public ActionResult Edit()
        {
            string userMail = HttpContext.Session.GetString("Email");
            var userDetails = userRepository.GetUserByEmailAsync(userMail);
            return View(userDetails);
        }

        // POST: USERS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Users users)
        {
            if (HttpContext.Session.GetString("Email")==null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("Email")!=null)
            {
                    string email = HttpContext.Session.GetString("Email");
                    var checklogin = await userRepository.GetUserByEmailAsync(email);
                   
                
                    checklogin.FirstName = users.FirstName;
                    checklogin.LastName = users.LastName;
                    checklogin.District = users.District;
                    checklogin.Location = users.Location;
                    checklogin.Email = users.Email;
                    checklogin.Phone = users.Phone;
                    checklogin.Password = checklogin.Password;
                   
                    await userRepository.UpdateUserAsync(checklogin);
                    return RedirectToAction(nameof(ViewProfile));
                
               
            }
            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgetPassword(Users Users)
        {
            if (Users.Email != null)
            {
                string email = Users.Email;
                var checklogin = await userRepository.GetUserByEmailAsync(email);
              

                checklogin.FirstName = checklogin.FirstName;
                checklogin.LastName = checklogin.LastName;
                checklogin.District = checklogin.District;
                checklogin.Location = checklogin.Location;
                checklogin.Email = checklogin.Email;
                checklogin.Phone = checklogin.Phone;
                checklogin.Password = Users.Password;
 
                await userRepository.UpdateUserAsync(checklogin);
                return RedirectToAction(nameof(LogIn));


            }
            return View();
        }


        /*// GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Users = await _context.Users.FindAsync(id);
            _context.Users.Remove(Users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private async Task<bool> UsersExists(int id)
        {
            return await userRepository.UserExistsAsync(id);
           
        }
    }

    public class HttpPostedFileBase
    {
        public int ContentLength { get; internal set; }

        internal void SaveAs(string path)
        {
            throw new NotImplementedException();
        }
    }
}
