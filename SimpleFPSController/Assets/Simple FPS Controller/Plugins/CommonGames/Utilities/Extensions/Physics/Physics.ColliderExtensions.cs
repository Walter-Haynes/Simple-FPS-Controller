using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonGames.Utilities.Defaults;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Physics
    {
        #region ToggleInTime

        public static void ToggleInTime(this Collider obj, bool state, float time = DEFAULT_TOGGLE_TIME)
            => CoroutineHandler.StartCoroutine(ToggleInTimeCoroutine(obj, state, time.ToAbs()));
		
        private static IEnumerator ToggleInTimeCoroutine(Collider obj, bool state, float time = DEFAULT_TOGGLE_TIME)
        {
            yield return (time.Approximately(default) ? DefaultWait : new WaitForSeconds(time));
			
            obj.enabled = state;
        }
		
        #endregion
    }
}