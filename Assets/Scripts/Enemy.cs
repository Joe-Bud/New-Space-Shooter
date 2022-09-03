using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBehavior();
    }

    void EnemyBehavior()
    {
        float randPosX = Random.Range(-8, 8);
        float posY = transform.position.y;
        Vector3 newPos = new Vector3(randPosX, 8.0f, 0);

        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(posY < -5.8f)
        {
            transform.position = newPos;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.PlayerDamage();
                Destroy(this.gameObject);
            }
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
