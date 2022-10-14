using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Variables

    [SerializeField]
    float speed = 8.0f;

    [SerializeField]
    GameObject tripleShot;

    bool isEnemyLaser = false;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LaserBehavior();
        DestroyLaser();
    }

    #endregion

    #region Custom Methods

    #region Laser Behaviors

    void LaserBehavior()
    {
        if (isEnemyLaser == false)
            ShootUp();
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

    #endregion

    #region Destroy Lasers

    void DestroyLaser()
    {
        float posY = transform.position.y;

        if (posY >= 8f || posY <= -8)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    #endregion

    #region Enemy Laser Behaviors && Triggers
    public void EnemyLaser()
    {
        isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            AudioClips MasterXploder = GameObject.Find("AudioManager").GetComponent<AudioClips>();

            if (player != null)
            {
                player.PlayerDamage();
                MasterXploder.PlayExplosionAudio();
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                Destroy(this.gameObject);
            }
        }
    }

    #endregion

    #endregion
}
