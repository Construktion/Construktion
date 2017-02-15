namespace Construktion.Tests.Acceptance
{
    using System;
    using Shouldly;
    using Xunit;

    public class PrimitiveConstruktionTests
    {
        private readonly Construktion _construktion;

        public PrimitiveConstruktionTests()
        {
            _construktion = new Construktion();
        }

        [Fact]
        public void strings()
        {
            var result = _construktion.Construct<string>();

            result.Substring(0, 7).ShouldBe("String-");
        }

        [Fact]
        public void chars()
        {
            var result = _construktion.Construct<char>();

            result.ShouldNotBeNull();
        }

        [Fact]
        public void guids()
        {
            var result = _construktion.Construct<Guid>();

            result.ShouldNotBe(new Guid());
        }

        [Fact]
        public void bytes()
        {
            var result = _construktion.Construct<byte>();

            result.ShouldNotBe(default(byte));
        }

        [Fact]
        public void decimals()
        {
            var result = _construktion.Construct<decimal>();

            result.ShouldNotBe(default(decimal));
        }

        [Fact]
        public void doubles()
        {
            var result = _construktion.Construct<double>();

            result.ShouldNotBe(default(double));
        }

        [Fact]
        public void shorts()
        {
            var result = _construktion.Construct<short>();

            result.ShouldNotBe(default(short));
        }

        [Fact]
        public void ints()
        {
            var result = _construktion.Construct<int>();

            result.ShouldNotBe(default(int));
        }

        [Fact]
        public void sbytes()
        {
            var result = _construktion.Construct<sbyte>();

            result.ShouldNotBe(default(sbyte));
        }

        [Fact]
        public void floats()
        {
            var result = _construktion.Construct<float>();

            result.ShouldNotBe(default(float));
        }

        [Fact]
        public void ushorts()
        {
            var result = _construktion.Construct<ushort>();

            result.ShouldNotBe(default(ushort));
        }

        [Fact]
        public void uints()
        {
            var result = _construktion.Construct<uint>();

            result.ShouldNotBe(default(uint));
        }

        [Fact]
        public void ulongs()
        {
            var result = _construktion.Construct<ulong>();

            result.ShouldNotBe(default(ulong));
        }

        [Fact]
        public void single()
        {
            var result = _construktion.Construct<Single>();

            result.ShouldNotBe(default(Single));
        }

        [Fact]
        public void bools()
        {
            var result = _construktion.Construct<bool>();

            result.ShouldBeOneOf(true, false);
        }
    }
}
