using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNavigation : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _navigationPoints = new List<GameObject>();
    [SerializeField]
    private float _turnSpeed = 1.0f;
    [SerializeField]
    private float _rotationModifier;


    private GameObject _destination;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject nav in GameObject.FindGameObjectsWithTag("NavigationPoint"))
        {
            _navigationPoints.Add(nav);
        }
        NewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        TurnShip();
        if(CheckDistance() <= 1.5f)
        {
            NewDestination();
        }
    }

    private void TurnShip()
    {
        Vector3 vectorToTarget = _destination.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _turnSpeed);
    }

    private void NewDestination()
    {
        int newTarget = Random.Range(0, _navigationPoints.Count);
        _destination = _navigationPoints[newTarget];
        Debug.Log(_destination);
    }

    private float CheckDistance()
    {
        float distance = Vector3.Distance(_destination.transform.position, transform.position);
        return distance;
    }
}
