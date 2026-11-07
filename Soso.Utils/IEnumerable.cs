using System;
using System.Collections;
using System.Collections.Generic;
using Soso.Utils.Exceptions;
using Soso.Utils.Helpers;

namespace Soso.Utils
{
    public static class IEnumerable
    {
        public static T First<T>(this IEnumerable<T> source, Func<T, bool>? predicate = null)
        {
            if (TryGetFirst(source, predicate, out T? result) == false)
            {
                throw new BadOperationException();
            }
            return result!;
        }
        public static T? FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool>? predicate = null)
        {
            TryGetFirst(source, predicate, out T? result);
            return result;
        }
        private static bool TryGetFirst<T>(IEnumerable<T> source, Func<T, bool>? predicate, out T? value)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    T? current = enumerator.Current;
                    if (predicate == null || predicate(current))
                    {
                        value = current;
                        return true;
                    }
                }
            }
            value = default(T);
            return false;
        }
        
        public static bool All<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            
            foreach (T value in source)
            {
                if (predicate(value) == false)
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            
            foreach (T value in source)
            {
                if (predicate(value))
                {
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<TS> Select<T, TS>(this IEnumerable<T> source, Func<T, TS> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            foreach (T item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<TS> SelectMany<T, TS>(this IEnumerable<T> source, Func<T, IEnumerable<TS>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            foreach (T item in source)
            {
                foreach (TS element in selector(item))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            foreach (T item in source)
            {
                if (selector(item))
                {
                    yield return item;
                }
            }
        }
        
        public static int Count<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ICollection icollection)
            {
                return icollection.Count;
            }

            if (source is ICollection<T> collection)
            {
                return collection.Count;
            }
            
            int count = 0;
            foreach (var i in source)
            {
                count++;
            }
            return count;
        }
        
        public static int Count<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            
            int count = 0;
            foreach (var i in source)
            {
                if (predicate(i))
                {
                    count++;
                }
            }
            return count;
        }
        
        public static T[] ToArray<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ICollection<T> collection)
            {
                int len = collection.Count;
                T[] array = new T[len];
                collection.CopyTo(array, 0);
                return array;
            }
            
            ExtendedArrayBuilder<T> builder = new ExtendedArrayBuilder<T>();
            builder.AppendRange(source);
            return builder.ToArray();
        }
        
        public static List<T> ToList<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is List<T> list)
            {
                return list;
            }

            return new List<T>(source);
        }
        
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is HashSet<T> list)
            {
                return list;
            }

            return new HashSet<T>(source);
        }
    }
}