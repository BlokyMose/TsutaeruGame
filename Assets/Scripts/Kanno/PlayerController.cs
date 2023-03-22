using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{
    public class PlayerController : MonoBehaviour
    {

        public float rotateSpeed = 20;

        // X, Y���W�̈ړ��\�͈�
        [System.Serializable]
        public class Bounds
        {
            public float xMin, xMax, yMin, yMax;
        }
        [SerializeField] Bounds bounds;

        // ��Ԃ̋����i0f�`1f�j �B0�Ȃ�Ǐ]���Ȃ��B1�Ȃ�x��Ȃ��ɒǏ]����B
        [SerializeField, Range(0f, 1f)] private float followStrength;

        private void Update()
        {
            // �}�E�X�ʒu���X�N���[�����W���烏�[���h���W�ɕϊ�����
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // X, Y���W�͈̔͂𐧌�����
            targetPos.x = Mathf.Clamp(targetPos.x, bounds.xMin, bounds.xMax);
            targetPos.y = Mathf.Clamp(targetPos.y, bounds.yMin, bounds.yMax);

            // Z���W���C������
            targetPos.z = 0f;

            // ���̃X�N���v�g���A�^�b�`���ꂽ�Q�[���I�u�W�F�N�g���A�}�E�X�ʒu�ɐ��`��ԂŒǏ]������
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
