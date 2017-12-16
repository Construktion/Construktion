namespace Construktion.Tests
{
    using Shouldly;
    using Xunit;
    using System.Text.RegularExpressions;

    public class DebuggingConstruktionTests
    {
        [Fact]
        public void should_log()
        {
            var context = new ConstruktionContext(typeof(Foo));

            new Construktion().DebuggingConstruct(context, out string log);

            log = RemoveWhiteSpace(log);
            var expected = RemoveWhiteSpace(ExpectedLog);

            log.ShouldBe(expected);
        }

        [Fact]
        public void should_log_exit_blueprint()
        {
            var context = new ConstruktionContext(typeof(Foo));

            new Construktion()
                .With(x => x.AddExitBlueprint<FooExitBlueprint>())
                .DebuggingConstruct(context, out string log);

            log = RemoveWhiteSpace(log);
            var expected = RemoveWhiteSpace(ExpectedLogWithExitBlueprint);

            log.ShouldBe(expected);
        }

        [Fact]
        public void log_should_power_through_exceptions()
        {
            var context = new ConstruktionContext(typeof(WillThrowFoo));

            var result = new Construktion().DebuggingConstruct(context, out string log);

            log.ShouldContain("Cannot construct the interface");
            log.ShouldContain("End Construktion.Tests.DebuggingConstruktionTests+WillThrowFoo");
        }

        private string RemoveWhiteSpace(string text) => Regex.Replace(text, @"\s+", "");

        public class Foo
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class FooExitBlueprint : AbstractExitBlueprint<Foo>
        {
            public override Foo Construct(Foo item, ConstruktionPipeline pipeline) => item;
        }

        public class WillThrowFoo
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public NotRegisteredInterface NotRegisteredInterface { get; set; }
        }

        public interface NotRegisteredInterface { }

        private string ExpectedLog =
            @"Start: Construktion.Tests.DebuggingConstruktionTests+Foo
Blueprint: Construktion.Blueprints.Recursive.EmptyCtorBlueprint

     Start Property: Name
     Blueprint: Construktion.Blueprints.Simple.StringBlueprint
     End Name

     Start Property: Age
     Blueprint: Construktion.Blueprints.Simple.NumericBlueprint
     End Age

End Construktion.Tests.DebuggingConstruktionTests+Foo";

        private string ExpectedLogWithExitBlueprint =
            @"Start: Construktion.Tests.DebuggingConstruktionTests+Foo
Blueprint: Construktion.Blueprints.Recursive.EmptyCtorBlueprint

     Start Property: Name
     Blueprint: Construktion.Blueprints.Simple.StringBlueprint
     End Name

     Start Property: Age
     Blueprint: Construktion.Blueprints.Simple.NumericBlueprint
     End Age

ExitBlueprint: Construktion.Tests.DebuggingConstruktionTests+FooExitBlueprint
End Construktion.Tests.DebuggingConstruktionTests+Foo";
    }
}