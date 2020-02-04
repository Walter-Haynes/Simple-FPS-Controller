using UnityEngine;

using JetBrains.Annotations;

using CommonGames.Utilities;
using CommonGames.Utilities.Extensions;

using Physics = UnityEngine.Physics;

[RequireComponent(typeof(CharacterController))]
public class PlayerCore : Multiton<PlayerCore>
{
    #region Variables

    #region Component Accessors

    private CharacterController _motor = null;
    [PublicAPI]
    public CharacterController PlayerMotor => _motor = _motor.TryGetIfNull(context: this);
    
    private Camera _playerCamera = null;
    [PublicAPI]
    public Camera PlayerCamera => _playerCamera = _playerCamera.TryGetIfNull(context: this);
    
    private FPSCamera _playerFpsCamera = null;
    [PublicAPI]
    public FPSCamera PlayerFPSCamera => _playerFpsCamera = _playerFpsCamera.TryGetIfNull(context: this);

    #endregion

    #region Shown

    [Header("Player Movement")]
    public float speed = 12f;
    public float dampingVelocity = 0.4f;
    
    private bool enableMovement = true;
        
    [Header("Gravity")]
    public bool useGravity = true;
    [SerializeField] private readonly Vector3 groundCheckOffset = new Vector3(0,-1.35f,0);
    [SerializeField] private readonly float maxGroundDistance = 0.4f;
    
    [Header("Jump")]
    public float jumpHeight = 15f;
    
    [Header("Effects")]
    public ParticleSystem HyperDrive;
    public float maxFOV = 105;

    #endregion

    [HideInInspector] public float v, h;
    [HideInInspector] public Vector3 velocity, inputVelocity, gravityVector, explosionVelocity, slidingMomentum;

    private RaycastHit hitInfo;
    
    [HideInInspector] public bool isGrounded = false;
    private float slopeAngle, slidingTime;
    
    private float jumpForce;

    [HideInInspector] public float 
        FOVAim = 0f, 
        DefaultFOV = 0f;

    #endregion

    #region Methods

    [UsedImplicitly]
    private void Start()
    {
        //GrapplingHook.pvm = this;

        jumpForce = Mathf.Sqrt(-Physics.gravity.y * jumpHeight);
    }

    public void SetupFOV(in float DefFOV)
    {
        DefaultFOV = DefFOV;
        FOVAim = DefaultFOV;
    }

    [UsedImplicitly]
    private void Update()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        
        Transform __characterTransform = transform;

        if (enableMovement)
        {
            inputVelocity = (__characterTransform.forward * v + __characterTransform.right * h) * speed;

            Vector3 __groundCheckOrigin = __characterTransform.position + groundCheckOffset.Multiply(__characterTransform.localScale);
            
            if(Physics.Raycast(origin: __groundCheckOrigin, direction: Vector3.down, out hitInfo, maxGroundDistance))
            {
                isGrounded = true;

                gravityVector *= dampingVelocity;

                // Jumping
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    gravityVector.y += jumpForce;
                }
            }
            else
            {
                isGrounded = false;

                if (useGravity)
                    gravityVector += Physics.gravity * (Time.deltaTime * EffectsManager.currentTimeScale);
            }

            if (isGrounded)
            {
                if (hitInfo.distance < maxGroundDistance + maxGroundDistance)
                {
                    // Sliding
                    slopeAngle = Vector3.Angle(transform.up, hitInfo.normal);
                    
                    if (slopeAngle > PlayerMotor.slopeLimit)
                    {
                        slidingTime += Time.deltaTime * EffectsManager.currentTimeScale;
                        slidingMomentum += (hitInfo.normal - transform.up) * Mathf.Clamp01(slidingTime * 1.1f) * slopeAngle * 2;
                        inputVelocity *= .1f;

                        if(Input.GetKeyDown(KeyCode.Space))
                        {
                            gravityVector +=
                                (transform.up * slopeAngle * .7f +
                                 transform.forward * slopeAngle * .3f
                                ); // * Mathf.Clamp01(slidingTime / 1.2f);// * Mathf.Clamp01(slidingTime / 2);//hitInfo.normal * jumpForce * Mathf.Clamp01(slidingTime) * slopeAngle;
                        }
                    }
                }
            }
            if (slidingMomentum != Vector3.zero)
            {
                slidingTime = 0.0f;
                slidingMomentum -= slidingMomentum.normalized * (20 * Time.deltaTime * EffectsManager.currentTimeScale);
                
                if (slidingMomentum.sqrMagnitude < 0.02f)
                    slidingMomentum = Vector3.zero;
            }
        }
        
        t += Time.deltaTime * EffectsManager.currentTimeScale;
        if (t > .1f)
        {
            __distanceSquared = lastFramePosition.DistanceSquared(transform.position);
            if (__distanceSquared > 10 * EffectsManager.currentTimeScale)
            {
                if(!HyperDrive.isPlaying)
                {
                    HyperDrive.Play();
                }
            }
            else
            {
                FOVAim = DefaultFOV;
                if(HyperDrive.isPlaying)
                {
                    HyperDrive.Stop();
                }
            }

            FOVAim = __distanceSquared > 30 
                ? Mathf.Clamp(value: (90 + __distanceSquared / 8), min: DefaultFOV, max: maxFOV) 
                : DefaultFOV;

            t = 0.0f;
            lastFramePosition = transform.position;
        }
        
        PlayerMotor.Move((velocity + inputVelocity + gravityVector + slidingMomentum + explosionVelocity) * (Time.deltaTime * EffectsManager.currentTimeScale));
        velocity = Vector3.zero;

        if(explosionVelocity != Vector3.zero)
        {
            explosionVelocity -= explosionVelocity.normalized * (10 * Time.deltaTime);
            
            if(explosionVelocity.sqrMagnitude <= .001f)
            {
                explosionVelocity = Vector3.zero;
            }
        }

        PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, FOVAim, Time.deltaTime * EffectsManager.currentTimeScale * 8);
    }
    
    private Vector3 lastFramePosition;
    private float t = 0.0f, __distanceSquared;

    public void EnableMovement()
    {
        ToggleMovement(enabledState: true);
    }
    public void DisableMovement()
    {
        ToggleMovement(enabledState: false);
    }
    public void ToggleMovement(in bool enabledState)
    {
        enableMovement = enabledState;
        gravityVector = inputVelocity = slidingMomentum = explosionVelocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Transform __characterTransform = transform;
        
        Vector3 __groundCheckOrigin = (__characterTransform.position + groundCheckOffset.Multiply(__characterTransform.localScale));
        
        Gizmos.color = Color.yellow;;
        Gizmos.DrawSphere(center: __groundCheckOrigin, radius: 0.05f);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(from: __groundCheckOrigin, direction: Vector3.down * maxGroundDistance);

        if(!Physics.Raycast(origin: __groundCheckOrigin, direction: Vector3.down, out hitInfo, maxGroundDistance)) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(center: hitInfo.point, radius: 0.05f);

    }

    #endregion
}