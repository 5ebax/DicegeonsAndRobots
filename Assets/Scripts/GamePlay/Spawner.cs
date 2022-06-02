using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Created by Sebatián Jiménez F.
 * Class Spawner.
 */

public class Spawner : MonoBehaviour
{
    private System.Random _random = new System.Random();
    [Header("Spawns")]
    public int spawnNumber;
    public GameObject[] enemyPrefabs;
    public GameObject[] spawners;
    public bool isBoss;
    public int resultDice;
    private bool spawnedEnemies;
    private EnemyType enemyToSpawn;

    private DiceRoll dices;

    private void Awake()
    {
        dices = FindObjectOfType<DiceRoll>();
        if (spawnNumber <= 0) spawnNumber = 2;
        else if(spawnNumber > 6) spawnNumber = 6;
    }
    private void Update()
    {
        if (dices.resultEnemy != 0)
            resultDice = dices.resultEnemy;
        if (dices.resultEnemy != 0 && !spawnedEnemies)
            SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        spawnedEnemies = true;
        bool zipZap = false;
        switch (resultDice)
        {
            case 1://MeleeBot1
                enemyToSpawn = EnemyType.MeleeBot1;
                break;
            case 2://MeleeBot2
                enemyToSpawn = EnemyType.MeleeBot2;
                zipZap = true;
                break;
            case 3://RangedBot
                enemyToSpawn = EnemyType.RangedBot;
                break;
            case 4://Slime
                enemyToSpawn = EnemyType.Slime;
                break;
        }
        ShuffleSpawns();

        #region MegaIF
        int i = 0;
        int j = 0;
        foreach (var spawner in spawners)
        {
            if (j != spawnNumber && spawnNumber > 0)
            {//
                j++;
                if (zipZap)
                {
                    i++;
                    foreach (var enemy in enemyPrefabs)
                    {
                        if (i < (spawners.Length-spawnNumber) / 2)
                        {
                            if (enemy.GetComponent<Enemies>().enemyType == enemyToSpawn)
                                Instantiate(enemy, spawner.transform.position, Quaternion.identity);
                        }
                        else if (isBoss && i == spawners.Length)
                        {
                            if (enemy.GetComponent<Enemies>().enemyType == enemyToSpawn)
                            {
                                GameObject enemyBoss = Instantiate(enemy, spawner.transform.position, Quaternion.identity);
                                enemyBoss.GetComponent<Enemies>().boss = isBoss;
                            }
                        }
                        else
                        {
                            if (enemy.GetComponent<Enemies>().enemyType == EnemyType.MeleeBot1)
                                Instantiate(enemy, spawner.transform.position, Quaternion.identity);
                        }
                    }
                }
                else if (isBoss)
                {
                    i++;
                    foreach (var enemy in enemyPrefabs)
                    {
                        if (i == spawners.Length)
                        {
                            if (enemy.GetComponent<Enemies>().enemyType == enemyToSpawn)
                            {
                                GameObject enemyBoss = Instantiate(enemy, spawner.transform.position, Quaternion.identity);
                                enemyBoss.GetComponent<Enemies>().boss = isBoss;
                            }
                        }
                        else
                        {
                            if (enemy.GetComponent<Enemies>().enemyType == enemyToSpawn)
                                Instantiate(enemy, spawner.transform.position, Quaternion.identity);
                        }
                    }
                }
                else
                {
                    foreach (var enemy in enemyPrefabs)
                    {
                        if (enemy.GetComponent<Enemies>().enemyType == enemyToSpawn)
                            Instantiate(enemy, spawner.transform.position, Quaternion.identity);
                    }
                }

            }//


        }
        #endregion
    }

    private void ShuffleSpawns()
    {
        var p = spawners.Length;
        for (int n = p - 1; n > 0; n--)
        {
            var r = _random.Next(1, n);
            var t = spawners[r];
            spawners[r] = spawners[n];
            spawners[n] = t;
        }
    }
}
