using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

using Utilities.Extensions;
using CommonGames.Utilities.Extensions;

using JetBrains.Annotations;

using Debug = UnityEngine.Debug;

namespace CommonGames.Utilities
{
    /// <summary> A Multiton in which you can set the index in the list yourself. </summary>
    
    [ExecuteAlways]
    public class IndexedMultiton<T> : Multiton<T> 
        where T : Multiton<T>
    {
        #region Editor
        
        [HorizontalGroup(@group: "Index")]
        [PropertyTooltip(tooltip: "Lock Index?"), HideLabel]
        [SerializeField] private bool lockIndex = false;

        [HorizontalGroup(@group: "Index")]
        [PropertyTooltip(tooltip: "Index"), HideLabel]
        [ShowInInspector]
        public int Index => IndexFromInstance(instance: this as T);

        [HorizontalGroup(@group: "Index")]
        [Button(name: "＋")]
        public void IndexSwapUp()
        {
            IndexedMultiton<T> __nextAvailableInstance = GetNextAvailableInstance(above: true);

            int __aboveIndex = __nextAvailableInstance.Index;
                
            Instances.Swap(
                firstIndex: this.Index, 
                secondIndex: __aboveIndex);
        }

        private IndexedMultiton<T> GetNextAvailableInstance(in bool above)
        {
            IndexedMultiton<T> __nextNeighbour = GetNeighbouringInstance(above: above);

            while(__nextNeighbour.IsLocked)
            {
                if(__nextNeighbour == null)
                {
                    Debug.LogError("Instance Above is null");

                    return null;
                }

                if(!__nextNeighbour.IsLocked) return __nextNeighbour;

                __nextNeighbour = __nextNeighbour.GetNeighbouringInstance(above: above);
            }

            return __nextNeighbour;
        }

        [HorizontalGroup(@group: "Index")]
        [Button(name: "—")]
        public void IndexSwapDown()
        {
            IndexedMultiton<T> __nextAvailableInstance = GetNextAvailableInstance(above: false);

            int __aboveIndex = __nextAvailableInstance.Index;
                
            Instances.Swap(
                firstIndex: this.Index, 
                secondIndex: __aboveIndex);
        }

        
        /// <summary> Gets the Index above or below the current one. </summary>
        /// <param name="above"> whether to check *above* or *below*</param>
        [PublicAPI]
        public IndexedMultiton<T> GetNeighbouringInstance(in bool above)
            => (Instances[index: (Index + (above ? 1 : -1))] as IndexedMultiton<T>);

        /// <summary> Gets the Index above or below the Index to check </summary>
        /// <param name="above"> whether to check *above* or *below*</param>
        [PublicAPI]
        public IndexedMultiton<T> GetNeighbouringInstance(in bool above, in int index)
            => (Instances[index: (Index + (above ? 1 : -1))] as IndexedMultiton<T>);
        
        
        [PublicAPI]
        public bool IsLocked => lockIndex;

        #endregion

        #region Methods

        protected override void Reset()
        { 
            base.Reset();
            
            //index = GetIndex;
        }

        protected override void OnEnable()
        { 
            base.OnEnable();
            
            //index = GetIndex;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            //index = GetIndex;
        }
        
        protected override void RecalculateIndices()
        {
            base.RecalculateIndices();
            
            OnValidate();
        }

        #endregion
    }
}