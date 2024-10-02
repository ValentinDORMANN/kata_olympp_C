using Kata.Domain.Entities;
using Kata.Domain.Repositories;
using Kata.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kata.Infrastructure.Repositories {
    public class BattleRepository : IBattleRepository {
        private readonly KataDBContext _context;
        private readonly ILogger<BattleRepository> _logger;

        public BattleRepository (KataDBContext context, ILogger<BattleRepository> logger) {
            this._context = context;
            this._logger = logger;
        }
        public async Task<IEnumerable<BattleReport>> GetAllBattlesReport() {
            try {
                return await this._context.BattleReports
                    .OrderBy((BattleReport battleReport) => battleReport.Id)
                    .ToListAsync();
            } catch (Exception exception) { // DbUpdateException 
                this._logger.LogError($"Error occurred while saving battleReport : {exception.Message}");
                throw;
            }
        }

        public async Task<BattleReport?> GetBattleReportById(Guid id) {
            try {
                return await this._context.BattleReports
                    .Where((BattleReport battleReport) => battleReport.Id == id)
                    .FirstOrDefaultAsync();
            } catch (Exception exception) { // DbUpdateException 
                this._logger.LogError($"Error occurred while saving battleReport : {exception.Message}");
                throw;
            }
        }

        public async Task<BattleReport> SaveBattleReport(BattleReport battleReport) {
            try {
                if (battleReport == null) { throw new ArgumentNullException(); }
                this._context.BattleReports.Add(battleReport);
                await this._context.SaveChangesAsync();
                return battleReport;
            } catch (Exception exception) { // DbUpdateException 
                this._logger.LogError($"Error occurred while saving battleReport : {exception.Message}");
                throw;
            }
        }
    }
}
