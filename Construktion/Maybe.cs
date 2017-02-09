using System;

namespace Construktion
{
    using System.Collections;
    using System.Collections.Generic;

    //https://github.com/ploeh/Booking/blob/master/BookingDomainModel/Maybe.cs
    public class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> value;

        public Maybe()
        {
            value = new T[0];
        }

        public Maybe(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            this.value = new[] { value };
        }

        public IEnumerator<T> GetEnumerator()
        {
            return value.GetEnumerator();
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
            return value == null 
                ? new Maybe<T>()
                : new Maybe<T>(value);
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