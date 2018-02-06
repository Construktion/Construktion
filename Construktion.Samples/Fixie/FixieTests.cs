namespace Construktion.Samples.Fixie
{
    using Shouldly;

    public class FixieTests
    {
        public void should_fill(string name, int age)
        {
            name.ShouldNotBeNullOrEmpty();
            age.ShouldNotBe(0);
        }

        public void should_fill_complex(TestClass testClass)
        {
            testClass.Name.ShouldNotBeNullOrEmpty();
            testClass.Age.ShouldNotBe(0);
        }

        public class TestClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}