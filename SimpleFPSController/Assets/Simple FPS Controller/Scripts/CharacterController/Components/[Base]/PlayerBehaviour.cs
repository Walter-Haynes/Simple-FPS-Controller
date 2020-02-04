using System;
using UnityEngine;

namespace SimpleFPSController.PlayerSystems.Movement
{
    using JetBrains.Annotations;
    
    using CommonGames.Utilities;
    using CommonGames.Utilities.Extensions;

    /// <summary>
    /// Inherit from <see cref="PlayerBehaviour"/> if you want a MonoBehaviour with access to your Players.
    /// It's basically a shorthand so I can just call all childed PlayerBehaviours from the PlayerCore on Start and send them the proper index.
    /// </summary>
    public abstract class PlayerBehaviour : MonoBehaviour
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

        /// <summary> The Index of our PlayerCore reference in it's <see cref="IndexedMultiton{T}"/>'s List </summary>
        public int PlayerIndex { get; set; } = -1;
        
        #endregion

        #region Methods

        protected virtual void Awake()
        {
            IsInitialized = true;
            
            SetupReferences();
        }

        /// <summary> Sets the Player's <see cref= "PlayerCore"/> reference. </summary>
        public virtual void SetupReferences()
            => Player = Player ? Player : PlayerCore.Instances[PlayerIndex];

        #endregion
        
    }
}