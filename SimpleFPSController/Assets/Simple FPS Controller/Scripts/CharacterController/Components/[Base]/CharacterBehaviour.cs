using System;
using UnityEngine;

namespace SimpleFPSController.PlayerSystems.Movement
{
    using JetBrains.Annotations;
    
    using CommonGames.Utilities;
    using CommonGames.Utilities.Extensions;

    /// <summary>
    /// Inherit from VehicleBehaviour if you want a MonoBehaviour with access to your Vehicle.
    /// It's basically a shorthand so I can just call all childed VehicleBehaviours from the VehicleCore on Start and send them the proper index.
    /// </summary>
    public abstract class CharacterBehaviour : MonoBehaviour
    {
        #region Variables

        private PlayerCore _player = null;
        [PublicAPI]
        public PlayerCore Player
        {
            get => _player = _player.TryGetInParentIfNull(this);
            protected set => _player = value;
        }

        [UsedImplicitly]
        protected bool IsInitialized = false;

        /// <summary> The Index of our VehicleCore reference in it's <see cref="IndexedMultiton{T}"/>'s List </summary>
        public int VehicleIndex { get; set; } = -1;
        
        #endregion

        #region Methods

        protected virtual void Awake()
        {
            IsInitialized = true;
            
            SetupReferences();
        }

        /// <summary> Sets the Vehicle's <see cref= "VehicleCore"/> reference to this vehicle's VehicleCore </summary>
        public virtual void SetupReferences()
            => Player = Player ? Player : PlayerCore.Instances[VehicleIndex];

        #endregion
        
    }
}