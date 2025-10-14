using System.Collections.Generic;
using DG.Tweening;
using SerapKeremGameKit._Managers;
using SerapKeremGameKit._UI;
using UnityEngine;

namespace SerapKeremGameKit._Feature
{
    public class FeatureManager : MonoBehaviour
    {
        [SerializeField] private List<FeatureData> _featureDataList = new List<FeatureData>();
        [SerializeField] private FeatureScreen _featureScreen;

        public bool HasFeature(int level)
        {
            foreach (var f in _featureDataList)
            {
                if (level >= f.StartLevel && level <= f.UnlockLevel)
                    return true;
            }
            return false;
        }

        private void OnEnable()
        {
            // No LevelManager events in template; drive via small delay and loading screen hook
            DOVirtual.DelayedCall(0.1f, () =>
            {
                if (LoadingScreen.IsLoadingActive)
                {
                    LoadingScreen.OnLoadingCompleted += OnLoadingCompleted;
                }
                else
                {
                    ShowFeatureForCurrentLevel();
                }
            });
        }

        private void OnDisable()
        {
            LoadingScreen.OnLoadingCompleted -= OnLoadingCompleted;
        }

        private void OnLoadingCompleted()
        {
            LoadingScreen.OnLoadingCompleted -= OnLoadingCompleted;
            ShowFeatureForCurrentLevel();
        }

        private void ShowFeatureForCurrentLevel()
        {
            int currentLevel = LevelManager.Instance.ActiveLevelNumber;
            ShowFeatureForLevel(currentLevel);
        }

        public void ShowFeatureForLevel(int level)
        {
            if (_featureScreen == null)
            {
                Debug.LogWarning("FeatureScreen reference is null!");
                return;
            }

            if (TryGetFeature(level, out FeatureData feature))
            {
                string key = "Feature_Shown_" + feature.UnlockLevel;
                if (PlayerPrefs.HasKey(key))
                    return;

                _featureScreen.ShowFeature(feature.Title, feature.Description, feature.FeatureSprite);
                PlayerPrefs.SetInt(key, 1);
                PlayerPrefs.Save();
            }
        }

        private bool TryGetFeature(int level, out FeatureData feature)
        {
            foreach (var f in _featureDataList)
            {
                if (level == f.UnlockLevel)
                {
                    feature = f;
                    return true;
                }
            }

            feature = null;
            return false;
        }

        public bool TryGetFeatureForUI(int level, out string title, out string description, out Sprite sprite, out float progress)
        {
            foreach (var f in _featureDataList)
            {
                if (level >= f.StartLevel && level <= f.UnlockLevel)
                {
                    title = f.Title;
                    description = f.Description;
                    sprite = f.FeatureSprite;

                    int range = f.UnlockLevel - f.StartLevel;
                    if (range > 0)
                    {
                        // Clamp between 0.1 and <1.0
                        float rawProgress = (float)(level - f.StartLevel) / range;
                        progress = Mathf.Clamp01(rawProgress);

                        // Ensure start level has some fill (e.g. 10%) and unlock level never reaches full (e.g. 95%)
                        progress = Mathf.Lerp(0.1f, 0.95f, progress);
                    }
                    else
                    {
                        progress = 1f;
                    }

                    return true;
                }
            }

            title = string.Empty;
            description = string.Empty;
            sprite = null;
            progress = 0f;
            return false;
        }
    }

    [System.Serializable]
    public class FeatureData
    {
        [SerializeField] private int _startLevel = 1;
        [SerializeField] private int _unlockLevel = 1;
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _featureSprite;

        public int StartLevel => _startLevel;
        public int UnlockLevel => _unlockLevel;
        public string Title => _title;
        public string Description => _description;
        public Sprite FeatureSprite => _featureSprite;
    }
}