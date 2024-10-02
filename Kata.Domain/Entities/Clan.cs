using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kata.Domain.Entities {
    public class Clan {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ArmyPerClan> ArmyPerClans { get; set; } = new List<ArmyPerClan>();

        public Clan() {
            this.Id = Guid.NewGuid();
            this.Name = "";
            this.ArmyPerClans = new List<ArmyPerClan>();
        }
        public Clan(string name) {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.ArmyPerClans = new List<ArmyPerClan>();
        }


        [NotMapped]
        public List<Army> Armies => this.ArmyPerClans
            .OrderBy(apc => apc.Order)
            .Select(apc => apc.Army).ToList();

        public bool HasArmiesLeft() => this.Armies.Any((Army army) => army.IsAlive());
    }
}
