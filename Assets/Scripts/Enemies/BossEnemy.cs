using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _projectilePrefabs = new List<GameObject>();
    [SerializeField]
    private Vector3 _startingPosition;
    [SerializeField]
    private GameObject _explosionPrefab;

    private bool _inPosition = false;
    private Shooter _shooter;
    private float _currentLife = 100;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("Boss Enemy could not find the UI Manager.");
        _shooter = GetComponentInChildren<Shooter>();
        if (_shooter == null)
            Debug.LogError("Boss Enemy could not find Shooter.");
        _shooter.gameObject.SetActive(false);

        StartCoroutine(MoveToPosition());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void CheckPosition()
    {
        if(transform.position == _startingPosition)
        {
            _inPosition = true;
            _uiManager.EnableBossLife();
        }
    }

    private void TakeDamage()
    {
        _currentLife -= 10;
        _uiManager.UpdateBossHealth(_currentLife / 100);
    }

    public List<GameObject> ChangeProjectile()
    {
        return _projectilePrefabs;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Cannon Ball")
        {
            CannonBall ball = other.GetComponent<CannonBall>();
            if(ball != null && !ball.IsEnemyCannonball())
            {
                TakeDamage();
                Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator MoveToPosition()
    {
        while(!_inPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startingPosition, 2.5f * Time.deltaTime);
            CheckPosition();
            yield return new WaitForEndOfFrame();
        }
    }
}
