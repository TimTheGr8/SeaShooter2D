using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.x <= -9)
        {
            transform.position = new Vector3(11, Random.Range(-3f, 5.5f), 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.DamagePlayer();
            }
            else
            {
                Debug.Log("The Player script is missing.");
            }
            Destroy(this.gameObject);
        }

        if (other.tag == "Cannon Ball")
        {
            Destroy(other.gameObject);
            // Add points to player
            Destroy(this.gameObject);
        }
    }

}
