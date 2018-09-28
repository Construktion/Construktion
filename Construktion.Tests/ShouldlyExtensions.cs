namespace Construktion.Tests
{
    using System;
    using System.Reflection;
    using Internal;
    using Shouldly;

    public static class ShouldlyExtensions
    {
        public static void ShouldBeNulloPropertyInfo(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.IsNulloPropertyInfo() == false)
                throw new ShouldAssertException("Expected PropertyInfo to be Nullo but was not.");
        }

        public static void ShouldBeNulloParameterInfo(this ParameterInfo parameterInfo)
        {
            if (parameterInfo.IsNulloParameterInfo() == false)
                throw new ShouldAssertException("Expected ParameterInfo to be Nullo but was not.");
        }

        public static void ShouldNotThrow<T>(Action work) where T : Exception
        {
            try
            {
                work();
            }
            catch (T)
            {
                throw new ShouldAssertException($"Exception should not be of type {typeof(T)}, but it was.");
            }
            catch (Exception)
            {
                //no op
            }
        }
    }
}