using System.ComponentModel.DataAnnotations;

namespace Kata.Domain.Entities {
    public class BattleReport {
        [Key]
        public Guid Id { get; set; }
        public string? Winner { get; set; }
        public EBattleStatus Status { get; set; }
        public List<Clan> Clans { get; set; }
        public List<BattleSummary> BattleSummaries { get; set; }

        public BattleReport() {
            this.Id = Guid.NewGuid();
            this.Winner = null;
            this.Status = EBattleStatus.UNDEFINED;
            this.Clans = new List<Clan>();
            this.BattleSummaries = new List<BattleSummary>();
        }

        public BattleReport AddBattleSummary(BattleSummary battleSummary) {
            this.BattleSummaries.Add(battleSummary);
            return this;
        }
    }
}
