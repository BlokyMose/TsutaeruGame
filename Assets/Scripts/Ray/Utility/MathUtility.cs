using System.Collections.Generic;
using UnityEngine;

namespace Encore.Utility
{
    public static class MathUtility
    {
        public struct Line
        {
            public Vector2 p1;
            public Vector2 p2;

            public Line(Vector2 p1, Vector2 p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }
        }
        public static Vector2? GetIntersection(Line lineA, Line lineB)
        {
            var x =
                ((lineA.p1.x * lineA.p2.y - lineA.p1.y * lineA.p2.x) * (lineB.p1.x - lineB.p2.x) - (lineA.p1.x - lineA.p2.x) * (lineB.p1.x * lineB.p2.y - lineB.p1.y * lineB.p2.x))
                /
                ((lineA.p1.x - lineA.p2.x) * (lineB.p1.y - lineB.p2.y) - (lineA.p1.y - lineA.p2.y) * (lineB.p1.x - lineB.p2.x));
            
            if (float.IsNaN(x))
                return null;

            var y =
                ((lineA.p1.x * lineA.p2.y - lineA.p1.y * lineA.p2.x) * (lineB.p1.y - lineB.p2.y) - (lineA.p1.y - lineA.p2.y) * (lineB.p1.x * lineB.p2.y - lineB.p1.y * lineB.p2.x))
                /
                ((lineA.p1.x - lineA.p2.x) * (lineB.p1.y - lineB.p2.y) - (lineA.p1.y - lineA.p2.y) * (lineB.p1.x - lineB.p2.x));

            if (float.IsNaN(y))
                return null;

            return new Vector2(x, y);
        }

        public static Vector2 ToVector2(this float angle)
        {
            return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }

        public static float ToAngle(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static List<int> GetRatio(List<int> list)
        {
            var lowestInt = GetTheLowestNumber(list);
            if (lowestInt < 0) 
                return list;

            var listResult = new List<int>(list);

            for (int i = 2; i < lowestInt; i++)
            {
                if (CanAllNumbersBeDividedBy(listResult, i))
                {
                    DivideAllNumbersBy(listResult, i);
                    i--;
                }
            }

            return listResult;

            int GetTheLowestNumber(List<int> allNumbers)
            {
                int lowestInt = allNumbers[0];
                for (int i = 1; i < allNumbers.Count; i++)
                    if (allNumbers[i] < lowestInt)
                        lowestInt = allNumbers[i];

                return lowestInt;
            }

            bool CanAllNumbersBeDividedBy(List<int> allNumbers, int divider)
            {
                foreach (var num in allNumbers)
                    if (num % divider != 0)
                        return false;
                return true;
            }

            void DivideAllNumbersBy(List<int> allNumbers, int divider)
            {
                for (int n = 0; n < allNumbers.Count; n++)
                    allNumbers[n] = allNumbers[n] / divider;
            }

        }

        public static string SecondsToTimeString(float seconds)
        {
            int minutes = (int)seconds / 60; // calculate the number of minutes
            int sec = (int)seconds % 60; // calculate the number of seconds
            return $"{minutes:D2}:{sec:D2}"; // return the time string in the format "mm:ss"
        }
    }
}
