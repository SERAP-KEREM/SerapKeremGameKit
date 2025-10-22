using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameKit._Levels
{
    [CreateAssetMenu(menuName = "SerapKeremGameKit/Levels/Level Database", fileName = "LevelDatabase")]
    public sealed class LevelDatabase : ScriptableObject
    {
        public List<LevelDefinition> Levels = new List<LevelDefinition>();

        public LevelDefinition GetByLevelIndex(int levelIndex)
        {
            return Levels.Find(x => x != null && x.LevelIndex == levelIndex);
        }
    }
}



