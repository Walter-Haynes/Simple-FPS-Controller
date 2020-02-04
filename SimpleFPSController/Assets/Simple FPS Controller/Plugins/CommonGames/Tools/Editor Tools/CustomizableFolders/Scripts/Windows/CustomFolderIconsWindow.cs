#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;

using CommonGames.Utilities.Helpers;

namespace CommonGames.Tools.CustomFolderIcons
{
    public class CustomFolderIconsWindow : OdinMenuEditorWindow
    {
        public string AssetFolderPath { get; set; } = "Assets";
        
        [MenuItem("Tools/Common-Games/Custom Folder Icons")]
        private static void Open()
        {
            OdinMenuEditorWindow __window = GetWindow<CustomFolderIconsWindow>();
            __window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree __tree = new OdinMenuTree(true)
            {
                DefaultMenuStyle = {IconSize = 28.00f}, Config = {DrawSearchToolbar = true}
            };

            __tree.AddAllAssetsAtPath("", AssetFolderPath, typeof(Style), true);

            return __tree;
        }

        protected override void OnBeginDrawEditors()
        {
            OdinMenuItem __selected = this.MenuTree.Selection.FirstOrDefault();
            int __toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(__toolbarHeight);
            {
                if (__selected != null)
                {
                    GUILayout.Label(__selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Style Preset")))
                {
                    ScriptableObjectCreator.ShowDialog<Style>
                    (
                        AssetFolderPath, style =>
                        {
                            style.name = style.name;
                            base.TrySelectMenuItemWithObject(style); // Selects the newly created item in the editor
                        }
                    );
                    
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}

#endif