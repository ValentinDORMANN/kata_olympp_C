using Kata.Domain.Entities;

namespace Kata.Tests.Domain.Entities {
    public class RegimentTests {
        private static readonly int _UNIT_COUNT = 100;
        private static readonly int _ATTACK_POWER = 50;
        private static readonly int _DEFENSE_POWER = 55;
        private static readonly int _HEALTH_POINT = 100;
        private static readonly int _DAMAGE = _HEALTH_POINT + 1;

        private Regiment SetupNewRegiment() {
            return new Regiment(_UNIT_COUNT, _ATTACK_POWER, _DEFENSE_POWER, _HEALTH_POINT);
        }

        [Fact]
        public void Constructor_WithParameters_SetsProperties() {
            var regiment = this.SetupNewRegiment();

            Assert.Equal(_UNIT_COUNT, regiment.UnitCount);
            Assert.Equal(_ATTACK_POWER, regiment.AttackPower);
            Assert.Equal(_DEFENSE_POWER, regiment.DefensePower);
            Assert.Equal(_HEALTH_POINT, regiment.HealthPoint);
        }

        [Fact]
        public void TotalAttackPower_ReturnsCorrectValue() {
            var regiment = this.SetupNewRegiment();

            int result = regiment.TotalAttackPower();

            Assert.Equal(_UNIT_COUNT * _ATTACK_POWER, result);
        }

        [Fact]
        public void TotalDefensePower_ReturnsCorrectValue() {
            var regiment = this.SetupNewRegiment();

            int result = regiment.TotalDefensePower();

            Assert.Equal(_UNIT_COUNT * _DEFENSE_POWER, result);
        }

        [Fact]
        public void IsAlive_WhenUnitCountIsGreaterThanZero_ReturnsTrue() {
            var regiment = this.SetupNewRegiment();

            bool result = regiment.IsAlive();

            Assert.True(result);
        }

        [Fact]
        public void IsAlive_WhenUnitCountIsZero_ReturnsFalse() {
            var regiment = this.SetupNewRegiment();
            regiment.UnitCount = 0;

            bool result = regiment.IsAlive();

            Assert.False(result);
        }

        [Fact]
        public void TakeDamage_ReducesUnitCountCorrectly() {
            var regiment = this.SetupNewRegiment();

            regiment.TakeDamage(_DAMAGE);

            Assert.Equal(_UNIT_COUNT - 1, regiment.UnitCount);
        }

        [Fact]
        public void TakeDamage_DoesNotReduceUnitCountBelowZero() {
            var regiment = this.SetupNewRegiment();
            regiment.UnitCount = 0;

            regiment.TakeDamage(_DAMAGE);

            Assert.Equal(0, regiment.UnitCount);
        }
    }
}
