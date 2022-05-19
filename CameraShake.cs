using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform _camera;
    private Vector3 _startPos;
    [SerializeField] private float _shakePower;
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _downAmount;
    public bool isShake;
    public static CameraShake Instance;
    private float _initialDuration;


    void Start()
    {
        if (Camera.main != null) _camera = Camera.main.transform;
        _startPos = _camera.localPosition;
        _initialDuration = _shakeDuration;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShake)
        {
            if (_shakeDuration > 0)
            {
                _camera.localPosition = _startPos + Random.insideUnitSphere * _shakePower;
                _shakeDuration -= _downAmount * Time.deltaTime;
            }
            else
            {
                isShake = false;
                _camera.localPosition = _startPos;
                _shakeDuration = _initialDuration;
            }
        }
    }
}