using UnityEngine;

[RequireComponent(typeof(FPSCamera))]
public class GrapplingHook : MonoBehaviour
{
    #region Variables

    private Vector3 grapplingLocation;
    private readonly Vector3 zeroVector = Vector3.zero;
    private RaycastHit hitInfo;
    private float distance;

    public float maxGrapplingDistance = 100;
    public float hookSpeed = 10;
    public float dampSpeed = 3; // Momentum(Vector3) slows down using damp speed
    public CharacterController PlayerCC;
    public static PlayerMovement pvm;
    [HideInInspector] public Vector3 momentum, dir;

    public static WallRun wr;
    public LineRenderer lr;
    
    #endregion

    #region Methods

    private void Start()
    {
        WallRun.gHook = this;
    }

    public static float DistanceSquared(in Vector3 P1, in Vector3 P2)
    {
        return (P1.x - P2.x) * (P1.x - P2.x) + (P1.y - P2.y) * (P1.y - P2.y) + (P1.z - P2.z) * (P1.z - P2.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxGrapplingDistance))
            {
                grapplingLocation = hitInfo.point;
                pvm.DisableMovement();
                momentum = zeroVector;
                wr.EndWallRun();
                
                __DecreaseMagnitude();
            }
            
            __DecreaseMagnitude();
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (grapplingLocation != zeroVector)
            {
                Vector3 __playerPosition = transform.position;
                
                distance = DistanceSquared(grapplingLocation, __playerPosition);

                dir = (grapplingLocation - __playerPosition).normalized;
                momentum = dir * (hookSpeed * Mathf.Clamp01(distance));
                lr.SetPosition(0, __playerPosition - Vector3.up);
                lr.SetPosition(1, grapplingLocation);

                __DecreaseMagnitude();
            }
            //return;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if(grapplingLocation != zeroVector) Unhook();
            
            __DecreaseMagnitude();
        }

        void __DecreaseMagnitude()
        {
            if(!(momentum.sqrMagnitude >= 0.0f)) return;
            
            momentum -= momentum * (dampSpeed * Time.deltaTime * EffectsManager.currentTimeScale);
            if (momentum.sqrMagnitude < 0.0f)
                momentum = zeroVector;
            pvm.velocity += momentum;
            //PlayerCC.Move(momentum * Time.deltaTime);
        }
    }

    public void Unhook()
    {
        grapplingLocation = zeroVector;
        
        pvm.EnableMovement();
        
        lr.SetPosition(0, zeroVector);
        lr.SetPosition(1, zeroVector);
    }
    
    #endregion
}