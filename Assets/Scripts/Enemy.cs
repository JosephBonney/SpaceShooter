using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;

namespace SpaceShooter.Enemy
{
    public class Enemy : MonoBehaviour
    {
        public float speed = -4f;

        Player.Player player;

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player.Player>();
        }

        // Update is called once per frame
        void Update()
        {
            EnemyBehavior();
        }

        void EnemyBehavior()
        {
            float posX = transform.position.x;
            float posY = transform.position.y;
            float randomX = Random.Range(-8f, 8f);

            transform.Translate(Vector3.down * speed * Time.deltaTime);
            
            if(posY <= -5.3f)
            {
                posY = 7.5f;
                transform.position = new Vector3 (randomX, posY, 0);
            }
            
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Hit: " + other.transform.name);

            if(other.tag == "Player")
            {
                if(player != null)
                {
                    player.Damage();
                }

                Destroy(this.gameObject);
            }

            if (other.tag == "Laser")
            {
                Destroy(other.gameObject);
                if(player != null)
                {
                    player.AddScore(10);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
