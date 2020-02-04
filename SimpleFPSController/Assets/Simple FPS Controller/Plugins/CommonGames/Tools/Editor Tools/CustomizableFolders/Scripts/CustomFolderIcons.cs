using System;
using System.Collections.Generic;
using System.Linq;
using CommonGames.Tools.CustomFolderIcons.Overlays;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;
using JetBrains.Annotations;

namespace CommonGames.Tools.CustomFolderIcons
{
	[InitializeOnLoad]
	public static class CustomFolderIcons
	{
		private static List<RuleList> _rulelistCache;
		private static bool _isRulelistCache;

		private static Color32
			_proColor = new Color32(56, 56, 56, 255),
			_scrubColor = new Color32(190, 190, 190, 255);
		
		public const int SMALL_ICON_SIZE = 16;
		public const int LARGE_ICON_SIZE = 64;

		[PublicAPI]
		public static ref Color32 BackgroundColor 
			=> ref EditorGUIUtility.isProSkin ? ref _proColor : ref _scrubColor;

		[PublicAPI] //TODO: Create Extension method in CGTK so this can be one line.
		public static Color InverseBackgroundColor
		{
			get
			{
				Color __color = Color.white - BackgroundColor;
				__color.a = 1f;
				
				return __color;
			}
		}

		[UsedImplicitly]
		static CustomFolderIcons()
		{
			EditorApplication.projectWindowItemOnGUI += FolderOverlay.Draw;
			EditorApplication.projectWindowItemOnGUI += PreviewOverlay.Draw;
			EditorApplication.projectWindowItemOnGUI += EditOverlay.Draw;
		}

		/// <summary> Adjusts the icon's size to be a square, and fixes offsets. </summary>
		/// <param name="rect"></param>
		/// <returns> Returns if the icon is small or not. </returns>
		[PublicAPI]
		public static bool SquarifyIconRect(ref Rect rect)
		{
			bool __isSmall = rect.width > rect.height;
			
			rect.width = rect.height = Math.Min(rect.width, rect.height);
			if(rect.width > LARGE_ICON_SIZE)
			{
				// center icon if region is larger than the large icon
				Vector2 __maxSize = new Vector2(LARGE_ICON_SIZE, LARGE_ICON_SIZE);
				rect = new Rect((rect.center - (__maxSize * .5f)), __maxSize);
			}
			else if(__isSmall && ((int)rect.x - 16) % 14 != 0)
			{
				// there is a small offset in the project view for small icons
				rect.x += 3;
			}
			return __isSmall;
		}

		/// <summary> Finds all Instance Assets of <T> </summary>
		[PublicAPI]
		public static IEnumerable<string> FindAll<T>() where T : Object
			=> AssetDatabase.FindAssets($"t:{typeof(T).FullName}").Select(AssetDatabase.GUIDToAssetPath);

		[PublicAPI]
		public static void InvalidateRulelistCache()
			=> _isRulelistCache = true;

		///<summary> Returns all instances of RuleList. </summary>
		[PublicAPI]
		public static IEnumerable<RuleList> GetAllRuleLists()
		{
			if(_rulelistCache == null || _isRulelistCache)
			{
				_rulelistCache = FindAll<RuleList>().Select(AssetDatabase.LoadAssetAtPath<RuleList>).ToList();
				_isRulelistCache = false;
			}
			else
			{
				_rulelistCache.RemoveAll(ruleList => !ruleList);
			}
			
			return _rulelistCache;
		}

		/// <summary> Returns the default RuleList instance. </summary>
		[PublicAPI]
		public static RuleList GetDefaultRules()
			=> GetAllRuleLists().FirstOrDefault(ruleList => ruleList.IsDefaultList);

		/// <summary> Draws a rect in the colour of your editor's background. </summary>
		[PublicAPI]
		public static void DrawBackgroundRect(in Rect rect)
			=> EditorGUI.DrawRect(rect, BackgroundColor);
	}
}