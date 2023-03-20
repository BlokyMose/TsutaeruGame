using Encore.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsuMovement : MonoBehaviour
{
    [SerializeField]
    GameObject tsuPrefab;

    [SerializeField]
    GameObject tsuKatakanaPrefab;

    [SerializeField]
    float spawnEvery = 1f;

    [SerializeField]
    Vector2 speed = new Vector2(1, 1);

    [SerializeField]
    float initialKillDelay = 10f;

    float spawnCooldown = 0f;
    float initialKillCooldown = 0f;
    List<GameObject> tsus = new();
    bool canKillPeriodically;
    int spawnTsuKatakanaEvery = 6;
    int spawnTsuKatakanaCooldown = 0;



    void Update()
    {
        spawnCooldown += Time.deltaTime;

        if (spawnCooldown > spawnEvery)
        {
            var _tsuPrefab = this.tsuPrefab;

            spawnTsuKatakanaCooldown++;
            if (spawnTsuKatakanaCooldown > spawnTsuKatakanaEvery)
            {
                _tsuPrefab = this.tsuKatakanaPrefab;
                spawnTsuKatakanaCooldown = 0;
            }

            var tsuGO = Instantiate(_tsuPrefab, transform);
            tsus.Add(tsuGO);
            spawnCooldown = 0f;

            if (canKillPeriodically)
            {
                Destroy(tsus[0]);
                tsus.RemoveAt(0);
                initialKillCooldown = 0f;
            }
        }

        if (initialKillCooldown > initialKillDelay)
            canKillPeriodically = true;
        else
            initialKillCooldown += Time.deltaTime;

        foreach (var tsu in tsus)
        {
            tsu.transform.position = new(tsu.transform.position.x + speed.x * Time.deltaTime, tsu.transform.position.y + speed.y * Time.deltaTime);
        }

        
    }
}
