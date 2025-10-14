using SerapKeremGameKit._Audio;
using SerapKeremGameKit._Haptics;
using SerapKeremGameKit._InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
    public class SettingsScreen : UIScreen
    {
        [Header("UI Components")]
        [SerializeField] private Transform _background;
        [SerializeField] private Transform _panel;

        [Header("Toggle Controls")]
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Toggle _hapticToggle;

        [Header("Buttons")]
        [SerializeField] private Button _closebutton;

        private float _previousTimeScale = 1f;

        protected override void Awake()
        {
            base.Awake();
            SetupEventListeners();
        }

        private void SetupEventListeners()
        {
            _closebutton.onClick.AddListener(Close);
            _soundToggle.onValueChanged.AddListener(ToggleSound);
            _hapticToggle.onValueChanged.AddListener(ToggleHaptic);
        }

        protected override void Start()
        {
            base.Start();
            InitializeToggleStates();
        }

        private void InitializeToggleStates()
        {
            // There is no global mute in AudioManager; keep as 'on' by default or wire to your own setting
            _soundToggle.SetIsOnWithoutNotify(true);
            _hapticToggle.SetIsOnWithoutNotify(true);
        }

        private void ToggleSound(bool isOn)
        {
            // Implement a real mute route if you add mixer groups; for now no-op
        }

        private void ToggleHaptic(bool isOn)
        {
            HapticManager.Instance.SetEnabled(isOn);
        }

        public override void Open()
        {
            SetGameState(true);
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        private void SetGameState(bool isPaused)
        {
            if (isPaused)
            {
                InputHandler.Instance.LockInput();
                _previousTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }
            else
            {
                InputHandler.Instance.UnlockInput();
                Time.timeScale = _previousTimeScale;
            }
        }

        protected override void OnOpenStart()
        {
            _closebutton.interactable = false;
            _background.gameObject.SetActive(true);
            _panel.gameObject.SetActive(true);
        }
        protected override void OnOpenComplete()
        {
            base.OnOpenComplete();
            _closebutton.interactable = true;
        }


        protected override void OnCloseStart()
        {
            base.OnCloseStart();
            SetGameState(false);
            _background.gameObject.SetActive(false);
            _panel.gameObject.SetActive(false);
        }
        protected override void OnCloseComplete()
        {
            base.OnCloseComplete();
        }
    }
}