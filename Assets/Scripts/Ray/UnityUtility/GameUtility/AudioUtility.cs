using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityUtility
{ 
    public static class AudioUtility
    {
        public static void SetFloatLog(this AudioMixer audioMixer, string paramName, float value, float multiplyBy = 20f)
        {
            if (value <= 0) value = 0.001f;
            audioMixer.SetFloat(paramName, Mathf.Log(value)*multiplyBy);
        }

        public static float GetFloatExp(this AudioMixer audioMixer, string paramName, float divideBy = 20f)
        {
            if (audioMixer.GetFloat(paramName, out var result))
                return Mathf.Exp(result / divideBy);

            return 0f;
        }
    }
}
