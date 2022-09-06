using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    float speed = 8.0f;

    [SerializeField]
    GameObject tripleShot;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LaserBehavior();
    }

    void LaserBehavior()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 8.0f)
        {
            if(transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        }


    }
}
