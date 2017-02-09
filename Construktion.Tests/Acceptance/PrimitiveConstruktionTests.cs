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
            var result = _construktion.Build<string>();

            result.Substring(0, 7).ShouldBe("String-");
        }

        [Fact]
        public void chars()
        {
            var result = _construktion.Build<char>();

            result.ShouldNotBeNull();
        }

        [Fact]
        public void guids()
        {
            var result = _construktion.Build<Guid>();

            result.ShouldNotBe(new Guid());
        }

        [Fact]
        public void bytes()
        {
            var result = _construktion.Build<byte>();

            result.ShouldNotBe(default(byte));
        }

        [Fact]
        public void decimals()
        {
            var result = _construktion.Build<decimal>();

            result.ShouldNotBe(default(decimal));
        }

        [Fact]
        public void doubles()
        {
            var result = _construktion.Build<double>();

            result.ShouldNotBe(default(double));
        }

        [Fact]
        public void shorts()
        {
            var result = _construktion.Build<short>();

            result.ShouldNotBe(default(short));
        }

        [Fact]
        public void ints()
        {
            var result = _construktion.Build<int>();

            result.ShouldNotBe(default(int));
        }

        [Fact]
        public void sbytes()
        {
            var result = _construktion.Build<sbyte>();

            result.ShouldNotBe(default(sbyte));
        }

        [Fact]
        public void floats()
        {
            var result = _construktion.Build<float>();

            result.ShouldNotBe(default(float));
        }

        [Fact]
        public void ushorts()
        {
            var result = _construktion.Build<ushort>();

            result.ShouldNotBe(default(ushort));
        }

        [Fact]
        public void unit()
        {
            var result = _construktion.Build<uint>();

            result.ShouldNotBe(default(uint));
        }

        [Fact]
        public void ulongs()
        {
            var result = _construktion.Build<ulong>();

            result.ShouldNotBe(default(ulong));
        }

        [Fact]
        public void bools()
        {
            var result = _construktion.Build<bool>();

            result.ShouldBeOneOf(true, false);
        }
    }
}
