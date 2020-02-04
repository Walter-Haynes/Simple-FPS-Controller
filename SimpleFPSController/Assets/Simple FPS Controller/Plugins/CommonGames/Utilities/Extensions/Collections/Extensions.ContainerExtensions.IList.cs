using System.Collections.Generic;

using System.Linq;

using JetBrains.Annotations;

namespace CommonGames.Utilities.Extensions
{
    using static CommonGames.Utilities.Defaults;

    public static partial class ContainerExtensions
    {
        [PublicAPI]
        public static void Add<T>(this IList<T> list, in T item, in int capacity)
        {
            list.Add(item);
            
            int __removeCount = list.Count - capacity;

            if (__removeCount > 0)
            {
                RemoveRange(list, 0, __removeCount);
            }
        }

        [PublicAPI]
        public static void Enqueue<T>(this IList<T> list, in T item)
        {
            list.Add(item);
        }

        [PublicAPI]
        public static void Enqueue<T>(this IList<T> list, in T item, in int capacity)
        {
            Add(list, item, capacity);
        }

        [PublicAPI]
        public static T Dequeue<T>(this IList<T> list)
        {
            T __value = list[0];
            list.RemoveAt(0);
            return __value;
        }

        [PublicAPI]
        public static T Peek<T>(this IList<T> list)
        {
            return list[0];
        }

        [PublicAPI]
        public static void Push<T>(this IList<T> list, in T item)
        {
            list.Insert(0, item);
        }

        [PublicAPI]
        public static void Push<T>(this IList<T> list, T item, in int capacity)
        {
            list.Insert(0, item);

            int __listCount   = list.Count;
            int __removeCount = __listCount - capacity;
            int __index       = __listCount - __removeCount - 1;

            RemoveRange(list, __index, __removeCount);
        }

        [PublicAPI]
        public static T Pop<T>(this IList<T> list)
        {
            return Dequeue(list);
        }

        [PublicAPI]
        public static void RemoveRange<T>(this IList<T> list, in int index, in int count)
        {
            IEnumerable<T> __removes = list.Skip(index).Take(count);
            //list = (IList<T>)list.Except(__removes);
        }

        [PublicAPI]
        public static IEnumerable<T> Slice<T>(this IList<T> list, in int index, in int count)
        {
            return list.Skip(index).Take(count);
        }

        [PublicAPI]
        public static T Random<T>(this IList<T> list)
        {
            return list[RANDOM.Next(list.Count)];
        }

        [PublicAPI]
        public static void Shuffle<T>(this IList<T> list)
        {
            int __count = list.Count;

            while (__count > 1)
            {
                __count--;

                int __index = RANDOM.Next(__count + 1);

                T __value     = list[__index];
                list[__index] = list[__count];
                list[__count] = __value;
            }
        }
        
    }
}
