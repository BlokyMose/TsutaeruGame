using Encore.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UnityUtility
{
    public class GOSpreader : MonoBehaviour
    {
        public enum OriginMode { Center, End }

        [InlineButton(nameof(SetChildrenAsGOs),"Add Children", ShowIf = "@gos.Count == 0")]
        public List<Transform> gos = new();

        //[Button("Add All Children", ButtonSizes.Large), PropertyOrder(-1), ShowIf("@gos.Count == 0")]
        public void SetChildrenAsGOs()
        {
            for (int i = 0; i < transform.childCount; i++)
                gos.AddIfHasnt(transform.GetChild(i));
        }

        [InlineButton(nameof(SetLocalRotation), "Set"), Range(0,360)]
        public float localRotationZ = 0f;
        public void SetLocalRotation() { foreach (var go in gos) go.localEulerAngles = new Vector3(0,0,localRotationZ); }

        // ===
        // ===
        // ===

        [TitleGroup("Spread Settings")]
        [LabelText("Origin"), OnValueChanged(nameof(Spread))]
        public OriginMode originMode;

        [TitleGroup("Spread Settings"), OnValueChanged(nameof(Spread))]
        public float space = 2f;

        [TitleGroup("Spread Settings")]
        [PropertyRange(0f, 360f), OnValueChanged(nameof(Spread))]
        public float angle = 0f;

        [HorizontalGroup("Spread Settings/Other Parent",0.3f), LabelText("Override Parent"), ToggleLeft]
        public bool isUsingOtherAsParent = false;

        [HorizontalGroup("Spread Settings/Other Parent")]
        [EnableIf(nameof(isUsingOtherAsParent)), LabelWidth(0.1f), OnValueChanged(nameof(Spread))]
        public Transform parent;

        [TitleGroup("Spread Settings")]
        [ToggleLeft, GUIColor("@"+nameof(isAutoSpreading)+"?Encore.Utility.ColorUtility.mediumSpringGreen : Color.gray")]
        public bool isAutoSpreading = true;

        public void Spread()
        {
            if (!isAutoSpreading) return;

            if (parent == null || !isUsingOtherAsParent) parent = transform;

            int index = 0;

            foreach (var go in gos)
            {
                go.position = parent.position;
                go.position = (Vector2)go.position + (index * space * angle.ToVector2());
                if (originMode == OriginMode.Center)
                    go.position = (Vector2)go.position - ((gos.Count - 1 - index) * space * angle.ToVector2());

                index++;
            }
        }

        // ===
        // ===
        // ===

        [TitleGroup("Debug", order: 100)]
        public Sprite debugSprite;
        [TitleGroup("Debug")]
        public float debugSpriteScale = 1f;
        public const string DEBUG_SPRITE = "DebugSprite";
        
        [ButtonGroup("Debug/Sprite But"), GUIColor("@Encore.Utility.ColorUtility.mediumSpringGreen")]
        [Button("Add")]
        public void AddDebugSprite()
        {
            if (debugSprite == null)
            {
#if UNITY_EDITOR
                var circleGUIDs = UnityEditor.AssetDatabase.FindAssets("t:Sprite circle");
                if (circleGUIDs.Length > 0)
                {
                    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(circleGUIDs[0]);
                    debugSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
                }
#endif
            }    
            foreach (var go in gos)
            {
                if (go.Find(DEBUG_SPRITE))
                    continue;
                var debugSpriteGO = new GameObject(DEBUG_SPRITE);
                debugSpriteGO.transform.localScale = new Vector3(debugSpriteScale, debugSpriteScale, debugSpriteScale);
                debugSpriteGO.transform.parent = go;
                debugSpriteGO.transform.localPosition = Vector2.zero;
                debugSpriteGO.transform.localEulerAngles = Vector2.zero;
                var sr = debugSpriteGO.AddComponent<SpriteRenderer>();
                sr.sprite = debugSprite;
            }
        }

        [ButtonGroup("Debug/Sprite But"), GUIColor("@Encore.Utility.ColorUtility.salmon")]
        [Button("Remove")]
        public void RemoveDebugSprite()
        {
            foreach (var go in gos)
            {
                var debugSpriteGO = go.Find(DEBUG_SPRITE);
                if (debugSpriteGO != null)
                    DestroyImmediate(debugSpriteGO.gameObject);
            }
        }

    }
}
