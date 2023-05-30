using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private float _shotTimer = 0.5f;
    [SerializeField]
    private GameObject _waterSplash;
    [SerializeField]
    private float _splashTime = 0.2f;
    [SerializeField]
    private bool _isEnemyCannonball = false; 

    private void Start()
    {
        StartCoroutine(DestroyCountdown());
    }

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.x > 11 || transform.position.x < -9.5f)
        {
            DestroyCannonball();
        }
    }

    private void DestroyCannonball()
    {
        GameObject splash = Instantiate(_waterSplash, transform.position, Quaternion.identity);
        Destroy(splash, _splashTime);

        if (transform.parent.name == "Triple_Shot(Clone)")
            Destroy(transform.parent.gameObject);
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Powerup" && _isEnemyCannonball)
            Destroy(other.gameObject);
    }

    public bool IsEnemyCannonball()
    {
        return _isEnemyCannonball;
    }

    IEnumerator DestroyCountdown ()
    {
        yield return new WaitForSeconds(_shotTimer);
        DestroyCannonball();
    }
}
