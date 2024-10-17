using Switch_and_Shift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Switch_and_Shift.Interface
{
    public interface IReportsAdminRepository
    {
        Task<IEnumerable<Reports_Admin>> GetAllReportsAsync();
        Task<Reports_Admin> GetReportByIdAsync(int? reportAdminId);
        Task AddReportAsync(Reports_Admin reportAdmin);
        Task UpdateReportAsync(Reports_Admin reportAdmin);
        Task DeleteReportAsync(Reports_Admin report_admin);
        Task<IEnumerable<Reports_Admin>> GetVisibleReportsAsync();
        bool ReportExists(int reportAdminId);
    }
}
