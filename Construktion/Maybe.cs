namespace Construktion
{
    using System.Collections.Generic;
    using System.Collections;

    //https://github.com/ploeh/Booking/blob/master/BookingDomainModel/Maybe.cs
    public class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> values;

        public Maybe()
        {
            values = new T[0];
        }

        public Maybe(T value)
        {
            values = value == null
                ? new T[0]
                : new[] { value };
        }

        public IEnumerator<T> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class Maybe
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> Empty<T>()
        {
            return new Maybe<T>();
        }

        public static bool HasValue<T>(this Maybe<T> maybe)
        {
            return maybe.GetEnumerator().MoveNext();
        }
    }
}
