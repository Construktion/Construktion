namespace Construktion.Tests
{
    using Shouldly;
    using Xunit;

    public class ComplexTypeTests
    {
        private readonly Construktion _construktion;

        public ComplexTypeTests()
        {
            _construktion = new Construktion();
        }

        [Fact]
        public void Enum()
        {
            var result = _construktion.Build<TestResult>();

            result.ShouldBeOneOf(TestResult.Pass, TestResult.Fail);
        }


        enum TestResult
        {
            Pass,
            Fail
        }
    }
}
