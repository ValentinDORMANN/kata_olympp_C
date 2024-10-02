using Kata.Domain.Entities;

namespace Kata.Domain.Interfaces {
    public interface IBattleService {
        Task<BattleReport> Battle();
    }
}
