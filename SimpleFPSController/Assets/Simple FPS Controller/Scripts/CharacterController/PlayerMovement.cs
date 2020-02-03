using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    
    public static CharacterController characterController;
    public static Camera cam;

    [Header("Player Movement")]
    public float speed = 12f;
    public float dampingVelocity = 0.4f;
    
    private bool enableMovement = true;
    
    [HideInInspector] public float v, h;
    [HideInInspector] public Vector3 velocity, inputVelocity, gravityVector, explosionVelocity, slidingMomentum;

    [Header("Gravity")]
    public bool useGravity = true;
    public Vector3 groundCheckOffset = new Vector3(0,-1.35f,0);
    public float maxGroundDistance = 0.2f;
    
    private RaycastHit hitInfo;
    
    [HideInInspector]
    public bool isGrounded = false;
    private float slopeAngle, slidingTime;

    [Header("Jump")]
    public float jumpHeight = 15f;
    private float jumpForce;

    [Header("Effects")]
    public ParticleSystem HyperDrive;

    [HideInInspector]
    public float FOVAim = 0.0f, DefaultFOV;
    public float maxFOV = 105;
    
    #endregion

    #region Methods

    private void Start()
    {
        characterController = this.GetComponent<CharacterController>();

        GrapplingHook.pvm = this;

        jumpForce = Mathf.Sqrt(-Physics.gravity.y * jumpHeight);
    }

    public void SetupFOV(float DefFOV)
    {
        DefaultFOV = DefFOV;
        FOVAim = DefaultFOV;
    }

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
                    if (slopeAngle > characterController.slopeLimit)
                    {
                        slidingTime += Time.deltaTime * EffectsManager.currentTimeScale;
                        slidingMomentum += (hitInfo.normal - transform.up) * Mathf.Clamp01(slidingTime * 1.1f) * slopeAngle * 2;
                        inputVelocity *= .1f;

                        if (Input.GetKeyDown(KeyCode.Space))
                            gravityVector += (transform.up * slopeAngle * .7f + transform.forward * slopeAngle * .3f);// * Mathf.Clamp01(slidingTime / 1.2f);// * Mathf.Clamp01(slidingTime / 2);//hitInfo.normal * jumpForce * Mathf.Clamp01(slidingTime) * slopeAngle;
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
            dSqr = GrapplingHook.DistanceSquared(lastFramePosition, transform.position);
            if (dSqr > 10 * EffectsManager.currentTimeScale)
            {
                if (!HyperDrive.isPlaying)
                    HyperDrive.Play();
            }
            else
            {
                FOVAim = DefaultFOV;
                if (HyperDrive.isPlaying)
                    HyperDrive.Stop();
            }

            FOVAim = dSqr > 30 
                ? Mathf.Clamp(value: (90 + dSqr / 8), min: DefaultFOV, max: maxFOV) 
                : DefaultFOV;

            t = 0.0f;
            lastFramePosition = transform.position;
        }
        
        characterController.Move((velocity + inputVelocity + gravityVector + slidingMomentum + explosionVelocity) * (Time.deltaTime * EffectsManager.currentTimeScale));
        velocity = Vector3.zero;

        if(explosionVelocity != Vector3.zero)
        {
            explosionVelocity -= explosionVelocity.normalized * (10 * Time.deltaTime);
            if (explosionVelocity.sqrMagnitude <= .001f)
                explosionVelocity = Vector3.zero;
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, FOVAim, Time.deltaTime * EffectsManager.currentTimeScale * 8);
    }
    
    private Vector3 lastFramePosition;
    private float t = 0.0f, dSqr;

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

        if(Physics.Raycast(origin: __groundCheckOrigin, direction: Vector3.down, out hitInfo, maxGroundDistance))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(center: hitInfo.point, radius: 0.05f);
        }

    }

    #endregion
}