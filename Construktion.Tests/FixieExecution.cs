namespace Construktion.Tests
{
    using Fixie;

    public class FixieExecution : Execution
    {
        public void Execute(TestClass testClass)
        {
            testClass.RunCases(@case =>
            {
                var instance = testClass.Construct();

                @case.Execute(instance);

                instance.Dispose();
            });
        }
    }
}