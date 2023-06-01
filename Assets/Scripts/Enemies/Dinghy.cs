using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinghy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private float _bombRadius = 2.5f;
    [SerializeField]
    private float _rotationModifier = 0;


    private Transform _target;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.Find("Player").GetComponent<Transform>();
        if (_target == null)
            Debug.LogError("The Dinghy cannot find a target.");
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            Vector3 vectorToTarget = _target.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, (vectorToTarget.x)) * Mathf.Rad2Deg - _rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 4f);
        }
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void Detonate()
    {
        GameObject bomb = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
        Collider2D[] other = Physics2D.OverlapCircleAll(transform.position, _bombRadius);
        foreach (var hitObject in other)
        {
            if (hitObject.tag == "Player")
            {
                Player player = hitObject.GetComponent<Player>();
                if (player != null)
                {
                    player.DamagePlayer();
                }
            }
        }
        Destroy(bomb, 0.4f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.DamagePlayer();
                Destroy(this.gameObject);
            }
        }

        if(other.tag == "Cannon Ball")
        {
            CannonBall ball = other.GetComponent<CannonBall>();
            if(ball != null && !ball.IsEnemyCannonball())
            {
                Detonate();
                Destroy(other.gameObject);
            }
        }

    }
}
