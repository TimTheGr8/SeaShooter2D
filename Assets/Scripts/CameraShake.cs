using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float _shakeDuration = 2.0f;
    [SerializeField]
    private float _shakeAmount = 0.7f;
    
    
    private float _shakeTimer;
    private Vector3 _originalCamPosition;

    // Start is called before the first frame update
    void Start()
    {
        _originalCamPosition = this.transform.localPosition;
    }

    public void ShakeCamera()
    {
        _shakeTimer = _shakeDuration;
        StartCoroutine(StartCameraShake());
    }

    IEnumerator StartCameraShake()
    {
        while (_shakeTimer > 0)
        {
            transform.localPosition = _originalCamPosition + Random.insideUnitSphere * _shakeAmount;
            _shakeTimer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _shakeTimer = 0;
        this.transform.position = _originalCamPosition;
    }
}
