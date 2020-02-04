﻿using UnityEngine;

namespace SimpleFPSController.PlayerSystems.Weapons
{
    using CommonGames.Utilities.Extensions;
    using SimpleFPSController.PlayerSystems.Movement;

    public class WeaponSway : PlayerBehaviour
    {
        #region Variables

        private Vector3 AimedPosition, InitialPosition;

        public float maxSwayAmount;

        public float SmoothAmount = 3.0f, CameraSmoothAmount = 10;
        private float movementX, movementY;

        #endregion

        #region Methods

        private void Start()
        {
            InitialPosition = transform.localPosition;
            AimedPosition = InitialPosition;
        }

        private void Update()
        {
            movementX = Mathf.Clamp(Player.PlayerFPSCamera.mouseX, -maxSwayAmount, maxSwayAmount);
            movementY = Mathf.Clamp(Player.PlayerFPSCamera.mouseY, -maxSwayAmount, maxSwayAmount);

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(movementX, movementY, 0) + InitialPosition, Time.deltaTime * CameraSmoothAmount);
            transform.localPosition = Vector3.Lerp(transform.localPosition, AimedPosition, SmoothAmount * 10 * Time.deltaTime);

            if(!RecoilTimer.Approximately(0f)) return;
            RecoilTimer -= Time.deltaTime;

            if(!(RecoilTimer <= 0)) return;
            
            AimedPosition = InitialPosition;
            RecoilTimer = 0;
        }

        private float RecoilTimer = 0f;
        public void Recoil()
        {
            AimedPosition = transform.localPosition + Vector3.back * .07f;
            RecoilTimer = .05f;
        }
    
        #endregion
    }   
}
