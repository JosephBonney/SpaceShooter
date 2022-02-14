using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;


public class Laser : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float speed = 8;

    private bool isEnemyLaser = false;

    #endregion

    // Update is called once per frame
    void Update()
    {
        DestroyLaser();

        if (isEnemyLaser == false)
        {
            ShootUp();
        }
        else
            ShootDown();
    }

    void ShootUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void ShootDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void DestroyLaser()
    {
        float posY = transform.position.y;

        if(posY >= 8f || posY <= -8)
        {
            
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void EnemyLaser()
    {
        isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            AudioClips MasterXploder = GameObject.Find("AudioManager").GetComponent<AudioClips>();

            if (player != null)
            {
                player.Damage();
                MasterXploder.GetExplosionAudio();
            }
        }
    }
}
