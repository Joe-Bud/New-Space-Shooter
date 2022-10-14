using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables 

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text gameOverText;

    [SerializeField]
    private TMP_Text restartLevelText;

    [SerializeField]
    private Image livesImg;

    [SerializeField]
    private Sprite[] livesSprites;

    [SerializeField]
    private Sprite[] ShieldLivesSprites;

    [SerializeField]
    private Image ShieldLivesImg;

    [SerializeField]
    private Sprite[] AmmoCountSprites;

    [SerializeField]
    private Image AmmoCountImg;

    [SerializeField]
    private TMP_Text WhiteAmmoCountText;

    [SerializeField]
    private TMP_Text RedAmmoCountText;

    private float flickerTime = 0.5f;

    Player player;

    GameManager GM;

    #endregion

    #region BuiltIn Methods 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (GM == null)
            Debug.LogError("Game Manager Is NULL!!");

        scoreText.text = "Score : " + player.score;
        gameOverText.enabled = false;
        restartLevelText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Custom Methods

    #region UI Update Methods

    #region Score

    public void UpdateScore(int playerScore)
    {
        scoreText.text = "Score : " + playerScore;
    }

    #endregion

    #region Lives

    public void UpdateLives(int currentLives)
    {
        livesImg.sprite = livesSprites[currentLives];

        if(currentLives <= 0)
        {
            GameOverMethod();
            GM.GameOver();
        }
    }

    #endregion

    #region Ammo

    public void UpdateAmmo(int AmmoCount)
    {
        AmmoCountImg.sprite = AmmoCountSprites[AmmoCount];
        WhiteAmmoCountText.text = ("Ammo " + AmmoCount);
        RedAmmoCountText.text = ("Ammo " + AmmoCount);
    }

    #endregion

    #region Shields

    public void UpdateShieldLives(int currentShieldLives)
    {
        ShieldLivesImg.sprite = ShieldLivesSprites[currentShieldLives];
    }

    #endregion

    #endregion

    #region Game Over

    public void GameOverMethod()
    {
        StartCoroutine(GameOverFlickerRoutine());
        restartLevelText.enabled = true;
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            gameOverText.enabled = true;
            yield return new WaitForSeconds(flickerTime);
            gameOverText.enabled = false;
            yield return new WaitForSeconds(flickerTime);   
        }

        
    }

    #endregion

    #endregion
}
