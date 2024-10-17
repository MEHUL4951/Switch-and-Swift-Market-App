using Switch_and_Shift.Interface;
using Switch_and_Shift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;



namespace Switch_and_Shift.Repository
{
    public class ReportsAdminRepository : IReportsAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportsAdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reports_Admin>> GetAllReportsAsync()
        {
            return await _context.Reports_Admin.ToListAsync();
        }

        public async Task<Reports_Admin> GetReportByIdAsync(int? reportAdminId)
        {
            return await _context.Reports_Admin.FindAsync(reportAdminId);
        }

        public async Task AddReportAsync(Reports_Admin reportAdmin)
        {
            await _context.Reports_Admin.AddAsync(reportAdmin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReportAsync(Reports_Admin reportAdmin)
        {
            _context.Reports_Admin.Update(reportAdmin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReportAsync(Reports_Admin reports_admin)
        {
            _context.Reports_Admin.Remove(reports_admin);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reports_Admin>> GetVisibleReportsAsync()
        {
            return await _context.Reports_Admin.Where(x => x.report_show == 1).ToListAsync();
        }

        public bool ReportExists(int reportAdminId)
        {
            return _context.Reports_Admin.Any(e => e.report_admin_id == reportAdminId);
        }
    }

}
