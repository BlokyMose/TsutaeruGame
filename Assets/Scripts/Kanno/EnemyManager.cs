using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> enemyList;    // 生成オブジェクト
        [SerializeField] Transform pos;                 // 生成位置
        [SerializeField] Transform pos2;                // 生成位置
        float minX, maxX, minY, maxY;                   // 生成範囲

        int frame = 0;
        [SerializeField] int generateFrame = 30;        // 生成する間隔

        void Start()
        {
            minX = Mathf.Min(pos.position.x, pos2.position.x);
            maxX = Mathf.Max(pos.position.x, pos2.position.x);
            minY = Mathf.Min(pos.position.y, pos2.position.y);
            maxY = Mathf.Max(pos.position.y, pos2.position.y);
        }

        void Update()
        {
            ++frame;

            if (frame > generateFrame)
            {
                frame = 0;

                // ランダムで種類と位置を決める
                int index = Random.Range(0, enemyList.Count);
                float posX = Random.Range(minX, maxX);
                float posY = Random.Range(minY, maxY);

                var enemyGO = Instantiate(enemyList[index], new Vector3(posX, posY, 0), Quaternion.identity);
                enemyGO.SetActive(true);
            }
        }
    }
}
