using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
    public sealed class SettingsScreen : UIScreen
    {
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Toggle _hapticToggle;

        private const string SoundKey = "skgk.settings.sound";
        private const string HapticKey = "skgk.settings.haptic";

        private void OnEnable()
        {
            if (_soundToggle != null) _soundToggle.isOn = PlayerPrefs.GetInt(SoundKey, 1) == 1;
            if (_hapticToggle != null) _hapticToggle.isOn = PlayerPrefs.GetInt(HapticKey, 1) == 1;
        }

        private void Awake()
        {
            if (_soundToggle != null) _soundToggle.onValueChanged.AddListener(OnSoundToggled);
            if (_hapticToggle != null) _hapticToggle.onValueChanged.AddListener(OnHapticToggled);
        }

        private void OnDestroy()
        {
            if (_soundToggle != null) _soundToggle.onValueChanged.RemoveListener(OnSoundToggled);
            if (_hapticToggle != null) _hapticToggle.onValueChanged.RemoveListener(OnHapticToggled);
        }

        private void OnSoundToggled(bool value)
        {
            PlayerPrefs.SetInt(SoundKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void OnHapticToggled(bool value)
        {
            PlayerPrefs.SetInt(HapticKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}




