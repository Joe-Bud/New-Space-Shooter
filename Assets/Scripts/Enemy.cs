using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables

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

    #endregion

    [ExecuteInEditMode]

    Transform _startPos;
    Transform _endPos;

    [SerializeField]
    [Range(0f, 1f)]
    //float _lerpPct = 0;
    public bool inBlackHole = false;

    //[SerializeField]
    private Transform _blackHole;

    #region BuiltIn Methods

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
        BlackHoleShrink();
    }

    #endregion

    #region Custom Methods

    #region Movement

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

    #endregion

    #region Trigger && Explosion

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

    #endregion

    #region Enemy Fire

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

    #endregion

    #endregion

    public void BlackHoleShrink()
    {
        if (inBlackHole == true)
        {
            float scalingFactor = -1.1f;
            transform.localScale = new Vector2(transform.localScale.x + transform.localScale.x * scalingFactor * Time.deltaTime, transform.localScale.y + transform.localScale.y * scalingFactor * Time.deltaTime);
            speed = .5f;
            canFire = 10000;
            //GravityPull();
        }
    }

    /*void GravityPull()
    {
        speed = .5f;
        canFire = 10000;
        _blackHole = GameObject.Find("BlackHole").GetComponent<Transform>();
        _startPos = this.gameObject.GetComponent<Transform>();
        _endPos = _blackHole.GetComponent<Transform>();
        this.gameObject.transform.position = Vector3.Lerp(_startPos.position, _endPos.position, _lerpPct + .01f);
        
    }*/
}
