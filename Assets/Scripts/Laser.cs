using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Laser : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float speed = 8;

    #endregion

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
            
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}
