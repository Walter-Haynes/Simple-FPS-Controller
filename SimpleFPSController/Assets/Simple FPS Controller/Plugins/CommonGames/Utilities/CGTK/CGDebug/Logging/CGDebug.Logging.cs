namespace CommonGames.Utilities.CGTK
{
    using UnityEngine;
    
    using Object = UnityEngine.Object;
    
    public static partial class CGDebug
    {
        public static void Log(in object message)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.Log(message);
            #endif
        }
    
        public static void Log(in object message, in Object context)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.Log(message, context);
            #endif
        }
    
        public static void LogFormat(in string format, params object[] args)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogFormat(format, args);
            #endif
        }
    
        public static void LogFormat(in Object context, in string format, params object[] args)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogFormat(context, format, args);
            #endif
        }
    
        public static void LogAssertion(Object message)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogAssertion(message);
            #endif
        }
    
        public static void LogAssertion(Object message, in Object context)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogAssertion(message, context);
            #endif
        }
    
        public static void LogAssertionFormat(in string format, params object[] args)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogAssertionFormat(format, args);
            #endif
        }
    
        public static void LogAssertionFormat(in Object context, in string format, params object[] args)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogAssertionFormat(context, format, args);
            #endif
        }
    
        public static void LogError(in object message)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogError(message);
            #endif
        }
    
        public static void LogError(in object message, in Object context)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogError(message, context);
            #endif
        }
    
        public static void LogErrorFormat(in string format, params object[] args)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogErrorFormat(format, args);
            #endif
        }
    
        public static void LogErrorFormat(in Object context, in string format, params object[] args)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogErrorFormat(context, format, args);
            #endif
        }
    
        public static void LogWarning(in object message)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogWarning(message);
            #endif
        }
    
        public static void LogWarning(in object message, in Object context)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogWarning(message, context);
            #endif
        }
    
        public static void LogWarningFormat(in string format, params object[] args)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogWarningFormat(format, args);
            #endif
        }
    
        public static void LogWarningFormat(in Object context, in string format, params object[] args)
        {
            #if UNITY_EDITOR || DEBUGBUILD
            Debug.LogWarningFormat(context, format, args);
            #endif
        }
    }
}