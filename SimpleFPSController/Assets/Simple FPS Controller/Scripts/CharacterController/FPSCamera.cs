using System;
using UnityEngine;

using SimpleFPSController.PlayerSystems.Movement;

[RequireComponent(typeof(Camera))]
public class FPSCamera : PlayerBehaviour
{
    #region Variables

    public bool lockCursor = false;

    public float Sensitivity = 50;
    [HideInInspector] public float mouseX, mouseY;
    private float rotationX, rotationY;
    [HideInInspector] public float aimedZRotation = 0, rotationZ; // the rotation in z axis(mainly used for shake effect)
    
    public Transform CameraPosition;
    
    #endregion
    
    #region Methods

    protected override void Awake()
    {
        base.Awake();
        
        Application.targetFrameRate = 90;
    }

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        EffectsManager.postProcessing = this.GetComponent<UnityEngine.Rendering.Volume>();
        EffectsManager.SetupPostProcessing();

        //TODO: Redo this:
        //Player.SetupFOV(Player.fieldOfView);
    }
    
    private void Update()
    {
        // Input from the mouse
        mouseX = Input.GetAxis("Mouse X") * Sensitivity * EffectsManager.currentTimeScale;
        mouseY = Input.GetAxis("Mouse Y") * Sensitivity * EffectsManager.currentTimeScale;
        
        // Calculating the rotation
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        rotationY += mouseX;

        // Rotating
        rotationZ = Mathf.Lerp(rotationZ, aimedZRotation, Time.deltaTime * 8);
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
        
        //TODO: Redo this:
        //Player.Rotate(Vector3.up * mouseX);

        transform.position = CameraPosition.position;
    }
    
    #endregion
}