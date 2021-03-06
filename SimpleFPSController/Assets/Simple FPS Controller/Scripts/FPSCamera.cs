﻿using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FPSCamera : MonoBehaviour
{
    #region Variables

    public bool lockCursor = false;

    public float Sensitivity = 50;
    [HideInInspector] public float mouseX, mouseY;
    private float rotationX, rotationY;
    [HideInInspector] public float aimedZRotation = 0, rotationZ; // the rotation in z axis(mainly used for shake effect)

    public Transform Player;
    public Transform CameraPosition;
    
    #endregion
    
    #region Methods

    private void Awake()
    {
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
        WallRun.fpsCam = WeaponSway.fpsCam = this;
        PlayerMovement.cam = this.GetComponent<Camera>();
        TimeManager.postProcessing = this.GetComponent<UnityEngine.Rendering.Volume>();
        TimeManager.SetupPostProcessing();

        Player.GetComponent<PlayerMovement>().SetupFOV(PlayerMovement.cam.fieldOfView);
    }
    
    private void Update()
    {
        // Input from the mouse
        mouseX = Input.GetAxis("Mouse X") * Sensitivity * TimeManager.currentTimeScale;
        mouseY = Input.GetAxis("Mouse Y") * Sensitivity * TimeManager.currentTimeScale;
        
        // Calculating the rotation
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        rotationY += mouseX;

        // Rotating
        rotationZ = Mathf.Lerp(rotationZ, aimedZRotation, Time.deltaTime * 8);
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
        Player.Rotate(Vector3.up * mouseX);

        transform.position = CameraPosition.position;
    }
    
    #endregion
}
