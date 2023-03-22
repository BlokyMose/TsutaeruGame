using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{
    public class PlayerController : MonoBehaviour
    {

        public float rotateSpeed = 20;

        // X, Y座標の移動可能範囲
        [System.Serializable]
        public class Bounds
        {
            public float xMin, xMax, yMin, yMax;
        }
        [SerializeField] Bounds bounds;

        // 補間の強さ（0f～1f） 。0なら追従しない。1なら遅れなしに追従する。
        [SerializeField, Range(0f, 1f)] private float followStrength;

        private void Update()
        {
            // マウス位置をスクリーン座標からワールド座標に変換する
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // X, Y座標の範囲を制限する
            targetPos.x = Mathf.Clamp(targetPos.x, bounds.xMin, bounds.xMax);
            targetPos.y = Mathf.Clamp(targetPos.y, bounds.yMin, bounds.yMax);

            // Z座標を修正する
            targetPos.z = 0f;

            // このスクリプトがアタッチされたゲームオブジェクトを、マウス位置に線形補間で追従させる
            transform.position = Vector3.Lerp(transform.position, targetPos, followStrength);

            Rotation();
        }

        void Rotation()
        {
            float wh = Input.GetAxis("Mouse ScrollWheel");
            transform.Rotate(new Vector3(0f, 0f, wh * rotateSpeed));
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var enemyController = collision.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                Debug.Log(enemyController.GetHiragana);
            }
            Destroy(collision.gameObject);
        }
    }
}
