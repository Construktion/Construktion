using System;
using Shouldly;

namespace Construktion.Tests
{

    /// <summary>
    /// Assert that an action throws an Exception. Shouldly's Should.Throw doesn't catch the exception and 
    /// interrupts test execution while debugging. This allows test runs to execute without interruption while debugging.
    /// Stolen from Marten: https://github.com/JasperFx/marten/blob/master/src/Marten.Testing/SpecificationExtensions.cs 
    /// </summary>
    public static class Exception<T> where T : Exception
    {
        public static T ShouldBeThrownBy(Action action)
        {
            T exception = null;

            try
            {
                action();
            }
            catch (Exception e)
            {
                exception = e.ShouldBeOfType<T>();
            }

            exception.ShouldNotBeNull("An exception was expected, but not thrown by the given action.");

            return exception;
        }
    }
}