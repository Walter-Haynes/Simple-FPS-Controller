using System;
using System.Collections;
using System.Reflection;

using UnityEngine;
using UnityEngine.SceneManagement;

using JetBrains.Annotations;

using static CommonGames.Utilities.Defaults;

namespace CommonGames.Utilities.Extensions
{
    using SRandom = System.Random;

    public static partial class GeneralExtensions
    {
        /// <summary>
        /// Swaps values <paramref name="left"/> and <paramref name="right"/>
        /// </summary>
        public static void Swap<T>(ref T left, ref T right)
        {
            T __temp = right;
            
            right = left;
            left = __temp;
        }

        /// <summary>
        /// Do with target item.
        /// </summary>
        public static void Do<T>(this T t, params Action<T>[] actions) => actions.For(x => x(t));

        /// <summary>
        /// If bool is true, execute action.
        /// </summary>
        public static void If(this bool b, Action action)
        {
            if(b)
            {
                action();
            }
        }

        /// <summary>
        /// Executes the first action on true, the other on false.
        /// </summary>
        public static void IfElse(this bool b, Action onTrue, Action onFalse)
        {
            if(b)
            {
                onTrue();
            }
            else
            {
                onFalse();
            }
        }

        /// <summary>
        /// Execute target function i times where func returns true with overload i.
        /// </summary>
        public static void For(this int i, Func<int, bool> func, Action<int> action)
        {
            for(int j = 0; j < i; j++)
            {
                if(func(i))
                {
                    action(i);
                }
            }
        }

        /// <summary>
        /// Execute target function i times.
        /// </summary>
        public static void For(this int i, Action action)
        {
            for(int j = 0; j < i; j++)
            {
                action();
            }
        }

        /// <summary>
        /// Execute target function i times, with the index as overload.
        /// </summary>
        public static void For(this int i, Action<int> action)
        {
            for(int j = 0; j < i; j++)
            {
                action(j);
            }
        }

        /// <summary>
        /// Returns random value in range
        /// </summary>
        public static float RandomRange(float min, float max, SRandom random = null)
        {
            SRandom usedRandom = random ?? RANDOM;
            float lerp = (float)usedRandom.NextDouble();
            return Mathf.Lerp(min, max, lerp);
        }

        public static Vector3 RandomVector3(float min, float max, SRandom random = null)
        {
            SRandom usedRandom = random ?? RANDOM;
            return new Vector3(
                RandomRange(min, max, random), 
                RandomRange(min, max, random), 
                RandomRange(min, max, random));
        }

        public static Vector3Int RandomVector3Int(float min, float max, SRandom random = null)
        {
            Vector3 vector = RandomVector3(min, max, random);
            return new Vector3Int(
                Mathf.RoundToInt(vector.x), 
                Mathf.RoundToInt(vector.y), 
                Mathf.RoundToInt(vector.z));
        }

        /// <summary>
        /// Converts class to type T, instead of having to do (MyClass as T).
        /// </summary>
        public static T Convert<T>(this object obj) where T : class => obj as T;

        public static void Stop(this Coroutine coroutine, MonoBehaviour behaviour)
        {
            if(coroutine != null)
                behaviour.StopCoroutine(coroutine);
        }

        /// <summary>
        /// The next time a scene is loaded, execute this function.
        /// </summary>
        public static void AddInstanceToSceneLoaded(Action action)
        {
            void __Wrapper(Scene scene, LoadSceneMode mode)
            {
                action();
                SceneManager.sceneLoaded -= __Wrapper;
            }

            SceneManager.sceneLoaded += __Wrapper;
        }

        /// <summary>
        /// Shortcut for Unity's SetActive.
        /// </summary>
        public static void SetActive(this MonoBehaviour behaviour, bool enabled = true)
            => behaviour.gameObject.SetActive(enabled);

        /// <summary>
        /// Converts float into a string that visualizes the time digitally.
        /// </summary>
        public static string ToTimeString(this float seconds)
        {
            TimeSpan result = TimeSpan.FromSeconds(seconds);
            return string.Format($"{result.Hours}:{result.Minutes}:{result.Seconds}");
        }

        #region Logging

        #region Logs

        /// <summary> Logs a string value to the console. </summary>
        [PublicAPI]
        public static void Log(this string message)
        {
            CommonGames.Utilities.CGTK.CGDebug.Log(message);
        }
        
        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void Log<T>(this T obj)
        {
            CommonGames.Utilities.CGTK.CGDebug.Log(obj.ToString());
        }
        
        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void Log<T>(this T obj, in string message)
        {
            try
            {
                CommonGames.Utilities.CGTK.CGDebug.LogFormat(message, obj.ToString());
            }
            catch
            {
                CommonGames.Utilities.CGTK.CGDebug.LogError("Debug Logging Went Wrong");
            }
        }

        #endregion

        #region LogWarnings

        /// <summary> Logs a string value to the console. </summary>
        [PublicAPI]
        public static void LogWarning(this string message)
            => CommonGames.Utilities.CGTK.CGDebug.LogWarning(message);

        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void LogWarning<T>(this T obj)
            => CommonGames.Utilities.CGTK.CGDebug.LogWarning(obj.ToString());

        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void LogWarning<T>(this T obj, in string message)
        {
            try
            {
                CommonGames.Utilities.CGTK.CGDebug.LogWarningFormat(message, obj.ToString());
            }
            catch
            {
                CommonGames.Utilities.CGTK.CGDebug.LogError("Debug Logging Went Wrong");
            }
        }

        #endregion

        #region LogErrors

        /// <summary> Logs a string value to the console. </summary>
        [PublicAPI]
        public static void LogError(this string message)
            => CommonGames.Utilities.CGTK.CGDebug.LogError(message);

        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void LogError<T>(this T obj)
            => CommonGames.Utilities.CGTK.CGDebug.LogError(obj.ToString());

        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void LogError<T>(this T obj, in string message)
        {
            try
            {
                CommonGames.Utilities.CGTK.CGDebug.LogErrorFormat(message, obj.ToString());
            }
            catch
            {
                CommonGames.Utilities.CGTK.CGDebug.LogError("Debug Logging Went Wrong");
            }
        }

        #endregion
        
        #endregion

        #region GetIfNull
        
        [PublicAPI]
        public static T GetIfNull<T>(this T obj, in Func<T> getMethod) where T : Component
            => obj ? obj : getMethod?.Invoke();
        
        #region GameObject Context
        
        [PublicAPI]
        public static T GetIfNull<T>(this T obj, in GameObject context) where T : Component
            => obj ? obj : context.GetComponent<T>();
        
        [PublicAPI]
        public static T GetInChildrenIfNull<T>(this T obj, in GameObject context) where T : Component
            => obj ? obj : context.GetComponentInChildren<T>();
        
        [PublicAPI]
        public static T TryGetIfNull<T>(this T obj, in GameObject context) where T : Component 
            => (obj != null)? obj : context.TryGetComponent(out T __returnedObject) ? __returnedObject : null;

        [PublicAPI]
        public static T TryGetInChildrenIfNull<T>(this T obj, in GameObject context) where T : Component
            => (obj != null)? obj : context.TryGetComponent(out T __returnedObject) ? __returnedObject : context.GetComponentInChildren<T>();

        [PublicAPI]
        public static T TryGetInParentIfNull<T>(this T obj, in GameObject context) where T : Component
            => (obj != null)? obj : context.TryGetComponent(out T __returnedObject) ? __returnedObject : context.GetComponentInParent<T>();

        [PublicAPI]
        public static T TryGetInParentOrChildrenIfNull<T>(this T obj, in GameObject context) where T : Component
        {
            if(obj != null) return obj;

            if(context.TryGetComponent(out T __triedComponent)) return __triedComponent;

            T __componentInParent = context.GetComponentInParent<T>();
            
            return __componentInParent ? __componentInParent : context.GetComponentInParent<T>();
        }

        #endregion

        #region Component Context

        [PublicAPI]
        public static T GetIfNull<T>(this T obj, in Component context) where T : Component
            => GetIfNull(obj, context.gameObject);

        [PublicAPI]
        public static T GetInChildrenIfNull<T>(this T obj, in Component context) where T : Component
            => GetInChildrenIfNull(obj, context.gameObject);

        [PublicAPI]
        public static T TryGetIfNull<T>(this T obj, in Component context) where T : Component
            => TryGetIfNull(obj, context.gameObject);

        [PublicAPI]
        public static T TryGetInChildrenIfNull<T>(this T obj, in Component context) where T : Component
            => TryGetInChildrenIfNull(obj, context.gameObject);

        [PublicAPI]
        public static T TryGetInParentIfNull<T>(this T obj, in Component context) where T : Component
            => TryGetInParentIfNull(obj, context.gameObject);

        [PublicAPI]
        public static T TryGetInParentOrChildrenIfNull<T>(this T obj, in Component context) where T : Component
            => TryGetInParentOrChildrenIfNull(obj, context.gameObject);

        #endregion
        
        #endregion
        
        #region GetComponent

        #region GameObject Context

        [PublicAPI]
        public static T TryGetInChildren<T>(this GameObject context) where T : Component
            => context.TryGetComponent(out T __returnedObject) ? __returnedObject : context.GetComponentInChildren<T>();
        
        [PublicAPI]
        public static T TryGetInParent<T>(this GameObject context) where T : Component
            => context.TryGetComponent(out T __returnedObject) ? __returnedObject : context.GetComponentInParent<T>();
        
        [PublicAPI]
        public static T TryGetInParentOrChildren<T>(this GameObject context) where T : Component
        {
            if(context.TryGetComponent(out T __triedComponent)) return __triedComponent;

            T __componentInParent = context.GetComponentInParent<T>();
            
            return __componentInParent ? __componentInParent : context.GetComponentInParent<T>();
        }

        #endregion

        #region Component Context

        [PublicAPI]
        public static T TryGetInChildren<T>(this Component context) where T : Component
            => TryGetInChildren<T>(context.gameObject);

        [PublicAPI]
        public static T TryGetInParent<T>(this Component context) where T : Component
            => TryGetInParent<T>(context.gameObject);

        [PublicAPI]
        public static T TryGetInParentOrChildren<T>(this Component context) where T : Component
            => TryGetInParentOrChildren<T>(context.gameObject);

        #endregion

        #endregion
    }

    public class ListArray<T>
    {
        public T[] Array { get; private set; }

        public int Length => Array.Length;
        public int Count { get; private set; }

        public T this[int index]
        {
            get => Array[index];
            set => Array[index] = value;
        }

        public ListArray(int length) => Array = new T[length];

        public void Add(T item)
        {
            Array[Length] = item;
            Count++;
        }

        public void Remove(T item, bool removeAll = false)
        {
            for (int i = 0; i < Length; i++)
                if(Array[i].Equals(item))
                {
                    RemoveAt(i);
                    if(!removeAll)
                        return;
                }
        }

        public void RemoveAt(int index)
        {
            int length = Length - index - 1;

            for (int i = 0; i < length; i++)
            {
                int current = index + length - i, next = current - 1;

                T item = Array[current];
                ref T nextItem = ref Array[next];

                Array[current] = nextItem;
                nextItem = item;
            }
        }

        public static implicit operator T[](ListArray<T> listArray) => listArray.Array;
        public static implicit operator ListArray<T>(T[] array) => new ListArray<T>(array.Length); 
    }

    /// <summary>
    /// An ragged array that decrements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FactoriumArray<T>
    {
        public T[] this[int i]
        {
            get => data[i];
            set => data[i] = value;
        }

        public T this[int i, int j]
        {
            get => data[i][j];
            set => data[i][j] = value;
        }

        public int Length => data.Length;

        private T[][] data;

        public FactoriumArray(int length)
        {
            data = new T[length][];
            for (int i = 0; i < length; i++)
                data[i] = new T[length - i];
        }

        public void Reset()
        {
            int length = data.Length;
            for (int i = 0; i < length; i++)
                for (int j = 0; j < length - i; j++)
                    data[i][j] = default;
        }
    }

    /// <summary>
    /// A container for data that you usually use together when lerping.
    /// </summary>
    [Serializable]
    public struct Lerpable
    {
        /// <summary>
        /// Returns Mathf.lerp(min, max, curve.Evaluate(f));
        /// </summary>
        public float Get(in float f) => Mathf.Lerp(min, max, curve.Evaluate(f));

        [SerializeField]
        private float min, max;
        [SerializeField]
        private AnimationCurve curve;
    }

    /// <summary>
    /// Used to invoke function with a return type by name.
    /// </summary>
    [Serializable]
    public class FuncEvent
    {
        public MonoBehaviour behaviour;
        public string name;

        private MethodInfo method = null;

        public T Invoke<T>()
        {
            if(method != null) return (T)method.Invoke(behaviour, null);
            
            Type type = behaviour.GetType();
            method = type.GetMethod(name);
            return (T)method.Invoke(behaviour, null);
        }
    }
}