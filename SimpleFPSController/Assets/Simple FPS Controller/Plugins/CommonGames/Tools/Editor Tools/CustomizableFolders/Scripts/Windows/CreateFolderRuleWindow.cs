using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.OdinInspector.Editor;

using JetBrains.Annotations;

using static UnityEditor.AssetDatabase;
using static System.IO.Path;

namespace CommonGames.Tools.CustomFolderIcons.Windows
{

    public class CreateFolderRuleWindow : OdinEditorWindow
    {
        #region Variables
        
        #region Statics & Consts
        
        public static Vector2 DefaultWindowSize => new Vector2(512, 90);

        [HideInInspector]
        public bool showHacks = false;
        
        private Rect WindowRect => new Rect(position.x, position.y, position.width, DefaultWindowSize.y);

        private const int LARGE_ICON_SIZE = CustomFolderIcons.LARGE_ICON_SIZE;

        private const string BASE_WINDOW_AREA = "Window";
        
        private const string HALVES = BASE_WINDOW_AREA + "/Halves";
        
        private const string
            PREVIEW_HALVE = HALVES + "/Preview",
            DATA_HALVE = HALVES + "/Data";
        
        private const string 
            RULELIST_AREA = DATA_HALVE + "/RuleListArea",
            MATCH_AREA = DATA_HALVE + "/MatchArea",
            PRESET_AREA = DATA_HALVE + "/PresetArea",
            BUTTON_AREA = DATA_HALVE + "/ButtonArea";
        
        #endregion
                
        [BoxGroup(BASE_WINDOW_AREA, showLabel: false)]
        
        #region Halves
        //These are just so I can control the area Inspector area's
        
        [HorizontalGroup(HALVES, width: LARGE_ICON_SIZE)] //, MaxWidth = LARGE_ICON_SIZE)]
        
        [UsedImplicitly]
        [ShowIf(nameof(showHacks))]
        [SerializeField] private string basegroup = "";
        
        [VerticalGroup(PREVIEW_HALVE)]
        
        [UsedImplicitly]
        [ShowIf(nameof(showHacks))]
        [SerializeField] private string previewArea = "";
        
        [VerticalGroup(DATA_HALVE)]
        
        [UsedImplicitly]
        [ShowIf(nameof(showHacks))]
        [SerializeField] private string dataArea = "";
        #endregion
        
        [HorizontalGroup(RULELIST_AREA)] [HideLabel]
        [SerializeField] private RuleList ruleList = null;

        [HorizontalGroup(MATCH_AREA, 0.25f)] [HideLabel]
        [SerializeField] private MatchType newRuleMatchType = MatchType.Path;
        
        [HorizontalGroup(MATCH_AREA)] [HideLabel]
        [SerializeField] private string newRuleValue = "";

        //Odin doesn't let me Serialize the Preset's Labelwidth in percentages, so this is my solution...
        [HorizontalGroup(PRESET_AREA, 0.25f)] [DisplayAsString] [HideLabel]
        [OdinSerialize] private readonly string _newRulePresetLabel = "Style ";
        
        [HorizontalGroup(PRESET_AREA)] [HideLabel]
        [SerializeField] private Style newRuleStyle = null;
        
        private Rule NewRule { get; set; } = null;

        [OdinSerialize]
        public MatchType NewRuleMatchType //{ get; set; }
        {
            get => NewRule.matchType;
            set => NewRule.matchType = value;
        }
        
        public string GUID { get; set; } = null; // GUID of the asset which was clicked.
        
        /// <summary> Path of the clicked asset. </summary>
        private string AssetPath => GUIDToAssetPath(GUID);
        
        /// <summary> Name of the clicked asset. </summary>
        private string AssetName => GetFileName(AssetPath);
        
        #endregion

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();
            
            NewRule = new Rule();
            ruleList = CustomFolderIcons.GetDefaultRules() ?? CustomFolderIcons.GetAllRuleLists().FirstOrDefault();

            newRuleValue = DefaultValueForType(newRuleMatchType);
        }
        
        #if UNITY_EDITOR

        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            if (IsValueDefaultOrEmpty)
            {
                newRuleValue = DefaultValueForType(newRuleMatchType);
            }
            
            Rect __rect = new Rect(8, 8, LARGE_ICON_SIZE, LARGE_ICON_SIZE);
            
            //NewRule?.preset.DrawPreview(new Rect());
            newRuleStyle?.DrawPreview(__rect);
        }

        #endif

        private void OnValidate()
        {
            if (IsValueDefaultOrEmpty)
            {
                newRuleValue = DefaultValueForType(newRuleMatchType);
            }
        }
        
        private string DefaultValueForType(MatchType type)
        {
            switch(type)
            {
                case MatchType.Path: return AssetPath;
                case MatchType.Name: return AssetName;
                default: return newRuleValue;
            }
        }
        
        private bool IsValueDefaultOrEmpty
            => (newRuleValue == AssetPath || newRuleValue == AssetName || newRuleValue == "" || newRuleValue == null);
        
        [ButtonGroup(BUTTON_AREA)]
        [Button(ButtonSizes.Medium)]
        public void Apply()
        {
            if (ruleList == null)
            {
                CloseWindow();
            }
            
            Undo.RecordObject(ruleList, "Added rule");
            
            ruleList.Add(new Rule()
            {
                value = newRuleValue,
                matchType = newRuleMatchType,
                style = newRuleStyle
            });
            
            //TODO: Create are you sure Window if there's already a rule using that Path or Name & if Preset is null
            
            ruleList.OnAfterDeserialize();
            
            EditorUtility.SetDirty(ruleList);

            CloseWindow();
        }

        [ButtonGroup(BUTTON_AREA)]
        [Button(ButtonSizes.Medium)]
        public void Cancel()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            Close();
            base.Close();
            DestroyImmediate(this);
        }
        
        #endregion
    }
}