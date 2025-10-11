using SerapKeremGameKit._Camera;
using SerapKeremGameKit._InputSystem;
using SerapKeremGameKit._Logging;
using SerapKeremGameKit._Managers;
using System.Collections;
using TriInspector;
using UnityEngine;
using Array2DEditor;



namespace SerapKeremGameKit._LevelSystem
{
    public class Level : MonoBehaviour
    {

        // [Title("Coins Settings")] // coin settings can be added here if level-specific

		[Title("Grid Settings"), PropertyOrder(2)]
		[SerializeField] private Array2DInt _tileSizeArray;

        [ReadOnly]
        [SerializeField] private bool _isLevelWon;


        private Coroutine _winCoroutine;
        private Coroutine _loseCoroutine;

        // [SerializeField] private Transform _levelCameraPoint;

        // [SerializeField] private float _currentLevelSize = 0f;
        public virtual void Load()
        {
            gameObject.SetActive(true);
            Initialize();
        
        }
        private void Initialize()
        {
            StartCoroutine(InitializeCoroutine());

        }
        private IEnumerator InitializeCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            if (CameraManager.Instance == null)
            {
                TraceLogger.LogError("CameraManager.Instance is null! Cannot initialize camera position.");
                return;
            }

            //InitializeCameraPosition 
            //if (_levelCameraCampoint != null)
            //{
            //    CameraManager.Instance.InitializeCameraPosition(_levelCameraCampoint);
            //    RichLogger.Log($"Level Loaded: Camera positioned to {_levelCameraCampoint.name} in {gameObject.name}");
            //}
            //else
            //{
            //    RichLogger.LogWarning($"Level Load Warning: '_levelCameraCampoint' is not assigned for {gameObject.name}. Camera position might be incorrect.");
            //}

            //AdjustCameraByLevelSize
            //if (_currentLevelSize != 0)
            //{
            //    CameraManager.Instance.AdjustCameraByLevelSize(_currentLevelSize); 
            //    RichLogger.Log($"Level Loaded: Camera adjusted by level size: {_currentLevelSize} in {gameObject.name}");
            //}
            //else
            //{
            //    Debug.LogError("Level Load Error: CameraManager instance is not found! Ensure it's present and initialized.");
            //}
        }

        public virtual void Play()
        {
            InputHandler.Instance.UnlockInput();

        }

        public void CheckWinCondition()
        {
            if (_isLevelWon) return;

            _isLevelWon = true;
            _winCoroutine = StartCoroutine(WinCoroutine());
        }

        private IEnumerator WinCoroutine()
        {
            InputHandler.Instance.LockInput();
            yield return new WaitForSeconds(0.5f);
            // Example: reward coins for win
            // EconomyManager.Instance.AddCoins(10);
            LevelManager.Instance.Win();
        }

        public void CheckLoseCondition()
        {
            if (_loseCoroutine != null) return;

            _loseCoroutine = StartCoroutine(LoseCoroutine());
        }

        private IEnumerator LoseCoroutine()
        {
            InputHandler.Instance.LockInput();
            yield return new WaitForSeconds(0.5f);

            TraceLogger.Log("LOSE!!!");

            LevelManager.Instance.Lose();
        }

    }
}