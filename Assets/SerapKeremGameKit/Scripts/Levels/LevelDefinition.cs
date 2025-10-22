using UnityEngine;

namespace SerapKeremGameKit._Levels
{
    [CreateAssetMenu(menuName = "SerapKeremGameKit/Levels/Level Definition", fileName = "LevelDefinition")]
    public sealed class LevelDefinition : ScriptableObject
    {
        [Min(0)]
        public int LevelIndex;

        [Tooltip("Time thresholds (seconds) for 3/2/1 stars. Lower time yields more stars.")]
        public float[] TimeThresholdsSec = new float[3] { 30f, 45f, 60f };

        // public int[] MoveThresholds = new int[3] { 10, 15, 20 }; // Moves reserved for future use

        [Header("Rewards")]
        public int WinCoins = 10;
        public int FailCoins = 0;
    }
}



