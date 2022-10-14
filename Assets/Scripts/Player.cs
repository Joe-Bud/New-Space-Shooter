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

    [HideInInspector]
    public bool IsShieldActive = false;

    [SerializeField]
    Color[] ShieldColors;

    [HideInInspector]
    public bool ShieldDestroyed = false;

    public int ShieldHits = 2;

    private SpriteRenderer NewShieldColor;

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
    public int maxAmmo = 15;

    public int currentAmmo;

    [HideInInspector]
    public bool hasAmmo = true;

    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private GameObject Shields;

    public int score = 0;

    private UIManager uiManager;

    private SpawnManager SM;

    [SerializeField]
    private GameObject[] Engines;

    [SerializeField]
    private AudioClips AC;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = speed;

        SM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        AC = GameObject.Find("AudioManager").GetComponent<AudioClips>();

        Shields.GetComponent<SpriteRenderer>().enabled = false;
        Shields.GetComponent<Collider2D>().enabled = false;
        ShieldHits = 0;

        currentAmmo = maxAmmo;

        if (AC == null)
            Debug.LogError("Player: AudioClips Is NULL");

        if (uiManager == null)
            Debug.LogError("The UI Manager Is NULL");

        if (SM == null)
            Debug.LogError("No Spawn Manager Found");

        if (NewShieldColor == null)
        {
            Debug.Log("There are no shields on start");
        }

        startPos = new Vector3(0, 0, 0);
        gameObject.transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        FireLaser();
        ButtonBoost();
    }

    #endregion

    #region Custom Methods

    #region Movement

    void PlayerMovement()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime);

        // Don't get why I couldn't use yPos right here though. It froze my player unless I typed transform.position.y in the clamp
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.1f, 0), 0);

        if (xPos > 9.75f)
        {
            //xPos = -11.5f;
            transform.position = new Vector3(-9.75f, transform.position.y, 0);
        }


        else if (xPos < -9.75f)
        {
            //xPos = 11.5f;
            transform.position = new Vector3(9.75f, transform.position.y, 0);
        }
    }

    #endregion

    #region Button Boost

    void ButtonBoost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed = speed * 2;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed = originalSpeed;
    }

    #endregion

    #region Laser

    void FireLaser()
    {
        AmmoCount();

        if (Input.GetButtonDown("Fire1") && hasAmmo == false || Input.GetButtonDown("Jump") && hasAmmo == false)
        {
            AC.GetNoAmmoAudio();
        }

        if (Input.GetButtonDown("Fire1") && Time.time > canFire && hasAmmo == true|| Input.GetButtonDown("Jump") && Time.time > canFire && hasAmmo == true)
        {
            currentAmmo -= 1;

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
        uiManager.UpdateAmmo(currentAmmo);
    }

    #endregion

    #region IEnums && Powerups

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(TripleShotCoolDownRate);
        isTripleShotActive = false;
        StopCoroutine(TripleShotPowerDownRoutine());
    }

    #region TripleShot

    public void ActivateTripleShot()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    #endregion

    #region SpeedBoost

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

    #endregion

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(SpeedBoostCoolDownRate);
        speed = originalSpeed;
        speedBoostIsActive = false;
    }

    #region Shields

    public void ActivateShields()
    {
        TurnShieldComponentsOn();
        NewShieldColor = Shields.GetComponent<SpriteRenderer>();
        IsShieldActive = true;
        ShieldHits = 3;
        NewShieldColor.color = ShieldColors[0];
        uiManager.UpdateShieldLives(3);
    }

    void TurnShieldComponentsOn()
    {
        Shields.GetComponent<Collider2D>().enabled = true;
        Shields.GetComponent<SpriteRenderer>().enabled = true;
    }

    void TurnShieldComponentsOff()
    {
        Shields.GetComponent<Collider2D>().enabled = false;
        Shields.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ShieldDamage()
    {
        ShieldHits -= 1;
    }

    #endregion

    #region AmmoBehavior

    public void AmmoReload()
    {
        currentAmmo = maxAmmo;
        uiManager.UpdateAmmo(currentAmmo);
        hasAmmo = true;
    }
    public void AmmoCount()
    {
        if (currentAmmo <= 0)
        {
            currentAmmo = 0;
            hasAmmo = false;
            Debug.Log("No Ammo");
        }
        else
        {
            hasAmmo = true;
        }
    }

    #endregion

    #endregion

    #region Damage

    public void PlayerDamage()
    {
        GameObject RandomEngine = Engines[Random.Range(0, Engines.Length)];

        GameObject R_Engine = Engines[0];
        GameObject L_Engine = Engines[1];

        if (IsShieldActive == true)
        {

            TurnShieldComponentsOn();
            NewShieldColor = Shields.GetComponent<SpriteRenderer>();
            if (AC == null)
            {
                Debug.LogError("No Audio source or clip");
            }
            if (ShieldHits == 3)
            {
                ShieldDamage();
                ShieldDestroyed = false;
                NewShieldColor.color = ShieldColors[0];
                uiManager.UpdateShieldLives(3);
            }
            if (ShieldHits == 2)
            {
                ShieldDamage();
                ShieldDestroyed = false;
                NewShieldColor.color = ShieldColors[1];
                uiManager.UpdateShieldLives(2);
                return;
            }
            if (ShieldHits == 1)
            {
                ShieldDamage();
                ShieldDestroyed = false;
                NewShieldColor.color = ShieldColors[2];
                uiManager.UpdateShieldLives(1);
                return;
            }
            else
            {
                IsShieldActive = false;
                TurnShieldComponentsOff();
                Debug.Log("ShieldDestroyed");
                uiManager.UpdateShieldLives(0);
                return;
            }
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

    #region Add Health

    public void AddHealth()
    {
        GameObject RandomEngine = Engines[Random.Range(0, Engines.Length)];

        GameObject R_Engine = Engines[0];
        GameObject L_Engine = Engines[1];

        if (lives < 3)
        {
            lives++;
            RandomEngine.SetActive(false);
        }

        else
            lives = 3;
        uiManager.UpdateLives(lives);
    }

    #endregion

    #endregion

    #region Points

    public void AddPoints(int points)
    {
        score += points;
        uiManager.UpdateScore(score);
    }

    #endregion

    #endregion
}

