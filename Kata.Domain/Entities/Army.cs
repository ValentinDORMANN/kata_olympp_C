using System.ComponentModel.DataAnnotations;

namespace Kata.Domain.Entities {
    public class Army {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid InfantryId { get; set; }

        public Regiment Infantry { get; set; }
        
        public int AttackPower { get; set; }
        public int DefensePower { get; set; }

        public List<ArmyPerClan> ArmyPerClans { get; set; } = new List<ArmyPerClan>();

        public Army() {
            this.Id = Guid.NewGuid();
            this.Name = "";
            this.InfantryId = Guid.Empty;
            this.EvaluatePower();
        }
        public Army(string name, Guid infrantryId) {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.InfantryId = infrantryId;
            this.EvaluatePower();
        }

        private void EvaluatePower() {
            this.AttackPower = this.Infantry?.TotalAttackPower() ?? 0;
            this.DefensePower = this.Infantry?.TotalDefensePower() ?? 0;
        }

        public bool IsAlive() => this.Infantry?.IsAlive() ?? false;

        public void TakeDamage(int damage) {
            this.Infantry?.TakeDamage(damage);
            this.EvaluatePower();
        }
    }
}
