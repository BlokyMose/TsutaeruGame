using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtility
{
    public static class BoxCollider2DUtility
    {

        #region [Get Side Points]

        public static Vector2 GetTopPos(this BoxCollider2D box)
        {
            return new Vector2(box.transform.position.x, box.transform.position.y + box.size.y / 2 * box.transform.localScale.y);
        }

        public static Vector2 GetBottomPos(this BoxCollider2D box)
        {
            return new Vector2(box.transform.position.x, box.transform.position.y - box.size.y / 2 * box.transform.localScale.y);
        }

        public static Vector2 GetRightPos(this BoxCollider2D box)
        {
            return new Vector2(box.transform.position.x + box.size.y / 2 * box.transform.localScale.x, box.transform.position.y);
        }

        public static Vector2 GetLeftPos(this BoxCollider2D box)
        {
            return new Vector2(box.transform.position.x - box.size.y / 2 * box.transform.localScale.x, box.transform.position.y);
        }


        #endregion

        #region [Get Corner Points]

        public static Vector2 GetTopLeftPos(this BoxCollider2D box)
        {
            return new Vector2(box.transform.position.x - box.size.y / 2 * box.transform.localScale.x, box.transform.position.y + box.size.y / 2 * box.transform.localScale.y);
        }

        public static Vector2 GetTopRightPos(this BoxCollider2D box)
        {
            return new Vector2(box.transform.position.x + box.size.y / 2 * box.transform.localScale.x, box.transform.position.y + box.size.y / 2 * box.transform.localScale.y);
        }

        public static Vector2 GetBottomLeftPos(this BoxCollider2D box)
        {
            return new Vector2(box.transform.position.x - box.size.y / 2 * box.transform.localScale.x, box.transform.position.y - box.size.y / 2 * box.transform.localScale.y);
        }

        public static Vector2 GetBottomRightPos(this BoxCollider2D box)
        {
            return new Vector2(box.transform.position.x + box.size.y / 2 * box.transform.localScale.x, box.transform.position.y - box.size.y / 2 * box.transform.localScale.y);
        }

        #endregion


        #region [Get Size]

        public static Vector2 GetScaledSize(this BoxCollider2D box)
        {
            return box.size * box.transform.localScale;
        }

        #endregion



    }
}
