using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _rotationSpeed = 0.25f;
    [SerializeField]
    private float _lifeTimer = 0.2f;
    [SerializeField]
    private float _explosionRadius = 5.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpriteRenderer _bombSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _bombSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_bombSpriteRenderer == null)
            Debug.LogError("There is no sprite renderer on the bomb.");

        StartCoroutine(LifeCountDown());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        _bombSpriteRenderer.transform.Rotate(new Vector3(0,0,-1) * _rotationSpeed * Time.deltaTime);
        if (transform.position.x > 11)
        {
            DestroyBomb();
        }
    }

    private void DestroyBomb()
    {
        _bombSpriteRenderer.enabled = false;
        GameObject bomb = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
        Collider2D[] other = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (var hitObject in other)
        {
            if(hitObject.tag == "Enemy")
            {
                Enemy enemy = hitObject.GetComponent<Enemy>();
                enemy.DestoryMe();
            }
        }
        Destroy(bomb, 0.4f);
        Destroy(this.gameObject);
    }

    IEnumerator LifeCountDown ()
    {
        yield return new WaitForSeconds(_lifeTimer);
        DestroyBomb();
    }
}
