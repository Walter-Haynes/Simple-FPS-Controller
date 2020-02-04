using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGames.Utilities.CGTK.Greasy;

public class NormalTween : MonoBehaviour
{
    //[SerializeField]
    private Camera _camera = null;
    [SerializeField] private float tweenDuration = 1f;

    [SerializeField] private AnimationCurve positionCurve = new AnimationCurve();
    
    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        if (_camera == null)
        {
            _camera = FindObjectOfType<Camera>();
        }
    }
    
    private void Update()
    {
        //camera.FieldOfViewTo(to: 25f, duration: 1f, ease: _animationCurve.Evaluate);
        
        //camera.FieldOfViewTo(to: 25f, duration: 1f, ease: EaseType.CircularIn);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.PositionTo(transform.position + Vector3.up, tweenDuration, positionCurve.Evaluate);
        }
    }
}
