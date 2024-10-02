using Kata.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kata.Infrastructure.Data {
    public class KataDBContext : DbContext {
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Army> Armies { get; set; }
        public DbSet<Regiment> Regiments { get; set; }
        public DbSet<ArmyPerClan> ArmyPerClans { get; set; }
        public DbSet<BattleReport> BattleReports { get; set; }
        public DbSet<BattleSummary> BattleSummaries { get; set; }

        public KataDBContext(DbContextOptions<KataDBContext> options) : base(options) {
            Database.EnsureCreated();
            Seed(); // test only
        }

        private void Seed() {
            if (Clans.Any() || Armies.Any()) { return; }
            var clan1 = new Clan("troyes");
            var clan2 = new Clan("grecques");
            var clan3 = new Clan("test");
            Clans.AddRange(clan1, clan2, clan3);

            var regiment11 = new Regiment(100, 100, 100, 100);
            var regiment12 = new Regiment(100, 100, 100, 100);
            var regiment21 = new Regiment(50, 50, 500, 100);
            var regiment31 = new Regiment(1, 999999, 999999, 1);
            Regiments.AddRange(regiment11, regiment12, regiment21, regiment31);

            var army11 = new Army("army11", regiment11.Id);
            var army12 = new Army("army12", regiment12.Id);
            var army21 = new Army("army21", regiment21.Id);
            var army31 = new Army("army31", regiment31.Id);
            Armies.AddRange(army11, army12, army21, army31);

            var apc11 = new ArmyPerClan(clan1.Id, army11.Id, 10);
            var apc12 = new ArmyPerClan(clan1.Id, army12.Id, 20);
            var apc21 = new ArmyPerClan(clan2.Id, army21.Id, 10);
            var apc31 = new ArmyPerClan(clan3.Id, army31.Id, 10);
            ArmyPerClans.AddRange(apc11, apc12, apc21, apc31);

            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ArmyPerClan>()
                .HasKey(apc => new { apc.ClanId, apc.ArmyId });
            modelBuilder.Entity<ArmyPerClan>()
                .HasIndex(apc => new { apc.ArmyId, apc.ClanId, apc.Order })
                .IsUnique();

            modelBuilder.Entity<ArmyPerClan>()
                .HasOne(apc => apc.Army)
                .WithMany(a => a.ArmyPerClans)
                .HasForeignKey(apc => apc.ArmyId);

            modelBuilder.Entity<ArmyPerClan>()
                .HasOne(apc => apc.Clan)
                .WithMany(a => a.ArmyPerClans)
                .HasForeignKey(apc => apc.ClanId);

            modelBuilder.Entity<Army>()
                .HasOne(a => a.Infantry)
                .WithOne()
                .HasForeignKey<Army>(a => a.InfantryId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
