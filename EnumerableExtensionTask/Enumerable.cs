using System;
using System.Collections;
using System.Collections.Generic;

namespace EnumerableExtensionTask
{
    public static class Enumerable
    {
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
            {
                throw new ArgumentNullException($"{nameof(source)} cannot be null.");
            }

            if (predicate is null)
            {
                throw new ArgumentNullException($"{nameof(predicate)} cannot be null.");
            }

            return WhereIterator(source, predicate);

            static IEnumerable<TSource> WhereIterator(IEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                foreach (var item in source)
                {
                    if (predicate(item))
                    {
                        yield return item;
                    }
                }
            }
        }

        public static IEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> key)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return OrderByIterator(source, key);

            static IEnumerable<TSource> OrderByIterator(IEnumerable<TSource> source, Func<TSource, TKey> key)
            {
                var array = source.ToArray();
                var keysArray = new TKey[array.Length];
                int i = 0;
                foreach (var item in array)
                {
                    keysArray[i++] = key(item);
                }

                Array.Sort(keysArray, array);
                foreach (var item in array)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> key, IComparer<TKey> comparer)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return OrderByIterator(source, key, comparer);

            static IEnumerable<TSource> OrderByIterator(IEnumerable<TSource> source, Func<TSource, TKey> key, IComparer<TKey> comparer)
            {
                var array = source.ToArray();
                var keysArray = new TKey[array.Length];
                int i = 0;
                foreach (var item in array)
                {
                    keysArray[i++] = key(item);
                }

                Array.Sort(keysArray, array, comparer);
                foreach (var item in array)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> key)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return OrderBy(source, key).Reverse();
        }

        public static IEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> key, IComparer<TKey> comparer)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return OrderBy(source, key, comparer);
        }

        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return AllIterator(source, predicate);

            static bool AllIterator(IEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                foreach (var item in source)
                {
                    if (!predicate(item))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> transformer)
        {
            if (source is null)
            {
                throw new ArgumentNullException($"{nameof(source)} cannot be null.");
            }

            if (transformer is null)
            {
                throw new ArgumentNullException($"{nameof(transformer)} cannot be null.");
            }

            return SelectIterator(source, transformer);

            static IEnumerable<TResult> SelectIterator(IEnumerable<TSource> source, Func<TSource, TResult> transformer)
            {
                foreach (var item in source)
                {
                    yield return transformer(item);
                }
            }
        }

        public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
        {
            if (source is IEnumerable<TResult> typedSource)
            {
                return typedSource;
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return CastIterator(source);

            static IEnumerable<TResult> CastIterator(IEnumerable source)
            {
                foreach (object? obj in source)
                {
                    yield return (TResult)obj!;
                }
            }
        }

        public static IEnumerable<int> Range<TSource>(int start, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException($"{nameof(count)} cannot be negative.");
            }

            long max = (long)(start + count - 1);

            if (max > int.MaxValue)
            {
                throw new ArgumentException($"Max value of sequence is out of range.");
            }

            for (int i = start; i <= count + start; i++)
            {
                yield return i;
            }
        }

        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return CountIterator(source, predicate);

            static int CountIterator(IEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                int count = 0;
                foreach (var item in source)
                {
                    if (predicate(item))
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return CountIterator(source);

            static int CountIterator(IEnumerable<TSource> source)
            {
                int count = 0;
                foreach (var item in source)
                {
                    count++;
                }

                return count;
            }
        }

        public static IEnumerable<T> Reverse<T>(this IEnumerable<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ReverseIterator(source);

            static IEnumerable<T> ReverseIterator(IEnumerable<T> source)
            {
                var array = source.ToArray();

                for (int i = array.Length - 1; i >= 0; i--)
                {
                    yield return array[i];
                }
            }
        }

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ToArrayIterator(source);

            static TSource[] ToArrayIterator(IEnumerable<TSource> source)
            {
                var array = new TSource[source.Count()];
                int i = 0;
                foreach (var item in source)
                {
                    array[i++] = item;
                }

                return array;
            }
        }
    }
}
