using SerapKeremGameKit._Logging;
using SerapKeremGameKit._Singletons;
using System.Collections;
using UnityEngine;

namespace SerapKeremGameKit._Time
{
    public sealed class TimeManager : MonoSingleton<TimeManager>
    {
        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField, Range(0f, 2f)] private float _defaultTimeScale = 1f;
        [SerializeField] private float _defaultFixedDeltaTime = 0.02f; // 50 Hz

        public bool IsPaused { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;

            Application.targetFrameRate = _targetFrameRate;
            SetFixedDeltaTime(_defaultFixedDeltaTime);
            ResetTimeScale();
        }

        public void SetTargetFrameRate(int fps)
        {
            if (fps < 15) fps = 15;
            Application.targetFrameRate = fps;
        }

        public void SetTimeScale(float scale)
        {
            float clamped = Mathf.Max(0f, scale);
            Time.timeScale = clamped;
            IsPaused = clamped == 0f;
        }

        public void ResetTimeScale()
        {
            SetTimeScale(_defaultTimeScale);
        }

        public void Pause()
        {
            SetTimeScale(0f);
        }

        public void Resume()
        {
            ResetTimeScale();
        }

        public void SetFixedDeltaTime(float seconds)
        {
            if (seconds <= 0f) seconds = 0.02f;
            Time.fixedDeltaTime = seconds;
        }

        public float ScaledDeltaTime()
        {
            return Time.deltaTime;
        }

        public float UnscaledDeltaTime()
        {
            return Time.unscaledDeltaTime;
        }

        public IEnumerator WaitSeconds(float seconds)
        {
            if (seconds <= 0f) yield break;
            yield return new WaitForSeconds(seconds);
        }

        public IEnumerator WaitSecondsUnscaled(float seconds)
        {
            if (seconds <= 0f) yield break;
            yield return new WaitForSecondsRealtime(seconds);
        }
    }
}


