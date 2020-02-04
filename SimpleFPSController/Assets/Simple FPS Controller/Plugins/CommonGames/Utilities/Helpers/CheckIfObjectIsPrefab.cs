using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CommonGames.Utilities.Extensions;

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CommonGames.Utilities.Helpers
{
    /// <summary> Prefab checks to reduce boilerplate for code that has to ExecuteAlways. </summary>
    [SuppressMessageAttribute("ReSharper", "InconsistentNaming")]
    public static partial class PrefabCheckHelper
    {
        public static bool CheckIfPrefab(in Component component)
        {
            #if UNITY_EDITOR

            bool isInPreviewScene = UnityEditor.SceneManagement.EditorSceneManager.IsPreviewSceneObject(component);
            bool isPrefab = isInPreviewScene || UnityEditor.EditorUtility.IsPersistent(component);

            if(isPrefab == false) return false;

            //Debug.LogError("GameObject is a prefab. ");
            
            return true;

            #else
            
            return false;

            #endif
            
        }
    }
}