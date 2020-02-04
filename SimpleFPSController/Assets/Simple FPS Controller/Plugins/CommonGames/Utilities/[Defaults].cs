using System;
using System.Collections;
using System.Reflection;

using UnityEngine;

using JetBrains.Annotations;

namespace CommonGames.Utilities
{
    using Random = System.Random;
    
    public static partial class Defaults
    {
        
        #region Alphanumerics

        /// <summary> Lowercase letters from a to z </summary>
        [PublicAPI]
        public const string LOWERCASE_LETTERS = "abcdefghijklmnopqrstuvwxyz";
        
        /// <summary> Uppercase letters from A to Z </summary>
        [PublicAPI]
        public const string UPPERCASE_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        /// <summary> All <see cref="LOWERCASE_LETTERS"/> and <see cref="UPPERCASE_LETTERS"/>. </summary>
        [PublicAPI]
        public const string LETTERS = LOWERCASE_LETTERS + UPPERCASE_LETTERS;
        
        /// <summary> Digits from 0 to 9 </summary>
        [PublicAPI]
        public const string DIGITS = "0123456789";
        
        /// <summary> All <see cref="LETTERS"/> and <see cref="DIGITS"/> </summary>
        [PublicAPI]
        public const string ALPHANUMERICS = LETTERS + DIGITS;

        #endregion

        #region Static MonoBehaviours

        [PublicAPI]
        public sealed class StaticMonoBehaviourClass : EnsuredSingleton<StaticMonoBehaviourClass>{}

        /// <summary> A static <see cref="MonoBehaviour"/>. It's an <see cref="EnsuredSingleton"/> so it spawns one if there isn't one in the scene). </summary>
        [PublicAPI]
        public static StaticMonoBehaviourClass StaticMonoBehaviour => StaticMonoBehaviourClass.Instance;
        
        /// <summary> A static <see cref="MonoBehaviour"/> for handling Coroutines. </summary>
        [PublicAPI]
        public static StaticMonoBehaviourClass CoroutineHandler => StaticMonoBehaviour;

        #endregion

        #region Default WaitForSeconds

        /// <summary> The default wait time for WaitForSeconds' </summary>
        [PublicAPI]
        public const float DEFAULT_TOGGLE_TIME = 0f;
        
        /// <summary> A static WaitForSeconds, used when functions don't get one send to them. </summary>
        /// <remarks> ZERO SECONDS BY DEFAULT, because when you don't send on with, we can assume you don't want any wait time. </remarks>
        [PublicAPI]
        public static readonly WaitForSeconds DefaultWait = new WaitForSeconds(DEFAULT_TOGGLE_TIME);

        #endregion

        #region Math Constants
        
        //TODO -Walter- Add TAU, it's better but no-one is taught about it.

        ///// <summary> Square root of 0.5 </summary>
        //[PublicAPI]
        //public const float SQRT_05 = 0.7071067811865475244f;
        
        /// <summary> Square root of 2. </summary>
        [PublicAPI]
        public const float SQRT_2 = 1.4142135623730950488f;
        
        /// <summary> Square root of 5. </summary>
        [PublicAPI]
        public const float SQRT_5 = 2.2360679774997896964f;
        
        /// <summary> Golden angle in radians. </summary>
        [PublicAPI]
        public const float GOLDEN_ANGLE = Mathf.PI * (3 - SQRT_5);

        /// <summary> PI times 2. </summary>
        [PublicAPI]
        public const float PI_2 = Mathf.PI * 2;
        
        /// <summary> PI times 4. </summary>
        [PublicAPI]
        public const float PI_4 = PI_2* 2;

        /// <summary> PI times 8. </summary>
        [PublicAPI]
        public const float PI_8 = PI_4 * 2;
        
        /// <summary> PI times 16. </summary>
        [PublicAPI]
        public const float PI_16 = PI_8 * 2;

        #endregion

        #region Default Random

        /// <summary> A static System.Random, used when functions don't get one send to them. </summary>
        [PublicAPI]
        // ReSharper disable once InconsistentNaming
        public static Random RANDOM { get; } = new Random();

        #endregion
        
    }
}
