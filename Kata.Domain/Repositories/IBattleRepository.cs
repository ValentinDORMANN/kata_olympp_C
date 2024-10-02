using Kata.Domain.Entities;

namespace Kata.Domain.Repositories {
    public interface IBattleRepository {
        Task<BattleReport> SaveBattleReport(BattleReport battleReport);
        Task<IEnumerable<BattleReport>> GetAllBattlesReport();
        Task<BattleReport?> GetBattleReportById(Guid id);
    }
}
