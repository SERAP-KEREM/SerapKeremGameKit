using SerapKeremGameKit._Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SerapKeremGameKit._Audio
{
    public sealed class AudioManager : MonoSingleton<AudioManager>
    {
        [Header("Registry")]
        [SerializeField] private List<AudioData> _audioList = new List<AudioData>();

        [Header("Pool")]
        [SerializeField] private AudioPool _pool;

        [Header("Music")]
        [SerializeField] private AudioSource _musicSource;

        private readonly Dictionary<string, AudioData> _keyToData = new Dictionary<string, AudioData>(StringComparer.Ordinal);

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;
            BuildRegistry();
            EnsurePool();
            EnsureMusicSource();
        }

        public void Play(string key)
        {
            PlayInternal(key, false, Vector3.zero);
        }

        public void PlayAt(string key, Vector3 worldPosition)
        {
            PlayInternal(key, true, worldPosition);
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;
            _musicSource.Stop();
            _musicSource.clip = clip;
            _musicSource.volume = 1f;
            _musicSource.loop = true;
            _musicSource.spatialBlend = 0f;
            _musicSource.Play();
        }

        public void StopMusic()
        {
            if (_musicSource != null)
            {
                _musicSource.Stop();
            }
        }

        private void PlayInternal(string key, bool is3D, Vector3 position)
        {
            if (!_keyToData.TryGetValue(key, out AudioData data) || data == null || data.Clip == null)
            {
                Debug.LogWarning("Audio key not found: " + key);
                return;
            }

            AudioPlayer source = _pool.Get();
            if (source == null) return;

            bool spatial = data.Spatial || is3D;
            Vector3? pos = is3D ? position : (Vector3?)null;
            source.Play(
                data.Clip,
                data.Volume,
                data.Pitch,
                data.Loop,
                spatial,
                pos,
                data.MinDistance,
                data.MaxDistance
            );

            if (!data.Loop)
            {
                StartCoroutine(RecycleWhenFinished(source));
            }
        }

        private IEnumerator RecycleWhenFinished(AudioPlayer src)
        {
            while (src != null && src.IsPlaying())
            {
                yield return null;
            }
            if (src != null)
            {
                _pool.Recycle(src);
            }
        }

        private void BuildRegistry()
        {
            _keyToData.Clear();
            for (int i = 0; i < _audioList.Count; i++)
            {
                AudioData data = _audioList[i];
                if (!string.IsNullOrEmpty(data.Key) && data.Clip != null)
                {
                    _keyToData[data.Key] = data;
                }
            }
        }

        private void EnsurePool()
        {
            if (_pool == null)
            {
                _pool = gameObject.GetComponent<AudioPool>();
                if (_pool == null)
                {
                    _pool = gameObject.AddComponent<AudioPool>();
                }
            }
        }

        private void EnsureMusicSource()
        {
            if (_musicSource == null)
            {
                _musicSource = gameObject.AddComponent<AudioSource>();
                _musicSource.playOnAwake = false;
                _musicSource.loop = true;
                _musicSource.spatialBlend = 0f;
            }
        }
    }
}

