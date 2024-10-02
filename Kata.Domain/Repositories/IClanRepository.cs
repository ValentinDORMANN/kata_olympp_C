using Kata.Domain.Entities;

namespace Kata.Domain.Repositories {
    public interface IClanRepository {
        Task<IEnumerable<Clan>> GetAllClansAsync();
        Task<Clan?> GetClanByNameAsync(string name);
        Task<Army?> GetArmyByClanNameAsync(string name);
        Task AddArmyAsync(string clanName, Army army);
        Task UpdateArmyAsync(string clanName, string armyName, Army army);
        Task DeleteArmyAsync(string clanName, string armyName);
    }
}
