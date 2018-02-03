namespace Construktion.Tests.Acceptance
{
    using System;
    using Shouldly;

    public class PrimitiveConstruktionTests
    {
        private readonly Construktion construktion;

        public PrimitiveConstruktionTests()
        {
            construktion = new Construktion();
        }

        public void strings()
        {
            var result = construktion.Construct<string>();

            result.Substring(0, 7).ShouldBe("String-");
        }

        public void chars()
        {
            var result = construktion.Construct<char>();

            result.ShouldNotBeNull();
        }

        public void guids()
        {
            var result = construktion.Construct<Guid>();

            result.ShouldNotBe(new Guid());
        }

        public void datetime()
        {
            var result = construktion.Construct<DateTime>();

            result.ShouldNotBe(default(DateTime));
        }

        public void bytes()
        {
            var result = construktion.Construct<byte>();

            result.ShouldNotBe(default(byte));
        }

        public void decimals()
        {
            var result = construktion.Construct<decimal>();

            result.ShouldNotBe(default(decimal));
        }

        public void doubles()
        {
            var result = construktion.Construct<double>();

            result.ShouldNotBe(default(double));
        }

        public void shorts()
        {
            var result = construktion.Construct<short>();

            result.ShouldNotBe(default(short));
        }

        public void ints()
        {
            var result = construktion.Construct<int>();

            result.ShouldNotBe(default(int));
        }

        public void sbytes()
        {
            var result = construktion.Construct<sbyte>();

            result.ShouldNotBe(default(sbyte));
        }

        public void floats()
        {
            var result = construktion.Construct<float>();

            result.ShouldNotBe(default(float));
        }

        public void ushorts()
        {
            var result = construktion.Construct<ushort>();

            result.ShouldNotBe(default(ushort));
        }

        public void uints()
        {
            var result = construktion.Construct<uint>();

            result.ShouldNotBe(default(uint));
        }

        public void ulongs()
        {
            var result = construktion.Construct<ulong>();

            result.ShouldNotBe(default(ulong));
        }

        public void single()
        {
            var result = construktion.Construct<Single>();

            result.ShouldNotBe(default(Single));
        }

        public void bools()
        {
            var result = construktion.Construct<bool>();

            result.ShouldBeOneOf(true, false);
        }

        public void timespans()
        {
            var result = construktion.Construct<TimeSpan>();

            result.ShouldNotBe(default(TimeSpan));
        }
    }
}