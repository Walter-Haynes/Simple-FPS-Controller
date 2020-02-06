using UnityEngine;

namespace SimpleFPSController.PlayerSystems.Movement.PhysicsComponents
{
    public class Grounding : PlayerBehaviour
    {
        #region Variables
        
        [Header("Gravity")]
        [SerializeField] private bool useGravity = true;

        [SerializeField] private Vector3 groundCheckOffset = new Vector3(0, -1.35f, 0);
        [SerializeField] private float maxGroundDistance = 0.4f;

        private bool _isGrounded;
        
        public bool IsGrounded { get; set; } 

        #endregion

        #region Methods

        private void FixedUpdate()
        {
            Time.frameCount
        }

        #endregion
    }
}