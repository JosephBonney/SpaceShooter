using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    public float waitTime = 5.0f;

    [SerializeField]
    private GameObject [] Powerups;

    [SerializeField]
    private GameObject EnemyContainer;

    private bool stopSpawing = false;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    #endregion

    #region Ienums

    #region EnemySpawn

    IEnumerator SpawnEnemyRoutine()
    {
        float posY = transform.position.y;
        posY = 7.5f;
        float randomX = Random.Range(-8f, 8f);

        while(stopSpawing == false)
        {
            Vector3 SpawnPos = new Vector3(randomX, posY, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, SpawnPos, Quaternion.identity);
            newEnemy.transform.parent = EnemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }

    #endregion

    #region PowerupSpawn

    IEnumerator SpawnPowerupRoutine()
    {
        float powerupPosY = transform.position.y;
        powerupPosY = 7.5f;

        while(stopSpawing == false)
        {
            Vector3 powerupSpawnPos = new Vector3(Random.Range(-8f, 8f), powerupPosY, 0);
            GameObject newPowerup = Instantiate(Powerups[Random.Range(0, Powerups.Length)], powerupSpawnPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    #endregion

    #endregion

    #region Custom Methods

    public void OnPlayerDeath()
    {
        stopSpawing = true;
    }

    #endregion
}
