using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;

    [SerializeField]
    private int powerupID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PU_Movement();
    }

    void PU_Movement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(transform.position.y < -5.0f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player == null)
                Debug.LogError("Player Script is NULL");

            if(player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1: 
                        player.ActivateSpeedBoost();
                        Debug.Log("Speed");
                        break;
                    case 2:
                        Debug.Log("shields");
                        break;

                    default:
                        Debug.Log("Default value");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
