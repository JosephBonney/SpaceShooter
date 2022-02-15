using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    private Player player;

    [SerializeField] // 0 = Triple Shot, 1 = Speed Boost, 2 = Shields
    private int powerupID;

    private AudioClips AC;

    private ShieldBehavior SB;

    void Start()
    {
        AC = GameObject.Find("AudioManager").GetComponent<AudioClips>();
        if(AC == null)
        {
            Debug.LogError("No AC");
        }
    }

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

        Player player = other.transform.GetComponent<Player>();
      
        if (other.tag == "Player")
        {
            if (player == null)
            {
                Debug.LogError("No player component");
            }

            if (player != null)
            {
                AC.GetPowerUpAudio();
                player.AddScore(15);

                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        Destroy(this.gameObject);
                        break;

                    case 1:
                        player.SpeedBoostActive();
                        Destroy(this.gameObject);
                        break;
                    case 2:
                        player.ShieldsActive();
                        Destroy(this.gameObject);
                        break;
                        

                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
        }
    }

    public void DestroyPowerup()
    {
        float posY = transform.position.y;
        if (posY <= -5.6f)
        {
            Destroy(this.gameObject);
        }
    }
}
