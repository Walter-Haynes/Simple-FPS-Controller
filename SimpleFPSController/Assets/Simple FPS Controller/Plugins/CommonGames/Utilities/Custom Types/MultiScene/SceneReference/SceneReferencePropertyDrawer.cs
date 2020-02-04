namespace CommonGames.Utilities.CGTK
{
    #if UNITY_EDITOR  
    
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor.VersionControl;
    using UnityEngine;
  
    using UnityEditor;
    
    #if ODIN_INSPECTOR
    using Sirenix.OdinInspector.Editor;
    //using PropertyDrawer = Sirenix.OdinInspector.Editor.OdinValueDrawer<SceneReference>;
    #endif
    
    /// <summary>
    /// Display a Scene Reference object in the editor.
    /// If scene is valid, provides basic buttons to interact with the scene's role in Build Settings.
    /// </summary>
    /// <inheritdoc />
    
    [CustomPropertyDrawer(typeof(SceneReference))]
    public sealed class SceneReferencePropertyDrawer : PropertyDrawer
    {
        // The exact name of the asset Object variable in the SceneReference object
        private const string SCENE_ASSET_PROPERTY_STRING = "sceneAsset";

        /// <summary> The exact name of  the scene Path variable in the SceneReference object. </summary>
        private const string SCENE_PATH_PROPERTY_STRING = "scenePath";

        private static readonly RectOffset boxPadding = EditorStyles.helpBox.padding;
        private const float PAD_SIZE = 2f, FOOTER_HEIGHT = 10f;
        private static readonly float LineHeight = EditorGUIUtility.singleLineHeight
                                    , PaddedLine = LineHeight + PAD_SIZE;
        

        /// <inheritdoc />
        /// <summary> Drawing the 'SceneReference' property </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sceneAssetProperty = GetSceneAssetProperty(property);

            // Draw the Box Background
            position.height -= FOOTER_HEIGHT;
            GUI.Box(EditorGUI.IndentedRect(position), GUIContent.none, EditorStyles.helpBox);
            position = boxPadding.Remove(position);
            position.height = LineHeight;

            // Draw the main Object field
            label.tooltip = "The actual Scene Asset reference.\nOn serialize this is also stored as the asset's path.";

            EditorGUI.BeginProperty(position, GUIContent.none, property);
            EditorGUI.BeginChangeCheck();
            int sceneControlId = GUIUtility.GetControlID(FocusType.Passive);
            
            Object selectedObject = EditorGUI.ObjectField(position, label, sceneAssetProperty.objectReferenceValue,typeof(SceneAsset), false);
            
            BuildUtils.BuildScene buildScene = BuildUtils.GetBuildScene(selectedObject);

            if (EditorGUI.EndChangeCheck())
            {
                sceneAssetProperty.objectReferenceValue = selectedObject;

                // If no valid scene asset was selected, reset the stored path accordingly
                if (buildScene.scene == null)
                {
                    GetScenePathProperty(property).stringValue = string.Empty;
                }
            }

            position.y += PaddedLine;

            if (buildScene.assetGUID.Empty() == false)
            {
                // Draw the Build Settings Info of the selected Scene
                DrawSceneInfoGUI(position, buildScene, sceneControlId + 1);
            }

            EditorGUI.EndProperty();
        }

        /// <summary> Ensure that what we draw in OnGUI always has the room it needs </summary>
        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = 2;
            SerializedProperty sceneAssetProperty = GetSceneAssetProperty(property);
            
            if (sceneAssetProperty.objectReferenceValue == null){ lines = 1; }

            return boxPadding.vertical + LineHeight * lines + PAD_SIZE * (lines - 1) + FOOTER_HEIGHT;
        }

        /// <summary> Draws info box of the provided scene </summary>
        private static void DrawSceneInfoGUI(Rect position, BuildUtils.BuildScene buildScene, int sceneControlID)
        {
            bool readOnly = BuildUtils.IsReadOnly();
            string readOnlyWarning = readOnly 
                ? "\n\n WARNING: Build Settings is not checked out and so cannot be modified." 
                : string.Empty;

            // Label Prefix
            GUIContent iconContent = new GUIContent();
            GUIContent labelContent = new GUIContent();

            // Missing from build scenes
            if (buildScene.buildIndex == -1)
            {
                iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_close");
                labelContent.text = "NOT In Build";
                labelContent.tooltip = "This scene is NOT in build settings.\nIt will be NOT included in builds.";
            }
            // In build scenes and enabled
            else if (buildScene.scene.enabled)
            {
                iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_max");
                labelContent.text = "BuildIndex: " + buildScene.buildIndex;
                labelContent.tooltip = "This scene is in build settings and ENABLED.\nIt will be included in builds." +
                                       readOnlyWarning;
            }
            // In build scenes and disabled
            else
            {
                iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_min");
                labelContent.text = "BuildIndex: " + buildScene.buildIndex;
                labelContent.tooltip =
                    "This scene is in build settings and DISABLED.\nIt will be NOT included in builds.";
            }

            // Left status label
            using(new EditorGUI.DisabledScope(readOnly))
            {
                Rect labelRect = DrawUtils.GetLabelRect(position);
                Rect iconRect = labelRect;
                iconRect.width = iconContent.image.width + PAD_SIZE;
                labelRect.width -= iconRect.width;
                labelRect.x += iconRect.width;
                EditorGUI.PrefixLabel(iconRect, sceneControlID, iconContent);
                EditorGUI.PrefixLabel(labelRect, sceneControlID, labelContent);
            }

            // Right context buttons
            Rect buttonRect = DrawUtils.GetFieldRect(position);
            buttonRect.width = (buttonRect.width) / 3;

            string tooltipMsg;
            
            using (new EditorGUI.DisabledScope(readOnly))
            {
                // NOT in build settings
                if (buildScene.buildIndex == -1)
                {
                    buttonRect.width *= 2;
                    int addIndex = EditorBuildSettings.scenes.Length;
                    tooltipMsg =
                        $"Add this scene to build settings. It will be appended to the end of the build scenes as buildIndex: {addIndex}.{readOnlyWarning}";
                    
                    if (DrawUtils.ButtonHelper(buttonRect, "Add...", "Add (buildIndex " + addIndex + ")",
                        EditorStyles.miniButtonLeft, tooltipMsg))
                        BuildUtils.AddBuildScene(buildScene);
                    
                    buttonRect.width /= 2;
                    buttonRect.x += buttonRect.width;
                }
                // In build settings
                else
                {
                    bool isEnabled = buildScene.scene.enabled;
                    string stateString = isEnabled ? "Disable" : "Enable";
                    tooltipMsg = stateString + " this scene in build settings.\n" +
                                 (isEnabled
                                     ? "It will no longer be included in builds"
                                     : "It will be included in builds") + $".{readOnlyWarning}";

                    if (DrawUtils.ButtonHelper(buttonRect, stateString, stateString + " In Build",
                        EditorStyles.miniButtonLeft, tooltipMsg))
                        
                    BuildUtils.SetBuildSceneState(buildScene, !isEnabled);
                    
                    buttonRect.x += buttonRect.width;

                    tooltipMsg = "Completely remove this scene from build settings." +
                                 $"\nYou will need to add it again for it to be included in builds! {readOnlyWarning}";
                    
                    if (DrawUtils.ButtonHelper(buttonRect, "Remove...", "Remove from Build", EditorStyles.miniButtonMid,
                        tooltipMsg))
                    {
                        BuildUtils.RemoveBuildScene(buildScene);
                    }
                }
            }

            buttonRect.x += buttonRect.width;

            tooltipMsg = $"Open the 'Build Settings' Window for managing scenes.{readOnlyWarning}";
            if (DrawUtils.ButtonHelper(buttonRect, "Settings", "Build Settings", EditorStyles.miniButtonRight, tooltipMsg))
            {
                BuildUtils.OpenBuildSettings();
            }

        }

        private static SerializedProperty GetSceneAssetProperty(SerializedProperty property)
            => property.FindPropertyRelative(SCENE_ASSET_PROPERTY_STRING);

        private static SerializedProperty GetScenePathProperty(SerializedProperty property)
            => property.FindPropertyRelative(SCENE_PATH_PROPERTY_STRING);

        private static class DrawUtils
        {
            /// <summary> Draw a GUI button, choosing between a short and a long button text based on if it fits </summary>
            public static bool ButtonHelper(Rect position, string msgShort, string msgLong, GUIStyle style,
                string tooltip = null)
            {
                GUIContent content = new GUIContent(msgLong)
                {
                    tooltip = tooltip
                };

                float longWidth = style.CalcSize(content).x;
                if (longWidth > position.width){ content.text = msgShort; }

                return GUI.Button(position, content, style);
            }

            /// <summary> Given a position rect, get its field portion </summary>
            public static Rect GetFieldRect(Rect position)
            {
                position.width -= EditorGUIUtility.labelWidth;
                position.x += EditorGUIUtility.labelWidth;
                return position;
            }

            /// <summary> Given a position rect, get its label portion </summary>
            public static Rect GetLabelRect(Rect position)
            {
                position.width = EditorGUIUtility.labelWidth - PAD_SIZE;
                return position;
            }
        }

        /// <summary> Various BuildSettings interactions </summary>
        private static class BuildUtils
        {
            // time in seconds that we have to wait before we query again when IsReadOnly() is called.
            private const float MIN_CHECK_WAIT = 3;

            private static float lastTimeChecked;
            private static bool cachedReadonlyVal = true;

            /// <summary>
            /// A small container for tracking scene data BuildSettings
            /// </summary>
            public struct BuildScene
            {
                public int buildIndex;
                public GUID assetGUID;
                public string assetPath;
                public EditorBuildSettingsScene scene;
            }

            /// <summary>
            /// Check if the build settings asset is readonly.
            /// Caches value and only queries state a max of every 'minCheckWait' seconds.
            /// </summary>
            public static bool IsReadOnly()
            {
                float curTime = Time.realtimeSinceStartup;
                float timeSinceLastCheck = curTime - lastTimeChecked;

                if (!(timeSinceLastCheck > MIN_CHECK_WAIT)) { return cachedReadonlyVal; }
                
                lastTimeChecked = curTime;
                cachedReadonlyVal = QueryBuildSettingsStatus();

                return cachedReadonlyVal;
            }

            /// <summary>
            /// A blocking call to the Version Control system to see if the build settings asset is readonly.
            /// Use BuildSettingsIsReadOnly for version that caches the value for better responsivenes.
            /// </summary>
            private static bool QueryBuildSettingsStatus()
            {
                // If no version control provider, assume not readonly
                if (UnityEditor.VersionControl.Provider.enabled == false) { return false; }

                // If we cannot checkout, then assume we are not readonly
                if (UnityEditor.VersionControl.Provider.hasCheckoutSupport == false) { return false; }

                //// If offline (and are using a version control provider that requires checkout) we cannot edit.
                //if (UnityEditor.VersionControl.Provider.onlineState == UnityEditor.VersionControl.OnlineState.Offline)
                //    return true;

                // Try to get status for file
                Task status = UnityEditor.VersionControl.Provider.Status("ProjectSettings/EditorBuildSettings.asset", false);
                
                status.Wait();

                // If no status listed we can edit
                if (status.assetList == null || status.assetList.Count != 1) { return true; }

                // If is checked out, we can edit
                return !status.assetList[0].IsState(UnityEditor.VersionControl.Asset.States.CheckedOutLocal);
            }

            /// <summary>
            /// For a given Scene Asset object reference, extract its build settings data, including buildIndex.
            /// </summary>
            public static BuildScene GetBuildScene(Object sceneObject)
            {
                BuildScene entry = new BuildScene()
                {
                    buildIndex = -1,
                    assetGUID = new GUID(string.Empty)
                };

                if (sceneObject as SceneAsset == null)
                    return entry;

                entry.assetPath = AssetDatabase.GetAssetPath(sceneObject);
                entry.assetGUID = new GUID(AssetDatabase.AssetPathToGUID(entry.assetPath));

                for (int index = 0; index < EditorBuildSettings.scenes.Length; ++index)
                {
                    if (!entry.assetGUID.Equals(EditorBuildSettings.scenes[index].guid)) { continue; }
                    
                    entry.scene = EditorBuildSettings.scenes[index];
                    entry.buildIndex = index;
                    return entry;
                }

                return entry;
            }

            /// <summary>
            /// Enable/Disable a given scene in the buildSettings
            /// </summary>
            public static void SetBuildSceneState(BuildScene buildScene, bool enabled)
            {
                bool modified = false;
                EditorBuildSettingsScene[] scenesToModify = EditorBuildSettings.scenes;
                foreach (EditorBuildSettingsScene curScene in scenesToModify)
                {
                    if (!curScene.guid.Equals(buildScene.assetGUID)) { continue; }
                    
                    curScene.enabled = enabled;
                    modified = true;
                    break;
                }

                if (modified)
                    EditorBuildSettings.scenes = scenesToModify;
            }

            /// <summary>
            /// Display Dialog to add a scene to build settings
            /// </summary>
            public static void AddBuildScene(BuildScene buildScene, bool force = false, bool enabled = true)
            {
                if (force == false)
                {
                    int selection = EditorUtility.DisplayDialogComplex(
                        "Add Scene To Build",
                        "You are about to add scene at " + buildScene.assetPath + " To the Build Settings.",
                        "Add as Enabled", // option 0
                        "Add as Disabled", // option 1
                        "Cancel (do nothing)"); // option 2

                    switch (selection)
                    {
                        case 0: // enabled
                            enabled = true;
                            break;
                        case 1: // disabled
                            enabled = false;
                            break;
                        default:
                            return;
                    }
                }

                EditorBuildSettingsScene newScene = new EditorBuildSettingsScene(buildScene.assetGUID, enabled);
                List<EditorBuildSettingsScene> tempScenes = EditorBuildSettings.scenes.ToList();
                tempScenes.Add(newScene);
                EditorBuildSettings.scenes = tempScenes.ToArray();
            }

            /// <summary>
            /// Display Dialog to remove a scene from build settings (or just disable it)
            /// </summary>
            public static void RemoveBuildScene(BuildScene buildScene, bool force = false)
            {
                bool onlyDisable = false;
                if (force == false)
                {
                    int selection = -1;

                    const string title = "Remove Scene From Build";
                    string details =
                        "You are about to remove the following scene from build settings:\n" +
                        $"{buildScene.assetPath}\n" +
                        $"buildIndex: {buildScene.buildIndex}" +
                        "\n\nThis will modify build settings, but the scene asset will remain untouched.";
                    
                    string confirm = "Remove From Build";
                    string alt = "Just Disable";
                    string cancel = "Cancel (do nothing)";

                    if (buildScene.scene.enabled)
                    {
                        details += "\n\nIf you want, you can also just disable it instead.";
                        selection = EditorUtility.DisplayDialogComplex(title, details, confirm, alt, cancel);
                    }
                    else
                    {
                        selection = EditorUtility.DisplayDialog(title, details, confirm, cancel) ? 0 : 2;
                    }

                    switch (selection)
                    {
                        case 0: // remove
                            break;
                        case 1: // disable
                            onlyDisable = true;
                            break;
                        default:
                            return;
                    }
                }

                // User chose to not remove, only disable the scene
                if (onlyDisable)
                {
                    SetBuildSceneState(buildScene, false);
                }
                // User chose to fully remove the scene from build settings
                else
                {
                    List<EditorBuildSettingsScene> tempScenes = EditorBuildSettings.scenes.ToList();
                    tempScenes.RemoveAll(scene => scene.guid.Equals(buildScene.assetGUID));
                    EditorBuildSettings.scenes = tempScenes.ToArray();
                }
            }

            /// <summary>
            /// Open the default Unity Build Settings window
            /// </summary>
            public static void OpenBuildSettings()
            {
                EditorWindow.GetWindow(typeof(BuildPlayerWindow));
            }
        }
    }
    
    #endif
}