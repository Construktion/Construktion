namespace Construktion.Tests
{
    using System.Reflection;
    using Internal;
    using Shouldly;

    public static class ShouldlyExtensions
    {
        public static void ShouldBeNulloPropertyInfo(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.IsNulloPropertyInfo() == false)
                throw new ShouldAssertException("Excepted PropertyInfo to be Nullo but was not.");
        }

        public static void ShouldBeNulloParameterInfo(this ParameterInfo parameterInfo)
        {
            if (parameterInfo.IsNulloParameterInfo() == false)
                throw new ShouldAssertException("Excepted ParameterInfo to be Nullo but was not.");
        }
    }
}