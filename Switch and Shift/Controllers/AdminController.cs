using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Switch_and_Shift.Interface;
using Switch_and_Shift.Models;

namespace Switch_and_Shift.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ApplicationDbContext db;

        public AdminController(IAdminRepository _adminRepository, ApplicationDbContext db) 
        {
              this._adminRepository = _adminRepository;
            this.db = db;
        }

        [HttpGet]
        public ActionResult AdminLogIn()
        {
            string email = HttpContext.Session.GetString("Admin_Email");
            if(email!=null)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AdminLogIn(Admin admin)
        {
            var checkLogin = await  _adminRepository.GetAdminByEmailAndPasswordAsync(admin.Admin_Email, admin.Admin_Password);

            if (checkLogin != null)
            {
                HttpContext.Session.SetString("Admin_Email", checkLogin.Admin_Email);
                HttpContext.Session.SetString("Admin_Password", checkLogin.Admin_Password);
                ModelState.Clear();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.Clear();
                ViewBag.Notification = "Wrong username or password";
            }
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> ShowReview(int? id)
        {
            string email = HttpContext.Session.GetString("Admin_Email");
            if (email == null)
            {
                return RedirectToAction("AdminLogIn", "Admin");
            }
            var reviewee = await _adminRepository.GetUserByIdAsync(id);
            var models = db.UserReview.Where(x => x.reviewee_email == reviewee.Email).ToList();
            return View(models);
        
        }

        [HttpGet]
        public async Task<ActionResult> Ban(int? id)
        {
            string email = HttpContext.Session.GetString("Admin_Email");
            if (email == null)
            {
                return RedirectToAction("AdminLogIn", "Admin");
            }

            var Reports_User =await  _adminRepository.GetUserByIdAsync(id);
            if (Reports_User != null)
            {
                Reports_User.report_show = 0;
                await _adminRepository.UpdateUserAsync(Reports_User);
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public async Task<ActionResult> Unban(int? id)
        {
            string email = HttpContext.Session.GetString("Admin_Email");
            if (email == null)
            {
                return RedirectToAction("AdminLogIn", "Admin");
            }

            var Reports_User = await _adminRepository.GetUserByIdAsync(id);
            if (Reports_User != null)
            {
                Reports_User.report_show = 1;
                await _adminRepository.UpdateUserAsync(Reports_User);
            }
            return RedirectToAction("Index", "Admin");
        }


        public IActionResult Index()
        {
            string email = HttpContext.Session.GetString("Admin_Email");
            if (email == null)
            {
                return RedirectToAction("AdminLogIn", "Admin");
            }

            var displaydata = _adminRepository.GetAllUsersAsync();
            return View(displaydata);
        }

   

        [HttpGet]
        //GET: ADMINs
        public async Task<IActionResult> Index(string UserSearch)
         {
            string email = HttpContext.Session.GetString("Admin_Email");
            if (email == null)
            {
                return RedirectToAction("AdminLogIn", "Admin");
            }
            ViewData["GetUserDetails"] = UserSearch;
            var users = await _adminRepository.GetAllUsersAsync();
            if (!String.IsNullOrEmpty(UserSearch))
            {
                users = users.Where(u => u.Email.Contains(UserSearch)).ToList();
            }
            return View(users);
             
        }

         // GET: ADMINs/Details/5
         public async Task<IActionResult> Details(int? id)
         {
            string email = HttpContext.Session.GetString("Admin_Email");
            if (email == null)
            {
                return RedirectToAction("AdminLogIn", "Admin");
            }
            if (id == null)
             {
                 return NotFound();
             }

            var User = await _adminRepository.GetUserByIdAsync(id);
             if (User == null)
             {
                 return NotFound();
             }

             return View(User);
         }

          //GET: ADMINs/Create
         public IActionResult Create()
         {
             return View();
         }

         // POST: ADMINs/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create(Admin admin)
         {
             if (ModelState.IsValid)
             {
                await _adminRepository.AddAdminAsync(admin);
                 
                 return RedirectToAction(nameof(Index));
             }
             return View(User);
         }

         // GET: ADMINs/Edit/5
         public async Task<IActionResult> Edit(int? id)
         {
            string email = HttpContext.Session.GetString("Admin_Email");
            if (email == null)
            {
                return RedirectToAction("AdminLogIn", "Admin");
            }
            if (id == null)
             {
                 return NotFound();
             }

            var user = await _adminRepository.GetUserByIdAsync(id);
             if (user == null)
             {
                 return NotFound();
             }
             return View(user);
         }

         // POST: ADMINs/Edit/5
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Edit(int id, [Bind("UserID,FisrtName,LastName,District,Loaction,Email,Phone,Pasword")] Users users)
         {
            string email = HttpContext.Session.GetString("Admin_Email");
            if (email == null)
            {
                return RedirectToAction("AdminLogIn", "Admin");
            }
             
            if (id != users.UserId)
             {
                 return NotFound();
             }

             if (ModelState.IsValid)
             {
                 try
                 {
                   await  _adminRepository.UpdateUserAsync(users);
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                    bool userexist = _adminRepository.UserExists(users.UserId);
                     if (!userexist)
                     {
                         return NotFound();
                     }
                     else
                     {
                         throw;
                     }
                 }
                 return RedirectToAction(nameof(Index));
             }
             return View(users);
         }

        private bool USERSExists(int userId)
        {
            throw new NotImplementedException();
        }

        // GET: ADMINs/Delete/5
        public async Task<IActionResult> Delete(int? id)
         {
            string email = HttpContext.Session.GetString("Admin_Email");
            if (email == null)
            {
                return RedirectToAction("AdminLogIn", "Admin");
            }
            if (id == null)
             {
                 return NotFound();
             }

   
             var user = await _adminRepository.GetUserByIdAsync(id);
             if (user == null)
             {
                 return NotFound();
             }
             return View(user);
         }

         // POST: ADMINs/Delete/5
         [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteConfirmed(int id)
         {
            var user = await _adminRepository.GetUserByIdAsync(id);
            await _adminRepository.DeleteUserAsync(user);
             return RedirectToAction(nameof(Index));
         }

        
    }
}
