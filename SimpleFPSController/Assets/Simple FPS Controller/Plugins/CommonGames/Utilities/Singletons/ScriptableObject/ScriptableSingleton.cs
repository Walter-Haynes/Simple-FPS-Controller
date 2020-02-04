namespace CommonGames.Utilities
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Collections.Generic;

    using JetBrains.Annotations;
    #if ODIN_INSPECTOR
    using ScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject;
    #endif
    
    #if UNITY_EDITOR
    using UnityEditor;
    #endif
    
    /// <typeparam name="T"> Type of the Singleton. </typeparam>
    public abstract class ScriptableSingleton<T> : ScriptableObject 
        where T : ScriptableSingleton<T>
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
                    Resources.LoadAll(path: "", typeof(T));
                    
                    Debug.Log($"Gettting all of type {typeof(T)}");
                    
                    _instance = Resources.FindObjectsOfTypeAll<T>()[0];
                }

                #if UNITY_EDITOR

                List<Object> __preloadedAssets = new List<Object>();
                __preloadedAssets.AddRange(UnityEditor.PlayerSettings.GetPreloadedAssets());

                if (!__preloadedAssets.Contains(_instance))
                {
                    __preloadedAssets.Add(_instance);

                    PlayerSettings.SetPreloadedAssets(__preloadedAssets.ToArray());
                }
                
                #endif

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
        [RuntimeInitializeOnLoadMethod]
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