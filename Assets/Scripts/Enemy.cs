using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private GameObject laserPrefab;

    private float fireRate = 2.0f;
    private float canFire = -1.0f;
    private bool isDead = false;

    Player player;

    AudioClips AC;

    Animator anim;

    Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        AC = GameObject.Find("AudioManager").GetComponent<AudioClips>();

        if (AC == null)
            Debug.LogError("Enemy: AudioClips is NULL");

        if (player == null)
            Debug.LogError("Player is Null!!");

        anim = gameObject.GetComponent<Animator>();

        col = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBehavior();
        LaserBehavior();
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
            if(player != null)
            {
                player.PlayerDamage();
            }
            EnemyExploder();
            Destroy(this.gameObject, 2.8f);
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            player.AddPoints(10);
            EnemyExploder();
            Destroy(this.gameObject, 2.8f);
        }
    }

    void EnemyExploder()
    {
        isDead = true;
        anim.SetTrigger("OnEnemyDeath");
        speed = 0;
        col.enabled = false;
        AC.PlayExplosionAudio();
    }

    private void LaserBehavior()
    {
        if (Time.time > canFire && isDead == false)
        {
            fireRate = Random.Range(2.0f, 5.0f);
            canFire = Time.time + fireRate;
            GameObject EnemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            AC.PlayLaserAudio();

            Laser[] lasers = EnemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].EnemyLaser();
            }
        }
    }
}
