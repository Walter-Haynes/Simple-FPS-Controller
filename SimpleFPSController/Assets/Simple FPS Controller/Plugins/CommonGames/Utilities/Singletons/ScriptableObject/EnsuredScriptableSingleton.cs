using UnityEngine;

using JetBrains.Annotations;

#if ODIN_INSPECTOR
using ScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject;
#endif
    
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CommonGames.Utilities
{

    /// <typeparam name="T"> Type of the Singleton. </typeparam>
    public abstract class EnsuredScriptableSingleton<T> : ScriptableObject 
        where T : EnsuredScriptableSingleton<T>
    {
        #region Variables

        private static T _instance = null;

        [PublicAPI]
        public static T Instance
        {
            get
            {
                if(!InstanceExists)
                {
                    _instance = Resources.FindObjectsOfTypeAll<T>()[0];
                    
                    if(InstanceExists)
                    {
                        _instance = CreateInstance<T>();    
                    }
                }

                return _instance;
            }
            set => _instance = value;
        }

        /// <summary> Gets whether an Instance of this singleton exists. </summary>
        [PublicAPI]
        public static bool InstanceExists => (_instance != null);

        #endregion

        #region Methods

        /// <summary> OnEnable method to associate Singleton with Instance. </summary>
        protected virtual void OnEnable()
        {
            if(InstanceExists)
            {
                #if UNITY_EDITOR

                if(!EditorApplication.isPlaying)
                {
                    DestroyImmediate(this);
                }
                else
                {
                    Destroy(this);
                }

                #else
                Destroy(this);
                #endif
                
            }
            else
            {
                Instance = this as T;
            }
        }

        /// <summary> OnDestroy method to clear Singleton association. </summary>
        protected virtual void OnDestroy()
        {
            if(Instance == this) Instance = null;
        }

        #endregion
    }
}