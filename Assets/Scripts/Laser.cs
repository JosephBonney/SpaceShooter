using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Laser : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float speed = 8;

    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DestroyLaser();
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void DestroyLaser()
    {
        float posY = transform.position.y;
        if(posY >= 8f)
        {
            Destroy(this.gameObject);
        }
    }
}
