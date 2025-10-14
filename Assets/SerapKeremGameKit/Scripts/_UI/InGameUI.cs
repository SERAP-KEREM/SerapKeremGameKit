using System.Collections;
using DG.Tweening;
using SerapKeremGameKit._Managers;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
	public class InGameUI : MonoBehaviour
	{
		[Title("Level")]
		[SerializeField] private TextMeshProUGUI _levelNumberText;

		[Title("Move Count")]
		[SerializeField] private TextMeshProUGUI _moveCountText;

		[Title("Retry")]
		[SerializeField] private RetryScreen _retryScreen;

		[Title("Buttons")]
		[SerializeField] private bool _askBeforeRestart = true;
		[SerializeField] private Button _restartButton;

		private void Awake()
		{
			_restartButton.onClick.AddListener(Restart);

			SetLevelNo(LevelManager.Instance.ActiveLevelNumber);
			//_warningText.gameObject.SetActive(false);
		}

		// Call this externally after level load/start if needed
		public void RefreshLevelNumber()
		{
			SetLevelNo(LevelManager.Instance.ActiveLevelNumber);
		}

		public void SetLevelNo(int levelNo)
		{
			if (_levelNumberText)
				_levelNumberText.SetText("LEVEL " + levelNo.ToString());
		}

		public void SetMoveCount(int moveCount)
		{
			if (_moveCountText)
				_moveCountText.SetText(moveCount.ToString());
		}

		private void Restart()
		{
			if (_askBeforeRestart)
			{
				_retryScreen.Show("Are you sure you want to restart?", "Restart", LevelManager.Instance.RestartLevel);
			}
			else
			{
				LevelManager.Instance.RestartLevel();
			}
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		//[Title("Warning")]
		//[SerializeField] private TextMeshProUGUI _warningText;

		//private Coroutine _warningCoroutine;
		//private float _lastWarningTime = -Mathf.Infinity;
		//private const float WARNING_COOLDOWN = 10f;
		//private const float FADE_DURATION = 0.4f;
		//private const float DISPLAY_DURATION = 2f;
		//public void PlayWarningText()
		//{
		//	if (Time.time < _lastWarningTime + WARNING_COOLDOWN)
		//		return;

		//	_lastWarningTime = Time.time;

		//	if (_warningCoroutine != null)
		//		StopCoroutine(_warningCoroutine);

		//	_warningCoroutine = StartCoroutine(PlayWarningTextRoutine());
		//}
		//private IEnumerator PlayWarningTextRoutine()
		//{
		//	_warningText.gameObject.SetActive(true);
		//	_warningText.alpha = 0f;

		//	yield return _warningText.DOFade(1f, FADE_DURATION).WaitForCompletion();

		//	yield return new WaitForSeconds(DISPLAY_DURATION);

		//	yield return _warningText.DOFade(0f, FADE_DURATION).WaitForCompletion();

		//	_warningCoroutine = null;
		//	_warningText.gameObject.SetActive(false);
		//}

		[Header("Capacity Bar")]
		[SerializeField] private Slider _capacitySlider;
		[SerializeField] private Image _fillImage;
		[SerializeField] private Color _barEmptyColor = Color.green;
		[SerializeField] private Color _barMidColor = Color.yellow;
		[SerializeField] private Color _barFullColor = Color.red;
		[SerializeField] private float _barLerpSpeed = 5f;

		private float _targetValue;
		[SerializeField] private TMP_Text _capacityText;

		public void UpdateCapacityBar(float value)
		{
			_targetValue = Mathf.Clamp01(value);

			if (_targetValue <= 0.5f)
			{
				float t = _targetValue / 0.5f;
				_fillImage.color = Color.Lerp(_barEmptyColor, _barMidColor, t);
			}
			else
			{
				float t = (_targetValue - 0.5f) / 0.5f;
				_fillImage.color = Color.Lerp(_barMidColor, _barFullColor, t);
			}

			if (_capacityText != null)
				_capacityText.text = Mathf.RoundToInt(_targetValue * 100f) + "%";
		}

		private void LateUpdate()
		{
			if (_capacitySlider != null)
				_capacitySlider.value = Mathf.Lerp(_capacitySlider.value, _targetValue, Time.deltaTime * _barLerpSpeed);
		}
	}
}