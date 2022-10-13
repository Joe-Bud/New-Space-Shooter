using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float speed = 3.5f;
    private float originalSpeed;
    [SerializeField]
    public float newspeed = 8.5f;

    private bool speedBoostIsActive = false;

    private Vector3 startPos;

    private float horizontalInput;
    private float verticalInput;

    [SerializeField]
    private GameObject laserPrefab;

    [SerializeField]
    private GameObject tripleShotPrefab;
    private GameObject tripleShot;

    public bool isTripleShotActive = false;

    [SerializeField]
    private float TripleShotCoolDownRate = 5.0f;
    [SerializeField]
    private float SpeedBoostCoolDownRate = 5.0f;

    [SerializeField]
    private float fireRate = 0.5f;

    private float canFire = -1f;

    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private GameObject Shields;

    private bool shieldsActive = false;

    public int score = 0;

    private UIManager uiManager;

    private SpawnManager SM;

    [SerializeField]
    private GameObject[] Engines;

    [SerializeField]
    private AudioClips AC;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = speed;

        SM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        AC = GameObject.Find("AudioManager").GetComponent<AudioClips>();

        if (AC == null)
            Debug.LogError("Player: AudioClips Is NULL");

        if (uiManager == null)
            Debug.LogError("The UI Manager Is NULL");

        if (SM == null)
            Debug.LogError("No Spawn Manager Found");

        startPos = new Vector3(0, 0, 0);
        gameObject.transform.position = startPos;

        Shields.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        FireLaser();
    }

    void PlayerMovement()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3 (horizontalInput, verticalInput, 0) * speed * Time.deltaTime);

        // Don't get why I couldn't use yPos right here though. It froze my player unless I typed transform.position.y in the clamp
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.1f, 0), 0); 

        if(xPos > 9.75f)
        {
            //xPos = -11.5f;
            transform.position = new Vector3(-9.75f, transform.position.y, 0);
        }
            

        else if (xPos < -9.75f)
        {
            //xPos = 11.5f;
            transform.position = new Vector3 (9.75f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if(Input.GetButtonDown("Fire1") && Time.time > canFire || Input.GetButtonDown("Jump") && Time.time > canFire)
        {
            canFire = Time.time + fireRate;
            
            if(isTripleShotActive == true)
            {
                Vector3 offset = new Vector3(1.69966674f, 0.427324027f, 0);
                tripleShot = Instantiate(tripleShotPrefab, transform.position + offset, Quaternion.identity);
            }
            else
            {
                Vector3 offset = new Vector3(0, 1.01f, 0);
                Instantiate(laserPrefab, transform.position + offset, Quaternion.identity);
            }

            AC.PlayLaserAudio();
        }
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(TripleShotCoolDownRate);
        isTripleShotActive = false;
        StopCoroutine(TripleShotPowerDownRoutine());
    }

    public void ActivateTripleShot()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void ActivateSpeedBoost()
    {
        speedBoostIsActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
        if (speedBoostIsActive == true)
        {
            speed = newspeed;
        }
        else
            speed = originalSpeed;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(SpeedBoostCoolDownRate);
        speed = originalSpeed;
        speedBoostIsActive = false;
    }

    public void ActivateShields()
    {
        Shields.SetActive(true);
        shieldsActive = true;
    }

    public void PlayerDamage()
    {
        GameObject RandomEngine = Engines[Random.Range(0, Engines.Length)];

        GameObject R_Engine = Engines[0];
        GameObject L_Engine = Engines[1];

        if (shieldsActive == true)
        {
            shieldsActive = false;
            Shields.SetActive(false);
            return;
        }

        lives--;

        if(lives == 2)
        {
            RandomEngine.SetActive(true);
        }

        if(lives == 1)
        {
            R_Engine.SetActive(true);
            L_Engine.SetActive(true);
        }

        uiManager.UpdateLives(lives);

        if (lives <= 0)
        {
            SM.OnPlayerDeath();
            Destroy(this.gameObject);
        }

        AC.PlayExplosionAudio();
    }

    public void AddPoints(int points)
    {
        score += points;
        uiManager.UpdateScore(score);
    }
}

