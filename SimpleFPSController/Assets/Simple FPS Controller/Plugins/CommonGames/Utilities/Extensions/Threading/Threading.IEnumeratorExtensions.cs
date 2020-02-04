using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Threading
    {
        public static void ExecuteCompletely(this IEnumerator enumerator)
        {
            while (enumerator.MoveNext()){}
        }

        public static void ForEach<T>(this IEnumerable<T> list, System.Action<T> action)
        {
            if(action == null)
            {
                Debug.LogError("<color=red> No Action Was Sent with </color>");
                return;
            }
            
            foreach(T __elem in list)
            {
                action(__elem);
            }
        }

        public static void CoroutinetoEnd(this IEnumerator coroutine)
        {
            Stack<IEnumerator> stack = new Stack<IEnumerator>();

            stack.Push(coroutine);
            while (stack.Count > 0)
            {
                if (stack.Peek().MoveNext())
                {
                    if (stack.Peek().Current is IEnumerator nested)
                    {
                        stack.Push(nested);
                    }
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        public static T Result<T>(this IEnumerator target) where T : class
        {
            return target.Current as T;
        }

        public static T ResultValueType<T>(this IEnumerator target) where T : struct
        {
            //System.Diagnostics.Debug.Assert(target.Current != null, "target.Current != null");
            System.Diagnostics.Debug.Assert(target.Current != null, "target.Current != null");
            return (T) target.Current;
        }
    }
}