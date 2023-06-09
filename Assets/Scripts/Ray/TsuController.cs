using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityUtility;
using static UnityEngine.InputSystem.InputAction;

namespace Tsutaeru
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Animator))]
    public class TsuController : MonoBehaviour
    {
        [SerializeField]
        float speed = 1f;

        [SerializeField]
        float rotationSpeed = 1f;

        [SerializeField]
        Transform receivedHiraganasParent;

        [SerializeField]
        float receivedHiraganasSpace = 2;

        [SerializeField]
        GameObject receivedHiraganaPrefab;

        [SerializeField]
        Vector2 receivedHiraganaScale = new Vector2(2, 2);

        public UnityEvent OnDamaged;
        public Action<string> OnHitHiragana;

        Vector2 targetPos;
        Collider2D col;
        Animator animator;
        bool isShowing = true;
        int boo_show;

        public void Init()
        {
            col = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
            boo_show = Animator.StringToHash(nameof(boo_show));
        }

        public void Hide()
        {
            Cursor.visible = true;
            col.isTrigger = true;
            isShowing = false;
            animator.SetBool(boo_show, false);
        }

        public void Show()
        {
            Cursor.visible = false;
            col.isTrigger = false;
            isShowing = true;
            animator.SetBool(boo_show, true);
        }

        public void Move(CallbackContext callback)
        {
            targetPos = Camera.main.ScreenToWorldPoint(callback.ReadValue<Vector2>());
            targetPos = new Vector2(targetPos.x, targetPos.y);
        }

        public void Rotate(CallbackContext callback)
        {
            var scrollValue = callback.ReadValue<Vector2>();

            if (scrollValue.y > 0)
                transform.localEulerAngles = new Vector3(0,0,transform.localEulerAngles.z+1f*rotationSpeed);
            if (scrollValue.y < 0)
                transform.localEulerAngles = new Vector3(0,0,transform.localEulerAngles.z-1f*rotationSpeed);
        }

        void Update()
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            OnDamaged.Invoke();

            var hiragana = collision.gameObject.GetComponent<HiraganaController>();
            if (hiragana != null)
                OnHitHiragana(hiragana.GetHiragana);

            Destroy(collision.gameObject);
        }

        public void AddHiragana(string hiragana)
        {
            var hiraganaGO = Instantiate(receivedHiraganaPrefab, receivedHiraganasParent);
            hiraganaGO.transform.localPosition = new Vector2(receivedHiraganasParent.childCount * receivedHiraganasSpace, 0);
            hiraganaGO.GetComponentInFamily<TextMeshProUGUI>().text = hiragana;
            hiraganaGO.transform.localScale = receivedHiraganaScale;
        }

        public void ClearHiraganas()
        {
            receivedHiraganasParent.DestroyChildren();
        }
    }
}
