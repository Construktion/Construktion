namespace Construktion.Tests.Acceptance
{
    using System;
    using Shouldly;
    using Xunit;

    public class PrimitiveConstruktionTests
    {
        private readonly Construktion construktion;

        public PrimitiveConstruktionTests()
        {
            construktion = new Construktion();
        }

        [Fact]
        public void strings()
        {
            var result = construktion.Construct<string>();

            result.Substring(0, 7).ShouldBe("String-");
        }

        [Fact]
        public void chars()
        {
            var result = construktion.Construct<char>();

            result.ShouldNotBeNull();
        }

        [Fact]
        public void guids()
        {
            var result = construktion.Construct<Guid>();

            result.ShouldNotBe(new Guid());
        }

        [Fact]
        public void datetime()
        {
            var result = construktion.Construct<DateTime>();

            result.ShouldNotBe(default(DateTime));
        }

        [Fact]
        public void bytes()
        {
            var result = construktion.Construct<byte>();

            result.ShouldNotBe(default(byte));
        }

        [Fact]
        public void decimals()
        {
            var result = construktion.Construct<decimal>();

            result.ShouldNotBe(default(decimal));
        }

        [Fact]
        public void doubles()
        {
            var result = construktion.Construct<double>();

            result.ShouldNotBe(default(double));
        }

        [Fact]
        public void shorts()
        {
            var result = construktion.Construct<short>();

            result.ShouldNotBe(default(short));
        }

        [Fact]
        public void ints()
        {
            var result = construktion.Construct<int>();

            result.ShouldNotBe(default(int));
        }

        [Fact]
        public void sbytes()
        {
            var result = construktion.Construct<sbyte>();

            result.ShouldNotBe(default(sbyte));
        }

        [Fact]
        public void floats()
        {
            var result = construktion.Construct<float>();

            result.ShouldNotBe(default(float));
        }

        [Fact]
        public void ushorts()
        {
            var result = construktion.Construct<ushort>();

            result.ShouldNotBe(default(ushort));
        }

        [Fact]
        public void uints()
        {
            var result = construktion.Construct<uint>();

            result.ShouldNotBe(default(uint));
        }

        [Fact]
        public void ulongs()
        {
            var result = construktion.Construct<ulong>();

            result.ShouldNotBe(default(ulong));
        }

        [Fact]
        public void single()
        {
            var result = construktion.Construct<Single>();

            result.ShouldNotBe(default(Single));
        }

        [Fact]
        public void bools()
        {
            var result = construktion.Construct<bool>();

            result.ShouldBeOneOf(true, false);
        }

        [Fact]
        public void timespans()
        {
            var result = construktion.Construct<TimeSpan>();

            result.ShouldNotBe(default(TimeSpan));
        }
    }
}