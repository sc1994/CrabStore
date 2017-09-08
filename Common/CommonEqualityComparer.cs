using System;
using System.Collections.Generic;

namespace Common
{
    public class CommonEqualityComparer<T, TV> : IEqualityComparer<T>
    {
        private readonly Func<T, TV> _keySelector;

        public CommonEqualityComparer(Func<T, TV> keySelector)
        {
            _keySelector = keySelector;
        }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<TV>.Default.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return EqualityComparer<TV>.Default.GetHashCode(_keySelector(obj));
        }
    }
}