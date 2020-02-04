using System;
using System.Collections;
using System.Reflection;

using UnityEngine;

using static CommonGames.Utilities.Defaults;

namespace CommonGames.Utilities.Extensions
{
	public static partial class GeneralExtensions
	{
		#region ToggleInTime

		public static void EnableInTime(this Behaviour obj, bool state, float time = DEFAULT_TOGGLE_TIME)
		{
			IEnumerator __EnableInTimeCoroutine()
			{
				yield return (time.Approximately(default) ? DefaultWait : new WaitForSeconds(time));
			
				obj.enabled = state;
			}
			
			CoroutineHandler.StartCoroutine(__EnableInTimeCoroutine());	
		}

		#endregion
	}
}