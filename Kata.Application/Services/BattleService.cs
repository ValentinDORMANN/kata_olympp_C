using Kata.Domain.Entities;
using Kata.Domain.Interfaces;
using Kata.Domain.Repositories;

namespace Kata.Application.Services {
    public class BattleService : IBattleService {
        private readonly IBattleRepository _battleRepository;
        private readonly IClanRepository _clanRepository;
        public BattleService(IBattleRepository battleRepository, IClanRepository clanRepository) {
            this._battleRepository = battleRepository;
            this._clanRepository = clanRepository;
        }

        /// <summary>
        /// Generates a battle between the armies of the first and the last Clan
        /// Returns a battle report after saving it for historisation 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<BattleReport> Battle() {
            try {
                // TODO Remove for taking first and last Clan in db
                Clan? clan1 = await this._clanRepository.GetClanByNameAsync("troyes");
                Clan? clan2 = await this._clanRepository.GetClanByNameAsync("grecques");

                if (clan1 == null || clan2 == null) { throw new KeyNotFoundException("Some clan does not exist"); }

                BattleReport battleReport = new BattleReport() {
                    Clans = new List<Clan> { clan1, clan2 }
                };

                while (clan1.HasArmiesLeft() && clan2.HasArmiesLeft()) {
                    Army army1 = clan1.Armies.First(a => a.IsAlive());
                    Army army2 = clan2.Armies.First(a => a.IsAlive());

                    int damage1to2 = Math.Max(0, army1.AttackPower - army2.DefensePower);
                    int damage2to1 = Math.Max(0, army2.AttackPower - army1.DefensePower);

                    army1.TakeDamage(damage2to1);
                    army2.TakeDamage(damage1to2);

                    BattleSummary battleSummary = new BattleSummary()
                        .SetArmy1(army1.Name, damage1to2, army1.Infantry.UnitCount)
                        .SetArmy2(army2.Name, damage2to1, army2.Infantry.UnitCount);
                    battleReport.AddBattleSummary(battleSummary);

                    if (damage1to2 == 0 && damage2to1 == 0) { break; }
                }

                bool clan1HasArmies = clan1.HasArmiesLeft();
                bool clan2HasArmies = clan2.HasArmiesLeft();
                if (clan1HasArmies && !clan2HasArmies) {
                    battleReport.Status = EBattleStatus.WIN;
                    battleReport.Winner = clan1.Name;
                } else if (!clan1HasArmies && clan2HasArmies) {
                    battleReport.Status = EBattleStatus.LOSE;
                    battleReport.Winner = clan2.Name;
                } else {
                    battleReport.Status = EBattleStatus.DRAW;
                    battleReport.Winner = null;
                }

                await this._battleRepository.SaveBattleReport(battleReport);

                return battleReport;
            } catch (Exception) {
                throw;
            }
        }
    }
}
