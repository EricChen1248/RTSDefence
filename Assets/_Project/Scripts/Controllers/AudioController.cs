using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Controllers
{
    public class AudioController : MonoBehaviour
    {
        public enum AudioState
        {
            Normal,
            Fight,
            Lose
        }

        public static AudioController Instance;
        private AudioState _state;


        private readonly Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

        private AudioSource audioSource;

        [SerializeField] public AudioFile[] Files;

        public AudioState State
        {
            get { return _state; }
            set
            {
                _state = value;
                ChangeAudio();
            }
        }

        // Use this for initialization
        private void Start()
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();

            foreach (var file in Files) audioDictionary[file.Name] = file.Clip;
        }

        // Update is called once per frame
        private void ChangeAudio()
        {
            // If state = ?? 
            // If clip is not playing, play next clip

            switch (State)
            {
                case AudioState.Normal:
                    if (State == AudioState.Normal)
                        switch (Random.Range(0, 3))
                        {
                            case 0:
                                StartCoroutine(FadeSound(audioDictionary["victory"]));
                                break;
                            case 1:
                                StartCoroutine(FadeSound(audioDictionary["epic"]));
                                break;
                            case 2:
                                StartCoroutine(FadeSound(audioDictionary["newdawn"]));
                                break;
                        }
                    break;

                case AudioState.Fight:
                    if (State == AudioState.Fight) StartCoroutine(FadeSound(audioDictionary["instinct"]));
                    break;

                case AudioState.Lose:
                    if (State == AudioState.Lose) StartCoroutine(FadeSound(audioDictionary["losing"]));
                    break;

                default:
                    if (State == AudioState.Normal)
                        if (!audioSource.isPlaying)
                            StartCoroutine(FadeSound(audioDictionary["victory"]));
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(FadeSound(audioDictionary["victory"]));
        }

        private IEnumerator FadeSound(AudioClip clip)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= 0.5f * Time.deltaTime;
                yield return null;
            }

            audioSource.clip = clip;
            audioSource.Play();

            while (audioSource.volume < 1)
            {
                audioSource.volume += 0.5f * Time.deltaTime;
                yield return null;
            }
        }

        public void Update()
        {
            if (!audioSource.isPlaying) ChangeAudio();
        }

        [Serializable]
        public struct AudioFile
        {
            public string Name;
            public AudioClip Clip;
        }
    }
}