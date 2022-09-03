using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float speed = 5.0f;

    private Vector3 startPos;

    private float horizontalInput;
    private float verticalInput;

    [SerializeField]
    private GameObject laserPrefab;

    [SerializeField]
    private GameObject tripleShotPrefab;

    [SerializeField]
    GameObject laserContainer;

    public bool isTripleShotActive = false;

    [SerializeField]
    private float fireRate = 0.5f;

    private float canFire = -1f;

    [SerializeField]
    private int lives = 3;

    private SpawnManager SM;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (SM == null)
            Debug.LogError("No sapwn Manager found");

        startPos = new Vector3(0, 0, 0);
        gameObject.transform.position = startPos;
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
                Instantiate(tripleShotPrefab, transform.position + offset, Quaternion.identity, laserContainer.transform);
            }
            else
            {
                Vector3 offset = new Vector3(0, 1.01f, 0);
                Instantiate(laserPrefab, transform.position + offset, Quaternion.identity, laserContainer.transform);
            }
        }
    }

    public void PlayerDamage()
    {
        lives--;

        if (lives <= 0)
        {
            SM.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
}
