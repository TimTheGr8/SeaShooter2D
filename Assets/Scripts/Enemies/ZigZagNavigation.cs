using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagNavigation : MonoBehaviour
{
    [SerializeField]
    private float _turnTimeMin = 0.5f;
    [SerializeField]
    private float _turnTimeMax = 1.0f;
    [SerializeField]
    private float _maxAngle = 120.0f;
    [SerializeField]
    private float _minAngle = 75.0f;

    void Start()
    {
        StartCoroutine(StartZigZag());
    }

    private float RandomFloat(float min, float max)
    {
        float newFloat = Random.Range(min, max);
        return newFloat;
    }

    IEnumerator StartZigZag()
    {
        while (true)
        {
            transform.rotation = Quaternion.Euler(0, 0, RandomFloat(_minAngle, _maxAngle));
            yield return new WaitForSeconds(RandomFloat(_turnTimeMin, _turnTimeMax));
        }
    }
}
