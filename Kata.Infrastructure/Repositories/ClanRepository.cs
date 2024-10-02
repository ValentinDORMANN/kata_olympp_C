using Kata.Domain.Entities;
using Kata.Domain.Repositories;
using Kata.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kata.Infrastructure.Repositories {
    public class ClanRepository : IClanRepository {
        private readonly KataDBContext _context;
        private readonly ILogger<ClanRepository> _logger;

        public ClanRepository(KataDBContext context, ILogger<ClanRepository> logger) {
            this._context = context;
            this._logger = logger;
        }

        public async Task<Army?> GetArmyByClanNameAsync(string name) {
            try {
                var clan = await _context.Clans
                    .Include(c => c.ArmyPerClans)
                    .ThenInclude(apc => apc.Army)
                    .FirstOrDefaultAsync(c => c.Name == name);
                return clan?.ArmyPerClans
                    .Select(apc => apc.Army)
                    .FirstOrDefault(a => a.Name == name);
            } catch (Exception exception) {
                this._logger.LogError($"Error occurred while getting army per clan name {name} : {exception.Message}");
                throw;
            }
        }

        public async Task<Clan?> GetClanByNameAsync(string name) {
            try {
                return await this._context.Clans
                    .Include((Clan clan) => clan.ArmyPerClans)
                    .ThenInclude((ArmyPerClan armyPerClan) => armyPerClan.Army)
                    .FirstOrDefaultAsync((Clan clan) => clan.Name == name);
            } catch (Exception exception) {
                this._logger.LogError($"Error occurred while getting clan {name}: {exception.Message}");
                throw;
            }
        }

        public async Task AddArmyAsync(string clanName, Army army) {
            try {
                Clan? clan = await this._context.Clans
                    .Include(c => c.ArmyPerClans)
                    .ThenInclude(apc => apc.Army)
                    .FirstOrDefaultAsync(c => c.Name == clanName);

                if (clan == null) {
                    throw new KeyNotFoundException($"Clan '{clanName}' not found.");
                }
                this._context.Armies.Add(army);
                await this._context.SaveChangesAsync();

                var armyPerClan = new ArmyPerClan(clan.Id, army.Id, clan.ArmyPerClans.Count + 1);
                this._context.ArmyPerClans.Add(armyPerClan);
                await this._context.SaveChangesAsync();
            } catch (Exception exception) {
                this._logger.LogError($"Error occurred while adding army {army.Name} to clan {clanName} : {exception.Message}");
                throw;
            }
        }

        public async Task UpdateArmyAsync(string clanName, string armyName, Army army) {
            throw new NotImplementedException();
        }

        public async Task DeleteArmyAsync(string clanName, string armyName) {
            try {
                Clan? clan = await _context.Clans
                    .Include(c => c.ArmyPerClans)
                    .ThenInclude(apc => apc.Army)
                    .FirstOrDefaultAsync(c => c.Name == clanName);

                if (clan == null) { throw new KeyNotFoundException($"Clan {clanName} does not exist"); }

                ArmyPerClan? armyPerClan = await this._context.ArmyPerClans
                    .FirstOrDefaultAsync(apc => apc.ClanId == clan.Id && apc.Army.Name == armyName);

                if (armyPerClan == null) {
                    throw new KeyNotFoundException($"Army '{armyName}' not found in clan '{clanName}'.");
                }

                this._context.ArmyPerClans.Remove(armyPerClan);

                Army? army = clan.Armies.FirstOrDefault(a => a.Name == armyName);
                if (army == null) { throw new KeyNotFoundException($"Clan {clanName} does not have army {armyName}"); }

                clan.Armies.Remove(army);
                await this._context.SaveChangesAsync();
            } catch (Exception exception) {
                this._logger.LogError($"Error occurred while dleting army {armyName} : {exception.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Clan>> GetAllClansAsync() {
            try {
                return await this._context.Clans
                    .Include(clan => clan.ArmyPerClans)
                    .ThenInclude(apc =>  apc.Army)
                    .OrderBy(clan => clan.Name)
                    .ToListAsync();
            } catch (Exception exception) {
                this._logger.LogError($"Error occurred while getting all clans : {exception.Message}");
                throw;
            }
        }
    }
}
