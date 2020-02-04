using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

using static CommonGames.Utilities.Defaults;

namespace CommonGames.Utilities.Extensions
{
	public static partial class Rendering
	{
		#region ToggleInTime

		public static void ToggleInTime(this Renderer obj, bool state, float time = DEFAULT_TOGGLE_TIME)
			=> CoroutineHandler.StartCoroutine(ToggleInTimeCoroutine(obj, state, time.ToAbs()));
		
		private static IEnumerator ToggleInTimeCoroutine(Renderer obj, bool state, float time = DEFAULT_TOGGLE_TIME)
		{
			yield return (time.Approximately(default) ? DefaultWait : new WaitForSeconds(time));
			
			obj.enabled = state;
		}
		
		#endregion
		
	}
}