using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    #region Variables

    [SerializeField]
    private float rotateSpeed = 2.0f;

    [SerializeField]
    private GameObject explosion;

    private SpawnManager spawnTrigger;

    private AudioClips AC;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        spawnTrigger = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        AC = GameObject.Find("AudioManager").GetComponent<AudioClips>();

        if(spawnTrigger == null)
        {
            Debug.LogError("spawn Manager is NULL");
        }
        if(AC == null)
        {
            Debug.LogError("No audio source or clip");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            gameObject.GetComponent<Collider2D>().enabled = false;
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        AC.GetExplosionAudio();
        spawnTrigger.StartSpawning();
        Destroy(this.gameObject, 0.25f);
    }
}
