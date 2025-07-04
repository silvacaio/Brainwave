using Brainwave.Core.DomainObjects;
using Xunit;

namespace Brainwave.Tests.Domain
{
    public class ValidationsTests
    {
        [Fact]
        public void ValidateIfEqual_ShouldThrow_WhenObjectsAreEqual()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateIfEqual(1, 1, "Values should not be equal"));
            Assert.Equal("Values should not be equal", ex.Message);
        }

        [Fact]
        public void ValidateIfDifferent_ShouldThrow_WhenObjectsAreDifferent()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateIfDifferent(1, 2, "Values should be equal"));
            Assert.Equal("Values should be equal", ex.Message);
        }

        [Fact]
        public void ValidateIfNotMatch_ShouldThrow_WhenRegexDoesNotMatch()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateIfNotMatch(@"^\d+$", "abc", "Must be numbers only"));
            Assert.Equal("Must be numbers only", ex.Message);
        }

        [Fact]
        public void ValidateMaxLength_ShouldThrow_WhenExceedsMaxLength()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateMaxLength("too long", 3, "Exceeded max length"));
            Assert.Equal("Exceeded max length", ex.Message);
        }

        [Theory]
        [InlineData("abc", 2, 5, false)]
        [InlineData("a", 2, 5, true)]
        [InlineData("abcdef", 2, 5, true)]
        public void ValidateLength_ShouldThrow_WhenOutOfBounds(string input, int min, int max, bool shouldThrow)
        {
            if (shouldThrow)
            {
                var ex = Assert.Throws<DomainException>(() =>
                    Validations.ValidateLength(input, min, max, "Length out of bounds"));
                Assert.Equal("Length out of bounds", ex.Message);
            }
            else
            {
                Validations.ValidateLength(input, min, max, "Should not throw");
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ValidateIfEmpty_ShouldThrow_WhenStringIsNullOrWhitespace(string input)
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateIfEmpty(input, "Value is empty"));
            Assert.Equal("Value is empty", ex.Message);
        }

        [Fact]
        public void ValidateIfNull_ShouldThrow_WhenObjectIsNull()
        {
            object obj = null!;
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateIfNull(obj, "Object is null"));
            Assert.Equal("Object is null", ex.Message);
        }

        [Theory]
        [InlineData(10, 5, 9)]
        [InlineData(1, 2, 3)]
        public void ValidateRange_Int_ShouldThrow_WhenOutsideRange(int value, int min, int max)
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateRange(value, min, max, "Out of range"));
            Assert.Equal("Out of range", ex.Message);
        }

        [Theory]
        [InlineData(0.5, 1.0, 2.0)]
        [InlineData(5.5, 6.0, 7.0)]
        public void ValidateRange_Double_ShouldThrow_WhenOutsideRange(double value, double min, double max)
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateRange(value, min, max, "Out of range"));
            Assert.Equal("Out of range", ex.Message);
        }

        [Theory]
        [InlineData(100L, 200L, 300L)]
        [InlineData(0L, 1L, 10L)]
        public void ValidateRange_Long_ShouldThrow_WhenOutsideRange(long value, long min, long max)
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateRange(value, min, max, "Out of range"));
            Assert.Equal("Out of range", ex.Message);
        }


        [Fact]
        public void ValidateIfLessThan_ShouldThrow_WhenLessThanMin()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateIfLessThan(1, 5, "Too small"));
            Assert.Equal("Too small", ex.Message);
        }

        [Fact]
        public void ValidateIfFalse_ShouldThrow_WhenFalse()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateIfFalse(false, "Must be true"));
            Assert.Equal("Must be true", ex.Message);
        }

        [Fact]
        public void ValidateIfTrue_ShouldThrow_WhenTrue()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Validations.ValidateIfTrue(true, "Must be false"));
            Assert.Equal("Must be false", ex.Message);
        }
    }
}
