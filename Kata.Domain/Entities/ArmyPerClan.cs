namespace Kata.Domain.Entities {
    public class ArmyPerClan {
        public Guid ClanId { get; set; }
        public Guid ArmyId { get; set; }
        public int Order { get; set; }

        public Clan Clan { get; set; }
        public Army Army { get; set; }

        public ArmyPerClan() {
            ClanId = Guid.Empty;
            ArmyId = Guid.Empty;
            Order = 0;
        }
        public ArmyPerClan(Guid clanId, Guid ArmyId, int order) {
            this.ClanId = clanId;
            this.ArmyId = ArmyId;
            this.Order = order;
        }
    }
}
