using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Rendering
    {
        /// <summary>
        /// Checks if AnimatorComponent has a parameter name of parameterName
        /// </summary>
        /// <returns></returns>
        public static bool HasParameter(this Animator animator, string parameterName)
        {
            return Enumerable.Any(animator.parameters, parameter => parameter.name == parameterName);
        }
    }
}