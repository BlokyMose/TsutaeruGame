using Encore.Utility;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.UI;
using Random = UnityEngine.Random;

namespace UnityUtility
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceRandom : MonoBehaviour
    {
        [Serializable]
        public class AudioPack
        {
            public string packName = "";

            [LabelText("Possible Clips")]
            public List<AudioClip> clips = new List<AudioClip>();

            [HorizontalGroup("Volume", 0.66f)]
            public float volume = 1f;
            
            [HorizontalGroup("Volume"), LabelText("+/-"), LabelWidth(25)]
            public float volumeRandomRange = 0f;

            [HorizontalGroup("Pitch", 0.66f)]
            public float pitch = 1f;

            [HorizontalGroup("Pitch"), LabelText("+/-"), LabelWidth(25)]
            public float pitchRandomRange = 0.15f;

            [HorizontalGroup("Delay", 0.66f)]
            public float delay = 0f;

            [HorizontalGroup("Delay"), LabelText("+/-"), LabelWidth(25)]
            public float delayRandomRange = 0f;

            public IEnumerator Play(AudioSource audioSource)
            {
                var _delay = Random.Range(delay - delayRandomRange, delay + delayRandomRange);
                yield return new WaitForSeconds(_delay);
                audioSource.pitch = Random.Range(pitch - pitchRandomRange, pitch + pitchRandomRange);

                if (clips.GetRandom() == null)
                    Debug.Log(audioSource.gameObject.name);
                audioSource.PlayOneShot(clips.GetRandom(), Random.Range(volume - volumeRandomRange, volume + volumeRandomRange));
            }

            public IEnumerator PlayAllClips(AudioSource audioSource)
            {
                var _delay = Random.Range(delay - delayRandomRange, delay + delayRandomRange);
                yield return new WaitForSeconds(_delay);
                foreach (var clip in clips)
                {
                    audioSource.pitch = Random.Range(pitch - pitchRandomRange, pitch + pitchRandomRange);
                    audioSource.PlayOneShot(clip, Random.Range(volume - volumeRandomRange, volume + volumeRandomRange));
                }
            }
        }


        [SerializeField, Range(0,1)]
        float playProbability = 1f;

        [SerializeField]
        List<AudioPack> audioPacks = new();

        [FoldoutGroup("Audio Source"), SerializeField]
        AudioClip audioClip;

        [FoldoutGroup("Audio Source"), SerializeField]
        AudioMixerGroup output;

        [FoldoutGroup("Audio Source"), SerializeField]
        bool mute;

        [FoldoutGroup("Audio Source"), SerializeField]
        bool bypassEffect;

        [FoldoutGroup("Audio Source"), SerializeField]
        bool bypassListenerEffect;

        [FoldoutGroup("Audio Source"), SerializeField]
        bool bypassReverbZones;

        [HorizontalGroup("Audio Source/Awake"), SerializeField, LabelText("Play On")]
        bool playOnAwake = true;

        [HorizontalGroup("Audio Source/Awake", 80f), SerializeField, LabelWidth(1f)]
        UnityInitialMethod invokeIn = UnityInitialMethod.Awake;

        [FoldoutGroup("Audio Source"), SerializeField]  
        bool loop;

        [FoldoutGroup("Audio Source"), SerializeField, ShowIf(nameof(loop))]
        float loopPeriod = 3f;

        [FoldoutGroup("Audio Source"), SerializeField, Range(0,256)]
        int priority = 128;

        [FoldoutGroup("Audio Source"), SerializeField, Range(0,1)]
        float volume = 1;

        [FoldoutGroup("Audio Source"), SerializeField, Range(-3,3)]
        float pitch = 1;

        [FoldoutGroup("Audio Source"), SerializeField, Range(-1,1)]
        float stereoPan = 0;

        [FoldoutGroup("Audio Source"), SerializeField, Range(0,1)]
        float spatialBlend;

        [FoldoutGroup("Audio Source"), SerializeField, Range(0,1.1f)]
        float reverbZoneMix = 1;

        AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            SyncAudioSourceProperties();

            if (playOnAwake && invokeIn == UnityInitialMethod.Awake)
            {
                if (loop)
                    PlayLoop();
                else
                    Play();
            }
        }

        void OnEnable()
        {
            if (playOnAwake && invokeIn == UnityInitialMethod.OnEnable)
            {
                if (loop)
                    PlayLoop();
                else
                    Play();
            }
        }

        void Start()
        {
            if (playOnAwake && invokeIn == UnityInitialMethod.Start)
            {
                if (loop)
                    PlayLoop();
                else
                    Play();
            }
        }

        [FoldoutGroup("Audio Source"), Button("Sync")]
        void SyncAudioSourceProperties()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = output;

            audioSource.mute = mute;
            audioSource.bypassEffects = bypassEffect;
            audioSource.bypassListenerEffects = bypassListenerEffect;
            audioSource.bypassReverbZones = bypassReverbZones;
            audioSource.playOnAwake = playOnAwake;
            audioSource.loop = loop;

            audioSource.priority = priority;
            audioSource.volume = volume;
            audioSource.pitch= pitch;
            audioSource.panStereo = stereoPan;
            audioSource.spatialBlend = spatialBlend;
            audioSource.reverbZoneMix = reverbZoneMix;
        }

        [HorizontalGroup("Buttons"), Button, PropertyOrder(-1)]
        public void Play()
        {
#if UNITY_EDITOR
            audioSource = GetComponent<AudioSource>();
#endif
            var probability = Random.Range(0f, 1f);
            if (probability <= playProbability)
                foreach (var pack in audioPacks)
                    StartCoroutine(pack.Play(audioSource));
        }

        public void PlayLoop()
        {
            StartCoroutine(Looping());
            IEnumerator Looping()
            {
                Play();

                var time = 0f;
                while (true)
                {
                    if (time > loopPeriod)
                    {
                        Play();
                        time = 0f;
                    }

                    time += Time.deltaTime;
                    yield return null;
                }
            }

        }

        public void PlayOneClipFromPack(string packName)
        {
            foreach (var pack in audioPacks)
            {
                if(pack.packName == packName)
                {
                    StartCoroutine(pack.Play(audioSource));
                    break;
                }

            }
        }

        public void PlayAllClipsFromPack(string packName)
        {
            foreach (var pack in audioPacks)
            {
                if (pack.packName == packName)
                {
                    StartCoroutine(pack.PlayAllClips(audioSource));
                    break;
                }

            }
        }
    }
}
