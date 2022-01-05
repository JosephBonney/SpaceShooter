using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    void Update()
    {
        PowerupBehavior();
        DestroyPowerup();
    }

    void PowerupBehavior()
    {
         transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -5.3f)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.TripleShotActive();
                Destroy(this.gameObject);
            }
        }
    }

    void DestroyPowerup()
    {
        float posY = transform.position.y;
        if (posY <= -5.6f)
        {
            Destroy(this.gameObject);
        }
    }
}
