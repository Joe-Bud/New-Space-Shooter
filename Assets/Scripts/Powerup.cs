using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float speed = 3.0f;

    [SerializeField]
    private int powerupID;

    [SerializeField]
    private AudioClips AC;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        AC = GameObject.Find("AudioManager").GetComponent<AudioClips>();
    }

    // Update is called once per frame
    void Update()
    {
        PU_Movement();
    }

    #endregion

    #region Custom Methods

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
            player.AddPoints(5);

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
                        player.ActivateShields();
                        Debug.Log("shields");
                        break;

                    case 3:
                        player.AmmoReload();
                        Destroy(this.gameObject);
                        break;

                    default:
                        Debug.Log("Default value");
                        break;
                }
            }
            AC.PlayPowerupAudio();

            Destroy(this.gameObject);
        }
    }
    #endregion
}
