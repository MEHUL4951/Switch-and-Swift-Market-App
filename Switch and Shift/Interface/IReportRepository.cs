using Switch_and_Shift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Switch_and_Shift.Interface
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<IEnumerable<Report>> GetBoughtProductsAsync(string buyerEmail);
        Task<IEnumerable<Report>> GetSoldProductsAsync(string sellerEmail);
        Task<Report> GetReportByIdAsync(int id);
        Task AddReportAsync(Report report);
        Task UpdateReportAsync(Report report);
        Task DeleteReportAsync(Report report);
        bool ReportExists(int id);
    }
}
