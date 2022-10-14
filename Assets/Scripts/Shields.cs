using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour
{
    public int ShieldHits = 3;

    private SpriteRenderer NewShieldColor;

    public bool ShieldDestroyed = false;

    private Color StartingShieldColor;
    private Player player;


    // Start is called before the first frame update
    void Start()
    {
        NewShieldColor = GetComponent<SpriteRenderer>();
        StartingShieldColor = GetComponent<SpriteRenderer>().color;
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("No Player Script");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShieldColor()
    {
        if (ShieldDestroyed == false)
        {
            if (NewShieldColor == null)
            {
                Debug.LogError("No Shields");
            }

            if (ShieldHits == 3)
            {
                NewShieldColor.color = StartingShieldColor;
                return;
            }

            if (ShieldHits == 2)
            {
                NewShieldColor.color = Color.yellow;
                return;
            }

            if (ShieldHits == 1)
            {
                NewShieldColor.color = Color.red;
                return;
            }

            if (ShieldHits >= 0)
            {
                ShieldDestroyed = true;
                player.IsShieldActive = false;
                this.gameObject.SetActive(false);
            }
        }
    }
    public void ShieldDamage()
    {
        ShieldHits -= 1;
        ShieldColor();
    }
}
