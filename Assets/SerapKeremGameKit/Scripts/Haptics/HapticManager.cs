using SerapKeremGameKit._Logging;
using SerapKeremGameKit._Singletons;
using UnityEngine;

namespace SerapKeremGameKit._Haptics
{
    public sealed class HapticManager : MonoSingleton<HapticManager>
    {
        [SerializeField] private bool _enabled = true;
        [SerializeField, Range(0f, 1f)] private float _globalIntensity = 1f;
        [SerializeField, Range(0f, 1f)] private float _cooldownSeconds = 0.05f;

        private float _nextAllowedTime;

        protected override void Awake()
        {
            base.Awake();
        }

        public void SetEnabled(bool isEnabled)
        {
            _enabled = isEnabled;
        }

        public void SetGlobalIntensity(float intensity)
        {
            _globalIntensity = Mathf.Clamp01(intensity);
        }

        public void Play(HapticType type)
        {
            if (!_enabled) return;
            if (Time.unscaledTime < _nextAllowedTime) return;
            _nextAllowedTime = Time.unscaledTime + _cooldownSeconds;

#if UNITY_ANDROID || UNITY_IOS
            PlayMobile(type, _globalIntensity);
#else
            // Editor/Unsupported: no-op (or optional log)
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        private static void PlayMobile(HapticType type, float intensity)
        {
            // Minimal cross-platform fallback; replace with platform plugin if needed
            switch (type)
            {
                case HapticType.Selection:
                case HapticType.Light:
                    Handheld.Vibrate();
                    break;
                case HapticType.Medium:
                case HapticType.Success:
                case HapticType.Warning:
                case HapticType.Failure:
                case HapticType.Heavy:
                    Handheld.Vibrate();
                    break;
                default:
                    break;
            }
        }
#endif
    }
}


