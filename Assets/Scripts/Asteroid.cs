using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed = 0.5f;

    [SerializeField]
    private GameObject explosion;

    private SpawnManager spawnTrigger;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnTrigger = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if((spawnTrigger == null))
            Debug.LogError("Asteroid Start(): SpawnManager is Null");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Laser")
        {
            Destroy(collision.gameObject);
            StartSpawning();
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject, .25f);
    }

    private void StartSpawning()
    {
        spawnTrigger.StartSpawning();
    }
}
