using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtility
{
    public static class DebugLog
    {
        public enum ColorName
        {
            Red,
            Green,
            Blue,
            Cyan,
            Yellow,
            Magenta
        }

        public static string ToText(this ColorName colorName)
        {
            return colorName switch
            {
                ColorName.Red => "red",
                ColorName.Green => "green",
                ColorName.Blue => "blue",
                ColorName.Cyan => "cyan",
                ColorName.Yellow => "yellow",
                ColorName.Magenta => "magenta",
                _=>""
            };
        }

        public static void Color(ColorName colorName, string coloredText, string text="", string separator = " : ")
        {
            Debug.Log($"<color={colorName.ToText()}>{coloredText}</color>{separator}{text}");
        }

        public static void Big(string bigText, string text="", int size = 20, string separator = " : ")
        {
            Debug.Log($"<size={size}>{bigText}</size>{separator}{text}");
        }

        public static void Notice(string noticeText, string text="", int size = 20, ColorName colorName = ColorName.Yellow, string separator = " : ")
        {
            Debug.Log($"<color={colorName.ToText()}><size={size}>{noticeText}</size></color>{separator}{text}");
        }
    }
}
