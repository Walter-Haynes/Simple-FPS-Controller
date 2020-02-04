namespace CommonGames.Utilities.CGTK.Greasy
{
    
    /*
    public static class Easing
    {
        
    }
    
    #if UNITY_EDITOR

    public class CustomEasingTypeDrawer : OdinValueDrawer<Easing>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Easing value = this.ValueEntry.SmartValue;

            Rect rect = EditorGUILayout.GetControlRect();

            // In Odin, labels are optional and can be null, so we have to account for that.
            if(label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            float prev = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 20;

            value.X = EditorGUI.Slider(rect.AlignLeft(rect.width * 0.5f), "X", value.X, 0, 1);
            value.Y = EditorGUI.Slider(rect.AlignRight(rect.width * 0.5f), "Y", value.Y, 0, 1);

            EditorGUIUtility.labelWidth = prev;

            this.ValueEntry.SmartValue = value;
        }
    }

    #endif
    */
	
    /*
    //TODO: Create Version of EaseType which handles custom curves.
    [Serializable]
    public struct EaseType
    {	
        public EaseType easeType;
    }
    */

    
    public delegate float EaseMethod(float t);
    
    public enum EaseType
    {
        Custom,
        Linear,
        QuadIn,
        QuadOut,
        QuadInOut,
        CubicIn,
        CubicOut,
        CubicInOut,
        QuartIn,
        QuartOut,
        QuartInOut,
        QuintIn,
        QuintOut,
        QuintInOut,
        BounceIn,
        BounceOut,
        BounceInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        CircularIn,
        CircularOut,
        CircularInOut,
        SinusIn,
        SinusOut,
        SinusInOut,
        ExponentialIn,
        ExponentialOut,
        ExponentialInOut
    }
}