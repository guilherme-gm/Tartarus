/**
* This file is part of Tartarus Emulator.
* 
* Tartarus is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
* 
* Tartarus is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with Tartarus.  If not, see<http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections;
using System.Collections.Generic;

// Code from: http://stackoverflow.com/questions/36815062/c-sharp-hashsett-read-only-workaround
namespace Common.DataClasses.Collections
{
    public static class SetExtensionMethods
    {
        /// <summary>
        /// Returns a ReadOnlySet that is thread-safe for reading.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <returns></returns>
        public static ReadOnlySet<T> AsReadOnly<T>(this ISet<T> set)
        {
            return new ReadOnlySet<T>(set);
        }
    }

    public class ReadOnlySet<T> : IReadOnlyCollection<T>, ISet<T>
    {
        private readonly ISet<T> _set;
        public ReadOnlySet(ISet<T> set)
        {
            _set = set;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_set).GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException("Set is a read only set.");
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("Set is a read only set.");
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("Set is a read only set.");
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("Set is a read only set.");
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("Set is a read only set.");
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _set.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _set.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _set.IsProperSupersetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _set.IsProperSubsetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return _set.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return _set.SetEquals(other);
        }

        public bool Add(T item)
        {
            throw new NotSupportedException("Set is a read only set.");
        }

        public void Clear()
        {
            throw new NotSupportedException("Set is a read only set.");
        }

        public bool Contains(T item)
        {
            return _set.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _set.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException("Set is a read only set.");
        }

        public int Count
        {
            get { return _set.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }
    }
}
