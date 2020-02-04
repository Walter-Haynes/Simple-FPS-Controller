using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonGames.Tools.CustomFolderIcons;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

#if ODIN_INSPECTOR || Odin_Inspector

using ScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject; 

#endif

namespace CommonGames.Tools.CustomFolderIcons
{
	[CreateAssetMenu(menuName = "CustomFolderIcons/Rule List")]
	public sealed class RuleList : ScriptableObject, ISerializationCallbackReceiver
	{
		//[ReadOnly]
		[SerializeField]
		private Rule[] rules;
		
		[OdinSerialize]
		private Dictionary<string, Style> _pathRules, _nameRules;

		//TODO: Find another way to check for this
		public bool IsDefaultList => name == "DefaultRuleList";

		public RuleList()
		{
			CustomFolderIcons.InvalidateRulelistCache();
		}

		
		/// <summary> Adds a <see cref="Rule"/> to the RuleList. </summary>
		public void Add(Rule rule)
		{
			if(rules == null)
			{
				rules = new[] {rule};
				return;
			}
			
			int __ruleCount = rules.Length;
			Array.Resize(ref rules, __ruleCount + 1);
			rules[__ruleCount] = rule;
		}

		public void Remove(IEnumerable<Rule> __rules)
		{
			if(this.rules == null) return;
			
			this.rules = this.rules.Except(__rules).ToArray();
		}

		public Style GetByPath(string path)
		{
			if(_pathRules == null) return null;

			_pathRules.TryGetValue(path, out Style __style);
			
			return __style;
		}

		public Style GetByName(string path)
		{
			// ReSharper disable once AssignNullToNotNullAttribute
			if(_nameRules != null && _nameRules.TryGetValue(Path.GetFileName(path), out Style __style))
			{
				return __style;
			}

			return null;
		}

		public void OnBeforeSerialize() { }
		
		//Don't want this to be public but we're inheriting from SerializedScriptableObject..
		#if ODIN_INSPECTOR
		public new void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();
		#else
		public void OnAfterDeserialize()
		{
		#endif
			
			if(this.rules == null)
			{
				_pathRules = new Dictionary<string, Style>();
				_nameRules = new Dictionary<string, Style>();
				return;
			}
			
			List<Rule> __rules = this.rules.Where(rule => rule?.value != null && rule.style != null).ToList();
			_pathRules = GetDictionary(__rules.Where(rule => rule.matchType == MatchType.Path));
			_nameRules = GetDictionary(__rules.Where(rule => rule.matchType == MatchType.Name));
		}

		private static Dictionary<string, Style> GetDictionary(IEnumerable<Rule> rules)
		{
			// Group because there might be duplicate keys causing is to not be able to directly do a ToDictionary() call
			return rules.GroupBy(x => x.value).ToDictionary(x => x.Key, x => x.First().style);
		}
	}
	
	public enum MatchType
	{
		Path,
		Name
	}
	
	/// <summary> Describes a rule used to match a folder to a style preset. </summary>
	[Serializable]
	public sealed class Rule
	{
		/// <summary> Depending on the <see cref="matchType"/> this value is either a name or a path. </summary>
		public string value = string.Empty;
		
		public MatchType matchType;
		public Style style;
	}
}
