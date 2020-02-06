using UnityEngine;

namespace SimpleFPSController.PlayerSystems.Movement
{
    using CommonGames.Utilities.Extensions;
    using Physics = UnityEngine.Physics;
    
    [RequireComponent(typeof(PlayerCore))] // this also requires Character Controller
    public class WallRun : PlayerBehaviour
    {
        #region Variables

        private RaycastHit hitInfo;
        
        private bool attachedOnWall;
        private bool directionToCheckForWall = false; // False is left and True is right
        private Vector3 velocity;
        private float timerToAttachToNextWall;

        [SerializeField] private GrapplingHook grapplingHook = null; 

        [Header("Specifications to attach")]
        [SerializeField] private float maxDistanceToAttachToWall; // The maximum distance needed for the player to attach to a wall

        [Header("Specifications for run")]
        [SerializeField] private float runningSpeed = 2.5f;

        [SerializeField] private float dampingVelocity = 4f;
        [SerializeField] private float heightForce = 8f;
        [SerializeField] private float sideForce = 5f;
        
        #region Component Accessors

        //public WallRun WallRun => wallRun = wallRun.TryGetIfNull(this);
        public GrapplingHook GrapplingHook => grapplingHook = Player.CharacterBehaviours.GetOfType<PlayerBehaviour, GrapplingHook>() as GrapplingHook;

        #endregion

        #endregion

        #region Methods

        private void Update()
        {
            if(!attachedOnWall)
            {
                timerToAttachToNextWall -= Time.deltaTime;
                if(timerToAttachToNextWall <= 0)
                {
                    timerToAttachToNextWall = 0.0f;
                    if(Physics.Raycast(transform.position, transform.right, out hitInfo, maxDistanceToAttachToWall))
                    {
                        if(hitInfo.transform.CompareTag("WallRun"))
                        {
                            if(Player.verticalInput <= 0) return;
                            
                            directionToCheckForWall = true;
                            BeginWallRun(10);

                            __WallRunning();
                        }
                    }

                    if(Physics.Raycast(transform.position, -transform.right, out hitInfo, maxDistanceToAttachToWall))
                    {
                        if(hitInfo.transform.CompareTag("WallRun"))
                        {
                            if(Player.verticalInput <= 0) return;
                            
                            directionToCheckForWall = false;
                            BeginWallRun(-10);

                            __WallRunning();
                        }
                    }
                }
            }

            __WallRunning();

            void __WallRunning()
            {
                if(attachedOnWall) WallRunning();

                if(velocity != Vector3.zero)
                {
                    velocity -= Vector3.Normalize(velocity) * dampingVelocity * Time.deltaTime;
                
                    if(velocity.sqrMagnitude <= .2f)
                    {
                        velocity = Vector3.zero;
                    }
                }

                Player.velocity += velocity + inputVelocity;
            }
        }

        /// <param name="zRotation"> The Z rotation of the camera </param>
        private void BeginWallRun(float zRotation)
        {
            attachedOnWall = true;
            Player.useGravity = false;
            
            Player.DisableMovement();
            
            Player.PlayerFPSCamera.aimedZRotation = zRotation;
            velocity = Vector3.zero;

            GrapplingHook.Unhook();
            GrapplingHook.momentum = Vector3.zero;
        }

        public void EndWallRun()
        {
            attachedOnWall = false;
            Player.useGravity = true;
            Player.EnableMovement();
            
            Player.PlayerFPSCamera.aimedZRotation = 0;
            timerToAttachToNextWall = .2f;
            inputVelocity = Vector3.zero;
        }

        private void WallRunning()
        {
            if(Player.verticalInput <= 0)
            {
                EndWallRun();
                return;
            }

            // Checking if still attached to the wall
            if(directionToCheckForWall)
            {
                if(!Physics.Raycast(transform.position, transform.right, out hitInfo, maxDistanceToAttachToWall))
                {
                    EndWallRun();
                    return;
                }
            }
            else
            {
                if(!Physics.Raycast(transform.position, -transform.right, out hitInfo, maxDistanceToAttachToWall))
                {
                    EndWallRun();
                    return;
                }
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(directionToCheckForWall)
                {
                    //PlayerCore.characterController.Move((-transform.right * runningSpeed + transform.forward * runningSpeed) * runningSpeed * Time.deltaTime);
                    velocity = -transform.right * sideForce + transform.forward * sideForce * .5f +
                               Vector3.up * heightForce;
                    EndWallRun();
                }
                else
                {
                    //PlayerCore.characterController.Move((transform.right * runningSpeed + transform.forward * runningSpeed) * runningSpeed * Time.deltaTime);
                    velocity = transform.right * sideForce + transform.forward * sideForce * .5f +
                               Vector3.up * heightForce;
                    EndWallRun();
                }

                return;
            }

            inputVelocity = transform.forward * runningSpeed - Vector3.up * 4;
        }

        private Vector3 inputVelocity;

        #endregion
    }
}