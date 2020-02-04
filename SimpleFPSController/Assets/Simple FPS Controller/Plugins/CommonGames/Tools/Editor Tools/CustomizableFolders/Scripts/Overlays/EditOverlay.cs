using System.Linq;
using UnityEditor;
using UnityEngine;

using JetBrains.Annotations;

using CommonGames.Tools.CustomFolderIcons.Windows;

namespace CommonGames.Tools.CustomFolderIcons.Overlays
{
	public static class EditOverlay
	{
		private const EventModifiers EDIT_FOLDER_STYLE_MODIFIER = EventModifiers.Alt;

		[PublicAPI]
		public static void Draw(string guid, Rect rect)
		{
			if((Event.current.modifiers & EDIT_FOLDER_STYLE_MODIFIER) == EventModifiers.None) return;

			bool __isMouseOver = rect.Contains(Event.current.mousePosition);

			if(!__isMouseOver && !IsSelected(guid)) return;

			string __path = AssetDatabase.GUIDToAssetPath(guid);
			if(!AssetDatabase.IsValidFolder(__path)) return;

			if(GUI.Button(rect, GUIContent.none, GUIStyle.none))
			{
				Vector2 __screenPoint = GUIUtility.GUIToScreenPoint(rect.position + new Vector2(0, rect.height + 2));

				OpenWindow(new Rect(__screenPoint, CreateFolderRuleWindow.DefaultWindowSize), guid);
			}

			EditorApplication.RepaintProjectWindow();
		}
		
		private static void OpenWindow(Rect position, string guid)
		{
			CreateFolderRuleWindow __window = UnityEditor.EditorWindow.GetWindow<CreateFolderRuleWindow>();
			__window.position = position;
			__window.GUID = guid;
		}

		private static bool IsSelected(string guid)
			=> Selection.assetGUIDs.Any(asset => asset == guid);
	}
}
