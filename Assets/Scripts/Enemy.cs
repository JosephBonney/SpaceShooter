using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;

namespace SpaceShooter.Enemy
{
    public class Enemy : MonoBehaviour
    {
        #region Variables

        public float speed = -4f;

        Player.Player player;

        private Animator anim;

        private Collider2D col;

        private AudioClips AC;

        [SerializeField]
        private GameObject laser;

        [SerializeField]
        private float FireRate = 3.0f;
        private float canFire = -1.0f;
        private bool isDead = false;

        #endregion

        #region BuiltIn Methods

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player.Player>();

            AC = GameObject.Find("AudioManager").GetComponent<AudioClips>();

            if(player == null)
            {
                Debug.LogError("Player is NULL");
            }

            anim = gameObject.GetComponent<Animator>();

            if(anim == null)
            {
                Debug.LogError("Animator is NULL");
            }

            if(AC == null)
            {
                Debug.LogError("No Audio Source or clip");
            }

            col = gameObject.GetComponent<Collider2D>();
            col.enabled = true;


        }

        // Update is called once per frame
        void Update()
        {
            EnemyBehavior();

            if(Time.time > canFire && isDead == false)
            {
                FireRate = Random.Range(2.0f, 5.0f);
                canFire = Time.time + FireRate;
                GameObject EnemyLaser = Instantiate(laser, transform.position, Quaternion.identity);
                AC.GetLaserAudioClip();
                Laser[] lasers = EnemyLaser.GetComponentsInChildren<Laser>();

                for(int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].EnemyLaser();
                }
            }
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

        #endregion


        #region Custom Methods

        #region Trigger

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Hit: " + other.transform.name);

            if(other.tag == "Player")
            {
                if(player != null)
                {
                    player.Damage();
                }
                EnemyExploder();

                Destroy(this.gameObject, 2.8f);
            }

            if (other.tag == "Laser")
            {
                Destroy(other.gameObject);
                if(player != null)
                {
                    player.AddScore(10);
                }
                EnemyExploder();

                Destroy(this.gameObject, 2.8f);
            }
        }

        #endregion

        void EnemyExploder()
        {
            isDead = true;
            anim.SetTrigger("OnEnemyDeath");
            col.enabled = false;
            speed = 0;
            AC.GetExplosionAudio();
        }

        #endregion
    }
}
