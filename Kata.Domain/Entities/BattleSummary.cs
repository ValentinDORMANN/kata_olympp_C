using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kata.Domain.Entities {
    public class BattleSummary {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("BattleReport")]
        public int ReportId { get; set; }

        public string ArmyName1 { get; set; }
        public string ArmyName2 { get; set; }
        public int ArmyDamage1 { get; set; }
        public int ArmyDamage2 { get; set; }
        public int UnitCount1 { get; set; }
        public int UnitCount2 { get; set; }

        public BattleSummary() {
            this.Id = Guid.NewGuid();
            this.ArmyName1 = "";
            this.ArmyName2 = "";
            this.ArmyDamage1 = 0;
            this.ArmyDamage2 = 0;
            this.UnitCount1 = 0;
            this.UnitCount2 = 0;
        }

        public BattleSummary SetArmy1(string armyName, int armyDamage, int unitCount) {
            this.ArmyName1 = armyName;
            this.ArmyDamage1 = armyDamage;
            this.UnitCount1 = Math.Max(0, unitCount);
            return this;
        }

        public BattleSummary SetArmy2(string armyName, int armyDamage, int unitCount) {
            this.ArmyName2 = armyName;
            this.ArmyDamage2 = armyDamage;
            this.UnitCount2 = Math.Max(0, unitCount);
            return this;
        }
    }
}
