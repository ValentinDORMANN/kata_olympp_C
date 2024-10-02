using Kata.Domain.Entities;
using Moq;

namespace Kata.Tests {
    public class ArmyTests {
        private static readonly string _NAME = "army name";
        private static readonly Guid _INFRANTRY_ID = Guid.NewGuid();
        private static readonly int _DAMAGE = 100;
        private static readonly int _NEW_ATTACK_POWER = 123;
        private static readonly int _NEW_DEFENSE_POWER = 456;

        private Mock<Regiment> _regimentMock;
        
        public ArmyTests() {
            this._regimentMock = new Mock<Regiment>(MockBehavior.Strict);
        }

        [Fact]
        public void Constructor_WithParameters_SetsProperties() {
            var army = new Army(_NAME, _INFRANTRY_ID);

            Assert.NotEqual(Guid.Empty, army.Id);
            Assert.Equal(_NAME, army.Name);
            Assert.Equal(_INFRANTRY_ID, army.InfantryId);
            Assert.Null(army.Infantry);
            Assert.Equal(0, army.AttackPower);
            Assert.Equal(0, army.DefensePower);
        }

        [Fact]
        public void IsAlive_WithoutRegiment_ReturnsFalse() {
            var army = new Army(_NAME, _INFRANTRY_ID);
            this._regimentMock.Setup(m => m.IsAlive()).Returns(true);

            var result = army.IsAlive();

            Assert.False(result);
            this._regimentMock.Verify(m => m.IsAlive(), Times.Never);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsAlive_HasRegiment_ReturnsInfanteryIsAlive(bool isAlive) {
            var army = new Army(_NAME, _INFRANTRY_ID) { Infantry = this._regimentMock.Object };
            this._regimentMock.Setup(m => m.IsAlive()).Returns(isAlive);

            var result = army.IsAlive();

            Assert.Equal(isAlive, result);
            this._regimentMock.Verify(m => m.IsAlive(), Times.Once);
        }

        [Fact]
        public void TakeDamage_CallsInfantryTakeDamageAndEvaluatePower() {
            var army = new Army(_NAME, _INFRANTRY_ID) { Infantry = this._regimentMock.Object };
            this._regimentMock.Setup(m => m.TakeDamage(It.IsAny<int>()));
            this._regimentMock.Setup(m => m.TotalAttackPower()).Returns(_NEW_ATTACK_POWER);
            this._regimentMock.Setup(m => m.TotalDefensePower()).Returns(_NEW_DEFENSE_POWER);

            army.TakeDamage(_DAMAGE);

            this._regimentMock.Verify(m => m.TakeDamage(_DAMAGE), Times.Once);
            this._regimentMock.Verify(m => m.TotalAttackPower(), Times.Once);
            this._regimentMock.Verify(m => m.TotalDefensePower(), Times.Once);
            Assert.Equal(_NEW_ATTACK_POWER, army.AttackPower);
            Assert.Equal(_NEW_DEFENSE_POWER, army.DefensePower);
        }
    }
}
