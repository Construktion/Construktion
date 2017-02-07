namespace Construktion.Tests
{
    using System;
    using System.Linq;
    using Shouldly;
    using Xunit;

    public class MaybeTests
    {
        [Fact]
        public void Null_Maybe_Should_Be_Empty()
        {
            object obj = null;
            var maybe = obj.ToMaybe();

            maybe.HasValue().ShouldBeFalse();
        }

        [Fact]
        public void Accessing_The_Value_Of_An_Empty_Maybe_Should_Throw()
        {
            var maybe = new Maybe<object>();
            var maybe2 = Maybe.Empty<object>();

            Should.Throw<InvalidOperationException>(() =>
            {
                var value = maybe.Single();
            });
            Should.Throw<InvalidOperationException>(() =>
            {
                var value = maybe2.Single();
            });
        }

        [Fact]
        public void Non_Null_Call_To_ToMaybe_Should_Not_Be_Empty()
        {
            var str = "maybe";
            var maybeStr = str.ToMaybe();

            maybeStr.HasValue().ShouldBeTrue();
            maybeStr.Single().ShouldBe("maybe");
        }

        [Fact]
        public void Non_Null_Ctor_Instantiated_Maybe_Should_Not_Be_Empty()
        {
            var str = "maybe";
            var maybeStr = new Maybe<string>(str);

            maybeStr.HasValue().ShouldBeTrue();
            maybeStr.Single().ShouldBe("maybe");
        }
    }
}
