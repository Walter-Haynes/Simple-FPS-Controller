using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CommonGames.Tools.CustomFolderIcons.Overlays
{
	/// <summary>
	/// Provides the core overlay functionality for folder icons.
	/// </summary>
	public static class FolderOverlay
	{
		public static void Draw(string guid, Rect rect)
		{
			string __assetPath = AssetDatabase.GUIDToAssetPath(guid);
			
			if(!AssetDatabase.IsValidFolder(__assetPath)) return; // ignore files

			try
			{
				CustomFolderIcons.SquarifyIconRect(ref rect);

				IEnumerable<RuleList> __ruleLists = CustomFolderIcons.GetAllRuleLists();

				IEnumerable<RuleList> __ruleListsArray = (__ruleLists as RuleList[]) ?? __ruleLists.ToArray();

				Style __selectedStyle = (__ruleListsArray.Select(x => x.GetByPath(__assetPath)).FirstOrDefault(NotNull)
				                           ?? __ruleListsArray.Select(x => x.GetByName(__assetPath)).FirstOrDefault(NotNull));

				if (__selectedStyle == null) return; // no preset found

				if (__selectedStyle.Drawable())
				{
					EditorGUI.DrawRect(rect, CustomFolderIcons.BackgroundColor);
				}

				__selectedStyle.Draw(rect);
			}
			catch (Exception __exception)
			{
				Debug.LogException(__exception);
			}
		}

		private static bool NotNull<T>(T obj) where T : class
		{
			return obj != null;
		}
	}
}
