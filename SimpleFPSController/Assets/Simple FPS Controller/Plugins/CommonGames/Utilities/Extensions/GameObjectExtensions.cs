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
		
		public static void ToggleInTime(this GameObject obj, bool state, float time = DEFAULT_TOGGLE_TIME)
			=> CoroutineHandler.StartCoroutine(ToggleInTimeCoroutine(obj, state, time.ToAbs()));
		
		private static IEnumerator ToggleInTimeCoroutine(GameObject obj, bool state, float time = DEFAULT_TOGGLE_TIME)
		{
			yield return (time.Approximately(default) ? DefaultWait : new WaitForSeconds(time));
			
			obj.SetActive(state);
		}
		
		#endregion
		
		public static T EnsuredGetComponent<T>(this GameObject gameObject) where T : Component
			=>  gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
	}
}