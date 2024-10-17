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
using Switch_and_Shift.Repository;


namespace Switch_and_Shift.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IReportRepository reportRepository;
        private readonly IReportsAdminRepository reportsAdminRepository;
        public ReportsController(ApplicationDbContext context,IReportRepository reportRepository, IReportsAdminRepository reportsAdminRepository)
        {
            _context = context;
            this.reportRepository = reportRepository;
            this.reportsAdminRepository = reportsAdminRepository;
            this.reportsAdminRepository = reportsAdminRepository;

        }

        // GET: REPORTs
        public async Task<IActionResult> Index()
        {            
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> BoughtProducts()
        {
            var boughtReports = await reportRepository.GetBoughtProductsAsync(HttpContext.Session.GetString("Email"));
            return View(boughtReports);
        }


        [HttpGet]
        public async Task<IActionResult> SoldProducts()
        {
            var soldReports = await reportRepository.GetSoldProductsAsync(HttpContext.Session.GetString("Email"));
            return View(soldReports);
        }


        [HttpGet]
        public async Task<IActionResult> CreateReport(string id)
        {
          
   
            var reportQuery =await reportsAdminRepository.GetAllReportsAsync();
            bool reportExists = reportQuery.Any(rep => rep.reporter_email == HttpContext.Session.GetString("Email") && rep.reportee_email == id);

            if (!reportExists)
            {                
                //success Toast
                Reports_Admin report = new Reports_Admin();
                report.reportee_email = id;
                report.report_show = 1;
                report.reporter_email = HttpContext.Session.GetString("Email");
                report.reportee_name = _context.Users.FirstOrDefault(m => m.Email == report.reportee_email).FirstName.ToString() + " " +
                    _context.Users.FirstOrDefault(m => m.Email == report.reportee_email).LastName.ToString();
                report.reporter_name = _context.Users.FirstOrDefault(m => m.Email == report.reporter_email).FirstName.ToString() + " " +
                    _context.Users.FirstOrDefault(m => m.Email == report.reporter_email).LastName.ToString();
              
                 await  reportsAdminRepository.AddReportAsync(report);
            }
            else
            {
                
                //ERROR TOAST
                ViewBag.CreateReportNotification = "You've already submitted a report";
            }
            return RedirectToAction("SoldProducts", "REPORTS");
        }

        [HttpGet]
        public async Task<IActionResult> CreateReport2(string id)
        {
            var reportQuery = await reportsAdminRepository.GetAllReportsAsync();
            bool reportExists = reportQuery.Any(rep => rep.reporter_email == HttpContext.Session.GetString("Email") && rep.reportee_email == id);

            if (!reportExists)
            {
                //success Toast
                Reports_Admin report = new Reports_Admin();
                report.reportee_email = id;
                report.report_show = 1;
                report.reporter_email = HttpContext.Session.GetString("Email");
                report.reportee_name = _context.Users.FirstOrDefault(m => m.Email == report.reportee_email).FirstName.ToString() + " " +
                    _context.Users.FirstOrDefault(m => m.Email == report.reportee_email).LastName.ToString();
                report.reporter_name = _context.Users.FirstOrDefault(m => m.Email == report.reporter_email).FirstName.ToString() + " " +
                    _context.Users.FirstOrDefault(m => m.Email == report.reporter_email).LastName.ToString();
               
                await reportsAdminRepository.AddReportAsync(report);
            }
            else
            {
                //FIX BUG
                //ERROR TOAST
                ViewBag.CreateReportNotification2 = "You've already submitted a report";
            }
            return RedirectToAction("BoughtProducts", "REPORTS");
        }



        [HttpGet]
        public async Task<IActionResult> AddReview(string id)
        {
            var reviewee = _context.Users.Where(c => c.Email == id).FirstOrDefault();
            return RedirectToAction("UserReview", "USERS", new { @id = reviewee.UserId });
        }


       


        // GET: REPORTs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Report = await reportRepository.GetReportByIdAsync(id.Value);
            if (Report == null)
            {
                return NotFound();
            }

            return View(Report);
        }

        // GET: REPORTs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: REPORTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("report_Id,product_model,product_brand,product_price,buyer_email,seller_email")] Report report)
        {
            if (ModelState.IsValid)
            {
                await reportRepository.AddReportAsync(report);
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: REPORTs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Report = await reportRepository.GetReportByIdAsync(id.Value);
            if (Report == null)
            {
                return NotFound();
            }
            return View(Report);
        }

        // POST: REPORTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("report_Id,product_model,product_brand,product_price,buyer_email,seller_email")] Report report)
        {
            if (id != report.report_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  
                  await reportRepository.UpdateReportAsync(report);
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool reportexists = reportRepository.ReportExists(report.report_Id);
                    if (!reportexists)
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
            return View(report);
        }

        // GET: REPORTs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Report = await reportRepository.GetReportByIdAsync(id.Value);
            if (Report == null)
            {
                return NotFound();
            }

            return View(Report);
        }

        // POST: REPORTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Report = await reportRepository.GetReportByIdAsync(id);
            await reportRepository.DeleteReportAsync(Report);
            return RedirectToAction(nameof(Index));
        }

        private bool REPORTExists(int id)
        {
           return reportRepository.ReportExists(id);
        }
    }
}
