namespace CommonGames.Utilities.CGTK
{
    using System;
    using UnityEngine;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif
    
    using Object = UnityEngine.Object;
    
    /// <summary>
    /// A wrapper that provides the means to safely serialize Scene Asset References.
    /// </summary>
    [Serializable]
    public sealed class SceneReference : ISerializationCallbackReceiver
    {
        #if UNITY_EDITOR
        [SerializeField] private Object sceneAsset;
        
        private bool IsValidSceneAsset => sceneAsset is SceneAsset;
        #endif

        // This should only ever be set during serialization/deserialization!
        [SerializeField] private string scenePath = string.Empty;

        // Use this when you want to actually have the scene path
        public string ScenePath
        {
            get
            {
                #if UNITY_EDITOR
                return GetScenePathFromAsset();
                #else
                // At runtime we rely on the stored path value which we assume was serialized correctly at build time.
                return scenePath;
                #endif
            }
            set
            {
                scenePath = value;
                #if UNITY_EDITOR
                sceneAsset = GetSceneAssetFromPath();
                #endif
            }
        }

        //public static implicit operator int(SceneReference sceneReference) => sceneReference.ScenePath;
        
        public static implicit operator string(SceneReference sceneReference) => sceneReference.ScenePath;
        
        #if UNITY_EDITOR
        
        public static implicit operator SceneAsset(SceneReference sceneReference) => sceneReference.GetSceneAssetFromPath();

        public static implicit operator SceneReference(SceneAsset sceneAsset) => new SceneReference() { SceneAsset = sceneAsset };

        public Object SceneAsset
        {
            get => sceneAsset;
            set => sceneAsset = value;
        }
        
        private SceneAsset GetSceneAssetFromPath()
            => string.IsNullOrEmpty(scenePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

        private string GetScenePathFromAsset()
            => (sceneAsset == null) ? string.Empty : AssetDatabase.GetAssetPath(sceneAsset);
        
        #endif
        

        /// <summary>
        /// Called to prepare this data for serialization.
        /// </summary>
        /// <inheritdoc />
        public void OnBeforeSerialize()
        {
            #if UNITY_EDITOR
            HandleBeforeSerialize();
            #endif
        }

        /// <summary>
        /// Called to set up data for deserialization.
        /// </summary>
        /// <inheritdoc />
        public void OnAfterDeserialize()
        {
            #if UNITY_EDITOR
            // We can't access AssetDataBase during serialization, so call event to handle later.
            EditorApplication.update += HandleAfterDeserialize;
            #endif
        }

        #if UNITY_EDITOR
        private void HandleBeforeSerialize()
        {
            //Asset is invalid but has a Path to try and recover from.
            if (IsValidSceneAsset == false && string.IsNullOrEmpty(scenePath) == false)
            {
                sceneAsset = GetSceneAssetFromPath();

                //No asset found, path was invalid. Make sure we don't carry over the invalid path.
                if (sceneAsset == null){ scenePath = string.Empty; }

                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }
            else
            {
                scenePath = GetScenePathFromAsset();
            }
        }

        private void HandleAfterDeserialize()
        {
            EditorApplication.update -= HandleAfterDeserialize;
            
            if (IsValidSceneAsset) { return; }
                
            //Asset is invalid but has a Path to try and recover from.
            if (string.IsNullOrEmpty(scenePath)) { return; }

            sceneAsset = GetSceneAssetFromPath();
            
            //No asset found, path was invalid. Make sure we don't carry over the invalid path.
            if (sceneAsset == null){ scenePath = string.Empty; }

            if (Application.isPlaying == false)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }
        }

        #endif
    }
}