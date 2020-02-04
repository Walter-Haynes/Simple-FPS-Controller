namespace CommonGames.Utilities.CustomTypes
{
	using System;
	using System.Collections;
	using UnityEditor;
	using UnityEngine;

	[Serializable]
	public struct RangedFloat
	{
		public float MinValue { get; }
		public float MaxValue { get; }

		public RangedFloat(float minValue, float maxValue)
		{
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}
	}

	#if UNITY_EDITOR

	#if ODIN_INSPECTOR
	//using PropertyDrawer = OdinPropertyD;
	#endif

	[CustomPropertyDrawer(typeof(RangedFloat), true)]
	public class RangedFloatDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);

			SerializedProperty __minProp = property.FindPropertyRelative("minValue");
			SerializedProperty __maxProp = property.FindPropertyRelative("maxValue");

			float __minValue = __minProp.floatValue;
			float __maxValue = __maxProp.floatValue;

			float __rangeMin = 0;
			float __rangeMax = 1;

			MinMaxRangeAttribute[] __ranges = (MinMaxRangeAttribute[])fieldInfo.GetCustomAttributes(typeof (MinMaxRangeAttribute), true);
			if (__ranges.Length > 0)
			{
				__rangeMin = __ranges[0].Min;
				__rangeMax = __ranges[0].Max;
			}

			const float __RANGE_BOUNDS_LABEL_WIDTH = 40f;

			Rect __rangeBoundsLabel1Rect = new Rect(position) {width = __RANGE_BOUNDS_LABEL_WIDTH};
		
			GUI.Label(__rangeBoundsLabel1Rect, new GUIContent(__minValue.ToString("F2")));
			position.xMin += __RANGE_BOUNDS_LABEL_WIDTH;

			Rect __rangeBoundsLabel2Rect = new Rect(position);
			__rangeBoundsLabel2Rect.xMin = __rangeBoundsLabel2Rect.xMax - __RANGE_BOUNDS_LABEL_WIDTH;
			GUI.Label(__rangeBoundsLabel2Rect, new GUIContent(__maxValue.ToString("F2")));
			position.xMax -= __RANGE_BOUNDS_LABEL_WIDTH;

			EditorGUI.BeginChangeCheck();
			EditorGUI.MinMaxSlider(position, ref __minValue, ref __maxValue, __rangeMin, __rangeMax);
			if (EditorGUI.EndChangeCheck())
			{
				__minProp.floatValue = __minValue;
				__maxProp.floatValue = __maxValue;
			}

			EditorGUI.EndProperty();
		}
	}


	#endif
}