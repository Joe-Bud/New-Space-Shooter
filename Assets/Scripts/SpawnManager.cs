using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject enemyContainer;

    public float spawnDelay;

    public float PUSpawnDelay;

    private bool stopSpawning = false;

    [SerializeField]
    GameObject [] powerUps;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Custom Methods

    #region IEnums && Spawn Routines

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(stopSpawning == false)
        {
            spawnDelay = Random.Range(3.0f, 5.0f);
            Vector3 enemySpawnPos = new Vector3(Random.Range(-8, 8), 8, 0);
            Instantiate(enemyPrefab, enemySpawnPos, Quaternion.identity, enemyContainer.transform);
            //newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(spawnDelay);
            
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(stopSpawning == false)
        {
            PUSpawnDelay = Random.Range(3.0f, 7.0f);
            Vector3 powerUpSpawnPos = new Vector3(Random.Range(-8, 8), 8, 0);
            int randomPU = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[randomPU], powerUpSpawnPos, Quaternion.identity);
            yield return new WaitForSeconds(PUSpawnDelay);
        }
    }

    #endregion

    #region Start && Stop Spawning Methods

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }

    public void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    #endregion

    #endregion
}
