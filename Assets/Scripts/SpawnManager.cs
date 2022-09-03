using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject enemyContainer;

    public float spawnDelay;

    private bool stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemySpawnRoutine()
    {
        
        while(stopSpawning == false)
        {
            spawnDelay = Random.Range(3.0f, 5.0f);
            Vector3 enemySpawnPos = new Vector3(Random.Range(-8, 8), 8, 0);
            Instantiate(enemyPrefab, enemySpawnPos, Quaternion.identity, enemyContainer.transform);
            //newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(spawnDelay);
            
        }
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }
}
