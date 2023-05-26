using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTNT : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.5f;
    [SerializeField]
    private float _tntTimer = 0.5f;
    [SerializeField]
    private float _bombRadius = 2.5f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private void Start()
    {
        StartCoroutine(DestroyDelay());
    }

    private void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
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
                if(player != null)
                {
                    player.DamagePlayer();
                }
            }
        }
        Destroy(bomb, 0.4f);
        Destroy(this.gameObject);
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(_tntTimer);
        Detonate();
    }
}
