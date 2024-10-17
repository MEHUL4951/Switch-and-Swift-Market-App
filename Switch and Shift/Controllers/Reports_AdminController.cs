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
    public class Reports_AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IReportsAdminRepository reportsAdminRepository;

        public Reports_AdminController(ApplicationDbContext context,IReportsAdminRepository reportsAdminRepository)
        {
            _context = context;
            this.reportsAdminRepository = reportsAdminRepository;
        }

        // GET: REPORTS_ADMIN
        public async Task<IActionResult> Index()
        {
            return View(await reportsAdminRepository.GetAllReportsAsync());
        }


        // GET: REPORTS_ADMIN/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           
            var reports_admin = await reportsAdminRepository.GetReportByIdAsync(id);
            if (reports_admin == null)
            {
                return NotFound();
            }

            return View(reports_admin);
        }

        // GET: REPORTS_ADMIN/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: REPORTS_ADMIN/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("report_admin_id,reporter_email,reportee_email")] Reports_Admin Report_Admin)
        {
            if (ModelState.IsValid)
            {
              await  reportsAdminRepository.AddReportAsync(Report_Admin);
               return RedirectToAction(nameof(Index));
            }
            return View(Report_Admin);
        }

        // GET: REPORTS_ADMIN/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Report_Admin = await reportsAdminRepository.GetReportByIdAsync(id);
            if (Report_Admin == null)
            {
                return NotFound();
            }
            return View(Report_Admin);
        }


        [HttpGet]
        public async Task<IActionResult> ReportPost2(int? id)
        {
            if (id.HasValue)
            {
                
                var reports_admin = await reportsAdminRepository.GetReportByIdAsync(id);
                reports_admin.report_show = 0;
               await  reportsAdminRepository.UpdateReportAsync(reports_admin);
            }

            var visibleReports = await reportsAdminRepository.GetVisibleReportsAsync();
            return View(visibleReports);
        }



        // POST: REPORTS_ADMIN/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("report_admin_id,reporter_email,reportee_email")] Reports_Admin Report_Admin)
        {
            if (id != Report_Admin.report_admin_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await reportsAdminRepository.UpdateReportAsync((Reports_Admin) Report_Admin);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool reports_adminExists = reportsAdminRepository.ReportExists(Report_Admin.report_admin_id);
                    if (!reports_adminExists)
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
            return View(Report_Admin);
        }

        // GET: REPORTS_ADMIN/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            

            var Report_Admin = await reportsAdminRepository.GetReportByIdAsync(id);
            if (Report_Admin == null)
            {
                return NotFound();
            }

            return View(Report_Admin);
        }

        // POST: REPORTS_ADMIN/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Report_Admin = await reportsAdminRepository.GetReportByIdAsync(id);
            await  reportsAdminRepository.DeleteReportAsync(Report_Admin);
            return RedirectToAction(nameof(Index));
        }

        private bool REPORTS_ADMINExists(int id)
        {
            return reportsAdminRepository.ReportExists(id);
        }
    }
}
