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

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        spawnTrigger = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if(spawnTrigger == null)
        {
            Debug.LogError("spawn Manager is NULL");
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
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        spawnTrigger.StartSpawning();
        Destroy(this.gameObject, 0.25f);
    }
}
