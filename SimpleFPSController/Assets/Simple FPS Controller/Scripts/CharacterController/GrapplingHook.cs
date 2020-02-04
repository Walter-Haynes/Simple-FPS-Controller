using UnityEngine;

namespace SimpleFPSController.PlayerSystems.Movement
{
    using CommonGames.Utilities.Extensions;
    
    [RequireComponent(typeof(FPSCamera))]
    public class GrapplingHook : CharacterBehaviour
    {
        #region Variables

        #region Shown
        
        [SerializeField] private float maxGrapplingDistance = 200;
        [SerializeField] private float hookSpeed = 10;
        [SerializeField] private float dampSpeed = 3; // Momentum(Vector3) slows down using damp speed

        
        [SerializeField] private WallRun wallRun;
        [SerializeField] private LineRenderer lineRenderer;

        #endregion

        #region Component Accessors

        public WallRun WallRun => wallRun = wallRun.TryGetIfNull(this); //Or get from the Player??

        #endregion

        #region Hidden

        private Vector3 grapplingLocation;
        private RaycastHit hitInfo;
        private float distance;
        
        [HideInInspector] public Vector3 momentum, dir;

        #endregion

        #endregion

        #region Methods

        private void Start()
        {
            WallRun.gHook = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxGrapplingDistance))
                {
                    grapplingLocation = hitInfo.point;
                    
                    Player.DisableMovement();
                    
                    momentum = Vector3.zero;
                    _wallRun.EndWallRun();
                    
                    __DecreaseMagnitude();
                }
                
                __DecreaseMagnitude();
            }

            if (Input.GetKey(KeyCode.E))
            {
                if (grapplingLocation != Vector3.zero)
                {
                    Vector3 __playerPosition = transform.position;
                    
                    distance = grapplingLocation.DistanceSquared(__playerPosition);

                    dir = (grapplingLocation - __playerPosition).normalized;
                    momentum = dir * (hookSpeed * Mathf.Clamp01(distance));
                    
                    _lineRenderer.SetPosition(0, __playerPosition - Vector3.up);
                    _lineRenderer.SetPosition(1, grapplingLocation);

                    __DecreaseMagnitude();
                }
                //return;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                if(grapplingLocation != Vector3.zero) Unhook();
                
                __DecreaseMagnitude();
            }

            void __DecreaseMagnitude()
            {
                if(!(momentum.sqrMagnitude >= 0.0f)) return;
                
                momentum -= momentum * (dampSpeed * Time.deltaTime * EffectsManager.currentTimeScale);
                if(momentum.sqrMagnitude < 0.0f)
                {
                    momentum = Vector3.zero;
                }
                
                Player.velocity += momentum;
                //PlayerCC.Move(momentum * Time.deltaTime);
            }
        }

        public void Unhook()
        {
            grapplingLocation = Vector3.zero;
            
            Player.EnableMovement();
            
            _lr.SetPosition(0, Vector3.zero);
            _lr.SetPosition(1, Vector3.zero);
        }
        
        #endregion
}
}