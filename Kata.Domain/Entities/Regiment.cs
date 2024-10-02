using System.ComponentModel.DataAnnotations;

namespace Kata.Domain.Entities {
    public class Regiment {
        [Key]
        public Guid Id { get; set; }
        public int UnitCount { get; set; }
        public int AttackPower { get; set; }
        public int DefensePower { get; set; }
        public int HealthPoint { get; set; }

        public Regiment() {
            this.Id = Guid.NewGuid();
            this.UnitCount = 0;
            this.AttackPower = 0;
            this.DefensePower = 0;
            this.HealthPoint = 0;
        }
        public Regiment(int soldatCount, int attackPower, int defensePower, int healthPoint) {
            this.Id = Guid.NewGuid();
            this.UnitCount = soldatCount;
            this.AttackPower = attackPower;
            this.DefensePower = defensePower;
            this.HealthPoint = healthPoint;
        }
        
        public virtual int TotalAttackPower() => this.UnitCount * this.AttackPower;
        public virtual int TotalDefensePower() => this.UnitCount * this.DefensePower;
        public virtual bool IsAlive() => this.UnitCount > 0;

        public virtual void TakeDamage(int damage) {
            int loseUnitCount = damage / this.HealthPoint;
            this.UnitCount = Math.Max(0, this.UnitCount - loseUnitCount);
        }
    }
}
