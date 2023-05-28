using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [Tooltip ("0 = Triple Shot Crate\n1 = Speed Boost Crate\n2 = Shield Crate\n3 = Ammo Crate\n4 = Health Crate\n5 = Bomb Crate\n6 = TNT Crate")]
    [SerializeField]
    private int _powerupId;
    [SerializeField]
    private AudioClip _collectionClip;

    private Vector3 _directionOfTravel;

    private void Start()
    {
        _directionOfTravel = Vector3.left;
    }

    void Update()
    {
        transform.Translate(_directionOfTravel * _speed * Time.deltaTime);
        if (transform.position.x <= -10.75f)
            Destroy(this.gameObject);
    }

    public void MoveToTarget(Vector3 target)
    {
        _directionOfTravel = target - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_collectionClip,transform.position);
                switch (_powerupId)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    case 3:
                        player.AddAmmo();
                        break;
                    case 4:
                        player.AddHealth();
                        break;
                    case 5:
                        player.ActivateBombs();
                        break;
                    case 6:
                        player.DamagePlayer();
                        break;
                    default:
                        Debug.Log("The powerup does not have a correct ID.");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
