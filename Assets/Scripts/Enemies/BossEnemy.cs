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
    private GameManager _gm;
    private Collider2D _collider;
    private int _direction = 0;

    // Start is called before the first frame update
    void Start()
    {
        AssignComponents();

        StartCoroutine(MoveToPosition());
    }

    private void AssignComponents()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("Boss Enemy could not find the UI Manager.");
        _gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gm == null)
            Debug.LogError("Boss Enemy could not find the Game Manager.");
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
            Debug.LogError("The Boss Enemy could not find it's collider");
        _shooter = GetComponentInChildren<Shooter>();
        if (_shooter == null)
            Debug.LogError("Boss Enemy could not find Shooter.");
    }

    // Update is called once per frame
    void Update()
    {
        if(_inPosition)
        {
            transform.Translate(_direction * Vector3.right * 3 * Time.deltaTime);
            if (transform.position.y >= 5.5f)
                _direction = -1;
            if (transform.position.y <= -3.5f)
                _direction = 1;
        }
    }

    private void CheckPosition()
    {
        if(transform.position == _startingPosition)
        {
            _inPosition = true;
            _uiManager.EnableBossLife();
            _collider.enabled = true;
            _shooter.enabled = true;
            StartCoroutine(ChooseDirection());
        }
    }

    private void TakeDamage()
    {
        _currentLife -= 10;
        _uiManager.UpdateBossHealth(_currentLife / 100);
        if (_currentLife == 0)
        {
            Destroy(this.gameObject);
            _gm.GameOver();
        }
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

    IEnumerator ChooseDirection()
    {
        while (_currentLife > 0)
        {

            Debug.Log(_direction);
            float rand = Random.Range(0f, 1f);
            _direction = rand >= 0.5f ? -1 : 1;
            yield return new WaitForSeconds(.75f);
        }
    }
}
