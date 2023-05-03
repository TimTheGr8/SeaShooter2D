using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private float _timeToWait = 0.5f;

    void Start()
    {
        Destroy(this.gameObject, _timeToWait);
    }
}
