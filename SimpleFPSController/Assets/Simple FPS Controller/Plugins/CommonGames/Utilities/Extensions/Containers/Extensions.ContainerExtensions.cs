using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using JetBrains.Annotations;

using static CommonGames.Utilities.Defaults;

using SRandom = System.Random;

namespace CommonGames.Utilities.Extensions
{
    [PublicAPI]
    public static partial class ContainerExtensions
    {
        /// <summary>
        /// Gets the count of the items in a list that aren't null/
        /// </summary>
        [PublicAPI]
        public static int CountNonNull<T>(this List<T> list) where T : class
        {
            int __normalCount = list.Count;
            Debug.Log($"Normal Count = {__normalCount}");
            int __nonNullCount = -1;

            for(int __index = 0; __index < __normalCount; __index++)
            {
                T __target = list[__index];

                if(__target != null) __nonNullCount++;
            }
            
            Debug.Log($"Non-Null Count = {list.Count}");

            return __nonNullCount;
        }
        
        /// <summary>
        /// Get an item which returns true when overloaded in target function.
        /// </summary>
        [PublicAPI]
        public static T Get<T>(this List<T> list, Func<T, bool> func)
        {
            foreach (T t in list)
                if (func(t))
                    return t;
            return default;
        }

        /// <summary>
        /// Get index that returns true with with target function.
        /// </summary>
        [PublicAPI]
        public static int GetIndex<T>(this List<T> list, Func<T, bool> func)
        {
            int __length = list.Count;
            
            for (int __i = 0; __i < __length; __i++)
            {
                if (func(list[__i])) return __i;
            }

            return -1;
        }

        /// <summary>
        /// Get index that returns true with with target function.
        /// </summary>
        [PublicAPI]
        public static int GetIndex<T>(this T[] array, Func<T, bool> func)
        {
            int __length = array.Length;
            
            for (int __i = 0; __i < __length; __i++)
            {
                if (func(array[__i])) return __i;
            }
            
            return -1;
        }
        
        /// <summary>
        /// Get all items which returns true when overloaded in target function.
        /// </summary>
        [PublicAPI]
        public static List<T> GetMultiple<T>(this List<T> list, Func<T, bool> func, List<T> other = null)
        {
            if (other == null)
                other = new List<T>();
            foreach (T t in list)
                if (func(t))
                    other.Add(t);
            return other;
        }

        /// <summary>
        /// Get an item which returns true when overloaded in target function.
        /// </summary>
        [PublicAPI]
        public static T Get<T>(this T[] array, Func<T, bool> func)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
                if (func(array[i]))
                {
                    return array[i];
                }

            return default;
        }

        /// <summary>
        /// Get all items which returns true when overloaded in target function.
        /// </summary>
        [PublicAPI]
        public static List<T> GetMultiple<T>(this T[] array, Func<T, bool> func, List<T> other = null)
        {
            if (other == null)
                other = new List<T>();
            int length = array.Length;
            for (int i = 0; i < length; i++)
                if (func(array[i]))
                    other.Add(array[i]);
            return other;
        }

        /// <summary>
        /// Order list by function.
        /// </summary>
        [PublicAPI]
        public static void CGOrderBy<T>(this List<T> list, Func<T, int> func)
        {
            int listCount = list.Count, index;
            T current, other;
            int currentValue;

            for (int i = 1; i < listCount; i++)
            {
                index = i;
                current = list[index];
                currentValue = func(current);

                while (index > 0)
                {
                    other = list[index - 1];
                    if (func(other) > currentValue) break;
                    
                    list[index] = other;
                    index--;
                    list[index] = current;
                }
            }
        }

        /// <summary>
        /// First order the list by the speciate function, then order it based on the normal function.
        /// </summary>
        [PublicAPI]
        public static void CGOrderBySpeciated<T>(this List<T> list, Func<T, int> func, Func<T, int> speciate)
        {
            int listCount = list.Count, index;
            T current, other;
            int currentValue;
            int currentSpeciateValue;

            for (int i = 1; i < listCount; i++)
            {
                index = i;
                current = list[index];
                currentValue = func(current);
                currentSpeciateValue = speciate(current);

                while (index > 0)
                {
                    other = list[index - 1];
                    if (currentSpeciateValue > speciate(other))
                        break;
                    if (currentSpeciateValue == speciate(other))
                        if (currentValue > func(other))
                            break;
                    list[index] = other;
                    index--;
                    list[index] = current;
                }
            }
        }

        /// <summary>
        /// Add values of other list to target list.
        /// </summary>
        [PublicAPI]
        public static void CGAdd<T>(this List<T> list, List<T> other) => other.For(x => list.Add(x));

        /// <summary>
        /// Add values of other array to target list.
        /// </summary>
        [PublicAPI]
        public static void CGAdd<T>(this List<T> list, T[] other) => other.For(x => list.Add(x));

        /// <summary>
        /// Add values of other list to target list that return true when overloaded with target function.
        /// </summary>
        [PublicAPI]
        public static void CGAdd<T>(this List<T> list, List<T> other, Func<T, bool> func)
        {
            foreach (T t in other)
                if (func(t))
                    list.Add(t);
        }

        /// <summary>
        /// Add values of other list to target array that return true when overloaded with target function.
        /// </summary>
        public static void CGAdd<T>(this List<T> list, T[] other, Func<T, bool> func)
        {
            foreach (T t in other)
                if (func(t))
                    list.Add(t);
        }

        public static void CGAddAtLast<T>(this List<T> list, in T value) where T : class
        {
            int __index = list.Count - 1;

            //Debug.Log($"Count = {list.Count}");
            
            if(list.Count <= 0) goto  ADD; //If the list is empty, *add* the value.

            //Debug.Log($"index = {__index}");
            
            if(!list[__index].Equals(null)) goto ADD; //If the item is empty, *insert* the value. Else, *add* the value.

            //Debug.Log($"Is in fact NULL");
            
            list[__index] = value;

            return;
            
            ADD:
            
            /*
            if(list.Count > 0)
            { 
                Debug.Log($"IndexValue is not null = {!list[__index].Equals(null)}");   
            }
            */

            list.Add(item: value);
            
            //Debug.Log($"New Count = {list.Count}");
        }

        /// <summary>
        /// Check if target list contains a value that returns true when overloaded with target function.
        /// </summary>
        [PublicAPI]
        public static bool CGContains<T>(this List<T> list, Func<T, bool> func)
        {
            foreach (T t in list)
                if (func(t))
                    return true;
            return false;
        }
        
        /// <summary> Looped indexer getter. </summary>
        [PublicAPI]
        public static T GetLooped<T>(this IList<T> list, int index)
        {
            if (index < 0)
            {
                index = list.Count % index;
            }    
            if (index >= list.Count)
            {
                index %= list.Count;
                
                //index = remainder of (index & list.count)
                //So.. If list length = 10, and you try to access index 12, you'll get a remainder of 2.
                //This is the index we will access instead of the one you were trying  to reach. 
            }
            return list[index];
        }

        /// <summary> Looped indexer setter, allows out of bounds indices, ignores IList.IsReadOnly </summary>
        [PublicAPI]
        public static void SetLooped<T>(this IList<T> list, int index, T value)
        {
            while (index < 0)
            {
                index += list.Count;
            }
            if (index >= list.Count)
            {
                index %= list.Count;
            }
            list[index] = value;
        }

        /// <summary>
        /// Get first value from list.
        /// </summary>
        [PublicAPI]
        public static T First<T>(this List<T> list) => list[0];

        /// <summary>
        /// Get last value from list.
        /// </summary>
        [PublicAPI]
        public static T Last<T>(this List<T> list) => list[list.Count - 1];

        /// <summary>
        /// Get middle value from list.
        /// </summary>
        [PublicAPI]
        public static T Middle<T>(this List<T> list) => list[Mathf.CeilToInt(list.Count / 2) - 1];

        /// <summary>
        /// Get middle value from list.
        /// </summary>
        [PublicAPI]
        public static T Middle<T>(this T[] array) => array[Mathf.CeilToInt(array.Length / 2) - 1];

        /// <summary>
        /// Get first value from list and remove it from said list.
        /// </summary>
        [PublicAPI]
        public static T Pop<T>(this List<T> list)
        {
            T item = list.First();
            list.RemoveAt(0);
            return item;
        }
        
        [PublicAPI]
        public static void Swap<T>
        (
            this IList<T> list,
            in int firstIndex,
            in int secondIndex
        ) 
        {
            if(list == null)
            {
                return;
            }
            
            if(!(firstIndex >= 0 && firstIndex < list.Count))
            {
                Debug.LogError("Something went wrong while swapping two instances in a list...");
                return;
            }
            
            if(!(secondIndex >= 0 && secondIndex < list.Count))
            {
                Debug.LogError("Something went wrong while swapping two instances in a list...");
                return;
            }
            
            if (firstIndex == secondIndex) 
            {
                return;
            }

            T __temp = list[firstIndex];
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = __temp;
        }

        /// <summary>
        /// Execute a function for each item in target list.
        /// </summary>
        [PublicAPI]
        public static void For<T>(this List<T> list, Action<T> action)
        {
            foreach (T item in list)
                action(item);
        }

        /// <summary>
        /// Execute a function for each item in target array.
        /// </summary>
        [PublicAPI]
        public static void For<T>(this T[] array, Action<T, int> action)
        {
            int __length = array.Length;
            for (int i = 0; i < __length; i++)
                action(array[i], i);
        }

        /// <summary>
        /// Execute a function for each item in target list.
        /// </summary>
        [PublicAPI]
        public static void For<T>(this List<T> list, Action<T, int> action)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
                action(list[i], i);
        }
        
        /// <summary>
        /// Execute a function for each item in target list.
        /// </summary>
        [PublicAPI]
        public static void SafeFor<T>(this List<T> list, Action<T, int> action)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i] != null)
                {
                    action(list[i], i);
                }
            }
        }


        /// <summary>
        /// Execute a function for each item in target array.
        /// </summary>
        [PublicAPI]
        public static void For<T>(this T[] array, Action<T> action)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
                action(array[i]);
        }
        
        /// <summary>
        /// Execute a function for each item in target array safely (it does expensive null-checks).
        /// </summary>
        [PublicAPI]
        public static void SafeFor<T>(this T[] array, Action<T> action)
        {
            int length = array.Length;
            
            if(length < 0){return;}
            
            for (int i = 0; i < length; i++)
            {
                if (array[i] != null)
                {
                    action(array[i]);
                }
            }
        }

        [PublicAPI]
        public static void CGRemoveAll<T>(this List<T> list, Func<T, bool> func)
        {
            int i = list.Count;
            while(i-- > 0)
            {
                if(func(list[i]))
                {
                    list.RemoveAt(i);   
                }
            }
        }

        /// <summary>
        /// Check if target array contains a value that returns true when overloaded with target function.
        /// </summary>
        [PublicAPI]
        public static bool Contains<T>(this List<T> list, T value)
        {
            foreach (T item in list)
                if (list.Equals(value))
                    return true;
            return false;
        }

        /// <summary>
        /// Check if target array contains a value that returns true when overloaded with target function.
        /// </summary>
        [PublicAPI]
        public static bool Contains<T>(this T[] array, T value)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
                if (array[i].Equals(value))
                    return true;
            return false;
        }

        [PublicAPI]
        public static void For<T>(this T[,] grid, Action<T, int, int> action)
        {
            int lengthX = grid.GetLength(0),
                lengthY = grid.GetLength(1);
            for (int x = 0; x < lengthX; x++)
                for (int y = 0; y < lengthY; y++)
                    action(grid[x, y], x, y);
        }

        [PublicAPI]
        public static void For<T>(this T[,] grid, Action<T> action)
        {
            int lengthX = grid.GetLength(0),
                lengthY = grid.GetLength(1);
            for (int x = 0; x < lengthX; x++)
                for (int y = 0; y < lengthY; y++)
                    action(grid[x, y]);
        }

        /// <summary>
        /// Get the index of the highest returning value when overloaded in target function.
        /// </summary>
        [PublicAPI]
        public static int GetTopIndex<T>(this T[] array, Func<T, double> func)
        {
            int index = -1, length = array.Length;
            double score = double.MinValue;
            for (int i = 0; i < length; i++)
            {
                double currentScore = func(array[i]);
                if (currentScore < score)
                    continue;
                score = currentScore;
                index = i;
            }
            return index;
        }

        /// <summary>
        /// Get the index of the highest returning value when overloaded in target function.
        /// </summary>
        [PublicAPI]
        public static int GetTopIndex<T>(List<T> list, Func<T, double> func)
        {
            int index = -1, length = list.Count;
            double score = double.MinValue;
            for (int i = 0; i < length; i++)
            {
                double currentScore = func(list[i]);
                if (currentScore < score)
                    continue;
                score = currentScore;
                index = i;
            }
            return index;
        }

        /// <summary>
        /// Get a random item from target list.
        /// </summary>
        [PublicAPI]
        public static T GetRandom<T>(this List<T> list, SRandom random = null) => list[(random ?? RANDOM).Next(0, list.Count - 1)];

        /// <summary>
        /// Get a random item from target array.
        /// </summary>
        [PublicAPI]
        public static T GetRandom<T>(this T[] array, SRandom random = null) => array[(random ?? RANDOM).Next(0, array.Length - 1)];
    }
}