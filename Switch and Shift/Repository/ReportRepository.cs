using Switch_and_Shift.Interface;
using Switch_and_Shift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Switch_and_Shift.Repository
{
    public class ReportRepository : IReportRepository
    {

        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _context.Report.ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetBoughtProductsAsync(string buyerEmail)
        {
            return await _context.Report
                .Where(x => x.buyer_email == buyerEmail)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetSoldProductsAsync(string sellerEmail)
        {
            return await _context.Report
                .Where(x => x.seller_email == sellerEmail)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Report> GetReportByIdAsync(int id)
        {
            return await _context.Report.FindAsync(id);
        }

        public async Task AddReportAsync(Report report)
        {
            await _context.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReportAsync(Report report)
        {
            _context.Update(report);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReportAsync(Report report)
        {

            if (report != null)
            {
                _context.Report.Remove(report);
                await _context.SaveChangesAsync();
            }
        }

        public bool ReportExists(int id)
        {
            return _context.Report.Any(e => e.report_Id == id);
        }
    }
}
