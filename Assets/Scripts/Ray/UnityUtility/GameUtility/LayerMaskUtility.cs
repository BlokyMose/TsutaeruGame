using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityUtility
{
    public static class LayerMaskUtility
    {
        public static bool HasLayer(this LayerMask layerMask, int otherLayerValue)
        {
            var layerMaskValues = layerMask.GetMemberLayerValues();
            foreach (var value in layerMaskValues)
                if (value == otherLayerValue)
                    return true;
            return false;
        }

        public static List<int> GetMemberLayerValues(this LayerMask layerMask)
        {
            var result = new List<int>();

            double layerMaskValue = layerMask.value;
            var highestPower = Mathf.FloorToInt(Mathf.Log((float)layerMaskValue) / Mathf.Log(2));

            for (int i = highestPower; i >= 0; i--)
            {
                var powerValue = Math.Pow(2, i);
                if (layerMaskValue - powerValue >= 0)
                {
                    layerMaskValue -= powerValue;
                    result.Add((int)powerValue);
                    if (layerMaskValue <= 0) 
                        break;
                }
            }

            return result;
        }

        public static List<int> GetMemberLayerNumbers(this LayerMask layerMask)
        {
            List<int> numbers = new();
            var layerMaskValues = layerMask.GetMemberLayerValues();
            foreach (var value in layerMaskValues)
                numbers.Add((int)Mathf.Log(value, 2));

            return numbers;
        }
    }
}
