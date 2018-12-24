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
        [SerializeField] public AudioFile[] Files;

        private AudioState _state;

        private readonly Dictionary<string, AudioClip> _audioDictionary = new Dictionary<string, AudioClip>();
        private AudioSource _audioSource;


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
            _audioSource = GetComponent<AudioSource>();

            foreach (var file in Files) _audioDictionary[file.Name] = file.Clip;
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
                                StartCoroutine(FadeSound(_audioDictionary["victory"]));
                                break;
                            case 1:
                                StartCoroutine(FadeSound(_audioDictionary["epic"]));
                                break;
                            case 2:
                                StartCoroutine(FadeSound(_audioDictionary["newdawn"]));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    break;

                case AudioState.Fight:
                    StartCoroutine(FadeSound(_audioDictionary["instinct"]));
                    break;
                case AudioState.Lose:
                    StartCoroutine(FadeSound(_audioDictionary["losing"]));
                    break;
                default:
                    if (State == AudioState.Normal)
                        if (!_audioSource.isPlaying)
                            StartCoroutine(FadeSound(_audioDictionary["victory"]));
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(FadeSound(_audioDictionary["victory"]));
        }

        private IEnumerator FadeSound(AudioClip clip)
        {
            while (_audioSource.volume > 0)
            {
                _audioSource.volume -= 0.5f * Time.deltaTime;
                yield return null;
            }

            _audioSource.clip = clip;
            _audioSource.Play();

            while (_audioSource.volume < 1)
            {
                _audioSource.volume += 0.5f * Time.deltaTime;
                yield return null;
            }
        }

        public void Update()
        {
            if (!_audioSource.isPlaying) ChangeAudio();
        }

        [Serializable]
        public struct AudioFile
        {
            public string Name;
            public AudioClip Clip;
        }
    }
}