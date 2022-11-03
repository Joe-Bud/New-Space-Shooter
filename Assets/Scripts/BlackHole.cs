using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    Player player;
    GameObject[] _enemies;
    Transform _startPos;
    Transform _endPos;
    float _lerpPct = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(var enemy in _enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            _startPos = enemy.GetComponent<Transform>();
            _endPos = this.gameObject.transform;
            enemy.transform.position = Vector3.Lerp(_startPos.position, _endPos.position, _lerpPct + .01f);
            enemyScript.inBlackHole = true;
            Destroy(enemy, 3);
        }
            
    }


    /*private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy _enemies = other.GetComponent<Enemy>();
        if (_enemies != null)
        {
            _enemies.inBlackHole = true;
            Destroy(_enemies.gameObject, 3);
            player.AddPoints(10);
        }
        else
            return;

    }*/

    
}
