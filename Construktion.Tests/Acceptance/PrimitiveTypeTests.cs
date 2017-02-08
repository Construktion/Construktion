namespace Construktion.Tests.Acceptance
{
    using System;
    using Shouldly;
    using Xunit;

    public class PrimitiveTypeTests
    {
        private readonly Construktion _construktion;

        public PrimitiveTypeTests()
        {
            _construktion = new Construktion();
        }

        [Fact]
        public void String()
        {
            var result = _construktion.Build<string>();

            result.Substring(0, 7).ShouldBe("String-");
        }

        [Fact]
        public void Char()
        {
            var result = _construktion.Build<char>();

            result.ShouldNotBeNull();
        }

        [Fact]
        public void Guid()
        {
            var result = _construktion.Build<Guid>();

            result.ShouldNotBe(new Guid());
        }

        [Fact]
        public void Byte()
        {
            var result = _construktion.Build<byte>();

            result.ShouldNotBe(default(byte));
        }

        [Fact]
        public void Decimal()
        {
            var result = _construktion.Build<decimal>();

            result.ShouldNotBe(default(decimal));
        }

        [Fact]
        public void Double()
        {
            var result = _construktion.Build<double>();

            result.ShouldNotBe(default(double));
        }

        [Fact]
        public void Short()
        {
            var result = _construktion.Build<short>();

            result.ShouldNotBe(default(short));
        }

        [Fact]
        public void Int()
        {
            var result = _construktion.Build<int>();

            result.ShouldNotBe(default(int));
        }

        [Fact]
        public void Sbyte()
        {
            var result = _construktion.Build<sbyte>();

            result.ShouldNotBe(default(sbyte));
        }

        [Fact]
        public void Float()
        {
            var result = _construktion.Build<float>();

            result.ShouldNotBe(default(float));
        }

        [Fact]
        public void Ushort()
        {
            var result = _construktion.Build<ushort>();

            result.ShouldNotBe(default(ushort));
        }

        [Fact]
        public void Uint()
        {
            var result = _construktion.Build<uint>();

            result.ShouldNotBe(default(uint));
        }

        [Fact]
        public void Ulong()
        {
            var result = _construktion.Build<ulong>();

            result.ShouldNotBe(default(ulong));
        }

        [Fact]
        public void Bool()
        {
            var result = _construktion.Build<bool>();

            result.ShouldBeOneOf(true, false);
        }
    }
}
